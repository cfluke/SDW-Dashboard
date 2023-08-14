using DialogManagement;
using FileExplorer;
using UnityEngine;

namespace DiscoveryWall
{
    public class DiscoveryWallConfigFileExplorer : MonoBehaviour
    {
        [SerializeField] private DialogManager dialogManager;

        public void SaveDialog()
        {
            Open(FileExplorerDialogType.Save);
        }

        public void OpenDialog()
        {
            Open(FileExplorerDialogType.Open);
        }

        private async void Open(FileExplorerDialogType dialogType)
        {
            FileExplorerArgs args = new FileExplorerArgs
            {
                DialogType = dialogType,
                Directory = "/",
                Extension = "*.sdw"
            };
            
            string path = await dialogManager.OpenFileDialog<string, FileExplorerArgs>(args);
            Debug.Log(path);
        }
    }
}