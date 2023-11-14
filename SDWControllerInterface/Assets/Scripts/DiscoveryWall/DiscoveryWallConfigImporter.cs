using System.IO;
using SerializableData;
using UnityEngine;

namespace DiscoveryWall
{
    public class DiscoveryWallConfigImporter
    {
        /// <summary>
        /// opens a file picker window to allow the user to select and import a file to set the SDW configuration
        /// </summary>
        /// <returns>a DiscoveryWallConfig object, otherwise null</returns>
        public DiscoveryWallData Import(string path)
        {
            // file IO shenanigans
            string json = File.ReadAllText(path);

            // deserialize the config from json to DiscoveryWallConfig object
            return JsonUtility.FromJson<DiscoveryWallData>(json);
        }
    }
}