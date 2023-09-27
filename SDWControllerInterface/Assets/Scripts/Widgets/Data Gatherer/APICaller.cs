using CsvHelper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
//using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net.Http;
//using SixLabors.ImageSharp;
using System.Collections;
//using SixLabors.ImageSharp.Formats.Gif;
//using SixLabors.ImageSharp.Formats;
using System.Net.Http.Headers;
using System;

namespace SSADataStreams
{
    public static class Globals
    {
        public static readonly bool isDebugMode = false;
    }
    internal abstract class APICaller
    {
        private string _APICallerName;
        public string APICallerName { get => _APICallerName; set => _APICallerName = value; }
        protected APICaller(string APICallerName) {
            _APICallerName = APICallerName;
        }
        //Abstract function for implementing data source specific function
        public abstract Task<string> CallAPIAsync();
        //Reads in text files with API endpoints from folder under data source name
        public Dictionary<string, List<string>> ReadEndpointFiles(string subFolderName = "")
        {
            string folderPath = ".\\Endpoints\\" + this.APICallerName + "\\";
            if (subFolderName != "")
            {
                folderPath += subFolderName + "\\";
            }
            //Log path to read if in debug mode
            if (Globals.isDebugMode)
            {
                Console.WriteLine("DEBUG Reading path: " + folderPath);
            }
            Dictionary<string, List<string>> endpointsMap = new Dictionary<string, List<string>>();

            //Iterate through directories and get files recursively
            foreach (string directory in Directory.EnumerateDirectories(folderPath))
            {
                string subFolder = Path.GetFileName(directory);
                if (subFolderName != "")
                {
                    subFolder = subFolderName + "\\" + subFolder;
                }
                Dictionary<string, List<string>> returnedEndpoints = ReadEndpointFiles(subFolder);
                foreach (string endpointFile in returnedEndpoints.Keys)
                {
                    endpointsMap.Add(endpointFile, returnedEndpoints[endpointFile]);

                }
            }
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.txt"))
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                //Add all lines of text file to endpoint list
                if (endpointsMap.ContainsKey(subFolderName + fileName))
                {
                    endpointsMap[subFolderName + "\\" + fileName].AddRange(File.ReadAllLines(file).ToList());
                }
                else
                {
                    endpointsMap.Add(subFolderName + "\\" + fileName, File.ReadAllLines(file).ToList());
                }
            }

            if (subFolderName == "")
            {
                int endpointCount = 0;
                //Log read endpoints if top level and in degbug mode
                if (Globals.isDebugMode) { Console.WriteLine("DEBUG API endpoint map:"); }
                foreach (string key in endpointsMap.Keys)
                {
                    if (Globals.isDebugMode) { Console.WriteLine($"\t{key}"); }
                    foreach (string value in endpointsMap[key])
                    {
                        if (Globals.isDebugMode) { Console.WriteLine($"\t\t{value}"); }
                        endpointCount++;
                    }
                }
                //Log endpoint count if top level
                Console.WriteLine($"Successfully read in {endpointCount} API endpoints...");
            }
            return endpointsMap;
        }
        //Called by CallAPIAsync to call all API endpoints
        public async Task<string> CallAPIEndpointsAsync(List<string> endpoints, HttpMethod httpMethod, string JSONOptions = "", string subFolder = "")
        {
            //Create an HttpClient instance (Created here as HttpClients should be reused)
            HttpClient httpClient = new HttpClient();
            foreach (string endpoint in endpoints)
            {
                //Log URL
                Console.WriteLine("Pulling data from: " + endpoint);

                //Send the GET/POST request
                HttpResponseMessage response;
                if (httpMethod == HttpMethod.Post)
                {
                    //Create the HTTP request content
                    StringContent content = new StringContent(JSONOptions, Encoding.UTF8, "application/json");
                    response = await httpClient.PostAsync(endpoint, content);
                }
                else
                {
                    response = await httpClient.GetAsync(endpoint);
                }
                //Get media type from API endpoint as well as allowed types to compare to
                MediaTypeHeaderValue jsonMediaHeaderType = MediaTypeHeaderValue.Parse("application/json");
                MediaTypeHeaderValue imageMediaHeaderType = MediaTypeHeaderValue.Parse("image/gif");
                MediaTypeHeaderValue responseMediaHeaderType = response.Content.Headers.ContentType;
                if (Globals.isDebugMode) { Console.WriteLine("DEBUG Content type is " + responseMediaHeaderType); }
                if (Equals(jsonMediaHeaderType, responseMediaHeaderType))
                {
                    await CallAPITextEndpointsAsync(response, endpoint, subFolder);
                }
                else if (Equals(imageMediaHeaderType, responseMediaHeaderType))
                {
                    //CallAPIImageEndpointsAsync(response, endpoint, subFolder);
                }
            }
            return "";
        }
        //If endpoint type is text then this function will be used to get data
        public async Task<string> CallAPITextEndpointsAsync(HttpResponseMessage response, string endpoint, string subFolder = "")
        {
            string responseString = string.Empty;
            //Read the response
            string responseJson = await response.Content.ReadAsStringAsync();
            //Check status code
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error occured while contacting " + endpoint + ": " + responseJson);
                responseString = "Error occured while contacting " + endpoint + ": " + responseJson;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully pulled data from " + endpoint);
                responseString = responseJson;
            }
            //Reset console colour from red/green
            Console.ResetColor();
            //Write to file using StreamWriter
            if (subFolder != "")
            {
                subFolder = "\\" + subFolder;
            }
            string fileName = Path.GetFileNameWithoutExtension(endpoint);

