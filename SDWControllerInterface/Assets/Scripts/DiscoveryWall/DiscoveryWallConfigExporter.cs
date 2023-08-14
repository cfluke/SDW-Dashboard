using UnityEngine;
using System.IO;

namespace DiscoveryWall
{
    public class DiscoveryWallConfigExporter
    {
        /// <summary>
        /// opens a file picker window to allow the user to create/save a new/existing SDW configuration
        /// </summary>
        /// <returns>true if the export was successful, otherwise false</returns>
        public bool Export(DiscoveryWallConfig config, string path)
        {
            // serialize config object to json
            string json = JsonUtility.ToJson(config);

            // file IO to write to desired file
            File.WriteAllText(path, json);
            return true;
        }
    }
}