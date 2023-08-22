using UnityEngine;
using System.IO;
using SerializableData;

namespace DiscoveryWall
{
    public class DiscoveryWallConfigExporter
    {
        /// <summary>
        /// opens a file picker window to allow the user to create/save a new/existing SDW configuration
        /// </summary>
        /// <returns>true if the export was successful, otherwise false</returns>
        public bool Export(DiscoveryWallSerializable serializable, string path)
        {
            // use the StandaloneFileBrowser tool to open a native file browser
            string filePath = null; // = StandaloneFileBrowser.SaveFilePanel("Export Discovery Wall Config", "", "", "config");
            if (filePath.Length == 0)
                return false;
            
            // serialize config object to json
            string json = JsonUtility.ToJson(serializable);

            // file IO to write to desired file
            File.WriteAllText(filePath, json);
            return true;
        }
    }
}