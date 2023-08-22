using System.Threading.Tasks;
using DialogManagement;
using FileExplorer;
using SerializableData;
using UnityEditor.VersionControl;
using UnityEngine;

namespace DiscoveryWall
{
    public class DiscoveryWallConfigFileExplorer : MonoBehaviour
    {
        [SerializeField] private DialogManager dialogManager;
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
            
            _discoveryWall = FindObjectOfType<DiscoveryWall>();
            DiscoveryWallConfigImporter sdwImporter = new DiscoveryWallConfigImporter(); 
            DiscoveryWallSerializable discoveryWallData = sdwImporter.Import(path);
            _discoveryWall.Clear();
            _discoveryWall.Set(discoveryWallData);
        }

        private async Task<string> Open(FileExplorerDialogType dialogType)
        {
            FileExplorerArgs args = new FileExplorerArgs
            {
                DialogType = dialogType,
                Directory = "/",
                Extension = "*.sdw"
            };
            
            return await dialogManager.OpenFileDialog<string, FileExplorerArgs>(args);
        }
    }
}