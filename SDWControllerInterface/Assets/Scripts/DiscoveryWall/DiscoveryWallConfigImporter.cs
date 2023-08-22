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
        public DiscoveryWallSerializable Import(string path)
        {
            // use the StandaloneFileBrowser tool to open a native file browser
            string[] filePath = null; // = StandaloneFileBrowser.OpenFilePanel("Import Discovery Wall Config", "", "config", false);
            if (filePath.Length == 0)
                return null;

            // file IO shenanigans
            string json = File.ReadAllText(filePath[0]);

            // deserialize the config from json to DiscoveryWallConfig object
            return JsonUtility.FromJson<DiscoveryWallSerializable>(json);
        }
    }
}