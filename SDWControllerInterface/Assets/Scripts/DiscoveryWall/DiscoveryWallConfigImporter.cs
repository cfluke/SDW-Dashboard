using System.IO;
using SerializableData;
using Newtonsoft.Json;

namespace DiscoveryWall
{
    public class DiscoveryWallConfigImporter
    {
        public SDWConfigData Import(string path)
        {
            // file IO shenanigans
            string json = File.ReadAllText(path);

            // deserialize the config from json to DiscoveryWallConfig object
            return JsonConvert.DeserializeObject<SDWConfigData>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
}