            WriteCSVFile(fileName, APICallerName + subFolder, responseJson);
            return responseString;
        }
        //REMOVED DUE TO IMAGESHARP BEING INCOMPATIBLE WITH UNITY
        //If endpoint type is image then this function will be used to get data
        /*public async Task<string> CallAPIImageEndpointsAsync(HttpResponseMessage response, string endpoint, string subFolder = "")
        {
            try
            {
                string responseString = string.Empty;
                //Read the response stream
                Stream stream = await response.Content.ReadAsStreamAsync();
                byte[] imageData = new byte[stream.Length];
                stream.Read(imageData, 0, (int)stream.Length);
                var image = Image.Load<Rgba32>(imageData);
                //Check status code
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Error occured while contacting " + endpoint + ": " + image);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully pulled data from " + endpoint);
                }
                //Reset console colour from red/green
                Console.ResetColor();
                //Write to file using StreamWriter
                if (subFolder != "")
                {
                    subFolder = "\\" + subFolder;
                }
                string fileName = Path.GetFileName(endpoint);
                string directoryPath = ".\\Data\\CurrentRun\\" + APICallerName + "\\" + subFolder + "\\";
                DirectoryInfo directoryInfo = Directory.CreateDirectory(directoryPath);
                //Save image to file
                image.Save(directoryPath + fileName);
                return responseString;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ex.ToString();
            }
        }*/
        //Converts collected JSON data to a CSV file to be saved
        public static void WriteCSVFile(string fileName, string folderName, string jsonContent)
        {
            //Console.WriteLine(jsonContent);
            //NewtonSoft json nuget package
            XmlNode xml = JsonConvert.DeserializeXmlNode("{records:{record:[" + jsonContent + "]}}");
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml.InnerXml);
            XmlReader xmlReader = new XmlNodeReader(xml);
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(xmlReader);
            //Check if data was returned from API
            DataTable dataTable = new DataTable();
            dataTable = dataSet.Tables[dataSet.Tables.Count - 1];
            //Datatable to CSV
            var lines = new List<string>();
            string[] columnNames = dataTable.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            var header = string.Join(",", columnNames);
            lines.Add(header);
            var valueLines = dataTable.AsEnumerable()
                               .Select(row => string.Join(",", row.ItemArray));
            lines.AddRange(valueLines);
            string directoryPath = ".\\Data\\CurrentRun\\" + folderName + "\\";
            DirectoryInfo directoryInfo = Directory.CreateDirectory(directoryPath);
            File.WriteAllLines(directoryPath + fileName + ".csv", lines);
        }
    }
}
