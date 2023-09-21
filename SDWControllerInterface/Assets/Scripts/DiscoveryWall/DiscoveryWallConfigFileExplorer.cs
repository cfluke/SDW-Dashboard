using System.Threading.Tasks;
using DialogManagement;
using FileExplorer;
using SerializableData;
using UnityEngine;

namespace DiscoveryWall
{
    public class DiscoveryWallConfigFileExplorer : MonoBehaviour
    {
        private DiscoveryWall _discoveryWall;
        private string path;

        public async void SaveDialog()
        {
            path = await Open(FileExplorerDialogType.Save);
            
            _discoveryWall = FindObjectOfType<DiscoveryWall>();
            DiscoveryWallConfigExporter sdwExporter = new DiscoveryWallConfigExporter();
            sdwExporter.Export(_discoveryWall.GetSerializable(), path);
        }

        public async void OpenDialog()
        {
            // get path
            path = await Open(FileExplorerDialogType.Open);

            if (!string.IsNullOrEmpty(path))
            {
                _discoveryWall = FindObjectOfType<DiscoveryWall>();
                DiscoveryWallConfigImporter sdwImporter = new DiscoveryWallConfigImporter(); 
                DiscoveryWallSerializable discoveryWallData = sdwImporter.Import(path);
                _discoveryWall.Clear();
                _discoveryWall.Populate(discoveryWallData);
            }
        }

        private async Task<string> Open(FileExplorerDialogType dialogType)
        {
            FileExplorerArgs args = new FileExplorerArgs
            {
                DialogType = dialogType,
                Directory = "/",
                Extension = "*.sdw"
            };
            
            return await DialogManager.Instance.OpenFileDialog<string, FileExplorerArgs>(args);
        }
    }
}