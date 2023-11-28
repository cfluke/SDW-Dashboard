using System.IO;
using SerializableData;
using Newtonsoft.Json;

namespace DiscoveryWall
{
    public class DiscoveryWallConfigExporter
    {
        public void Export(SDWConfigData data, string path)
        {
            // serialize config object to json
            string json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            // file IO to write to desired file
            File.WriteAllText(path, json);
        }
    }
}