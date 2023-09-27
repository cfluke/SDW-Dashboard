using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSADataStreams
{
    internal class SpaceWeatherServiceAPICaller : APICaller
    {
        public SpaceWeatherServiceAPICaller(): base("SpaceWeather")
        {

        }
        public override async Task<string> CallAPIAsync()
        {
            try
            {
                //API endpoints that require options
                List<string> APIEndpointsWithOptions = new List<string>
                {
                    "get-a-index",
                    "get-k-index",
                    "get-dst-index",
                };
                //API endpoints that only require an API key
                //The endpoints can return blank if there is no alert/warning
                List<string> APIEndpointsNoOptions = new List<string>
                {
                    "get-mag-alert",
                    "get-mag-warning",
                    "get-aurora-alert",
                    "get-aurora-watch",
                    "get-aurora-outlook"
                };
                //Options map with API key
                string APIKeyOnly = @"{
                ""api_key"": ""710f9362-83c8-4c2e-92fb-2baf5ab8825d""
		        }";
                //Options map with API key and location
                string optionsReduced = @"{
                ""api_key"": ""710f9362-83c8-4c2e-92fb-2baf5ab8825d"",
                ""options"": {
                    ""location"": ""Australian region""
                    }
		        }";
                //Options map with full options
                string optionsFull = @"{
                ""api_key"": ""710f9362-83c8-4c2e-92fb-2baf5ab8825d"",
                ""options"": {
                    ""location"": ""Australian region"",
                    ""start"": ""prop1"",
                    ""end"": ""prop1""
                    }
		        }";
                List<String> returnList = new List<String>();
                //Call each API endpoint async
                //returnList.Concat(await CallAPITextEndpointsAsync("https://sws-data.sws.bom.gov.au/api/v1/", APIEndpointsWithOptions, HttpMethod.Post, optionsReduced));
                //returnList.Concat(await CallAPITextEndpointsAsync("https://sws-data.sws.bom.gov.au/api/v1/", APIEndpointsNoOptions, HttpMethod.Post, APIKeyOnly));
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.ToString();
            }
        }
    }
}
