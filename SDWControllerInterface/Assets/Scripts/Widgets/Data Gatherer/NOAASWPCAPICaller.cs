using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SSADataStreams
{
    internal class NOAASWPCAPICaller : APICaller
    {
        //Implement the caller as an extension of APICaller with name
        public NOAASWPCAPICaller(): base("NOAA")
        {

        }
        public override async Task<string> CallAPIAsync()
        {
            try
            {
                //The endpoints can return blank if there is no alert/warning
                Dictionary<string, List<string>> endpointSubpageMap = ReadEndpointFiles();
                string responseLastText = string.Empty;
                //List<Image> responseLastImage = new List<Image>();
                //Call each API endpoint async
                foreach (string subpage in endpointSubpageMap.Keys)
                {
                    //Blank string on end as NOAA does not need authentication yet
                    responseLastText = await CallAPIEndpointsAsync(endpointSubpageMap.GetValueOrDefault(subpage), HttpMethod.Get, "", subpage);
                }
                return responseLastText;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.ToString();
            }
        }
    }
}
