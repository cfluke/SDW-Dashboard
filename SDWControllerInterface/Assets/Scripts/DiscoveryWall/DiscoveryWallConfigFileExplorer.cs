using System.Threading.Tasks;
using DialogManagement;
using DialogManagement.FileExplorer;
using SerializableData;
using SSH;
using UnityEngine;
using Widgets;
using Logger = Logs.Logger;

namespace DiscoveryWall
{
    public class DiscoveryWallConfigFileExplorer : MonoBehaviour
    {
        public async void SaveDialog()
        {
            string path = await Open(FileExplorerDialogType.Save);
            if (string.IsNullOrEmpty(path))
                return; // ignore if no path

            // create serializable data class for export
            DiscoveryWall discoveryWall = DiscoveryWall.Instance;
            WidgetManager widgetManager = WidgetManager.Instance;
            SDWConfigData sdwConfig = new SDWConfigData
            {
                discoveryWallData = discoveryWall.Serialize(),
                widgetsData = widgetManager.Serialize()
            };

            // perform export
            DiscoveryWallConfigExporter sdwExporter = new DiscoveryWallConfigExporter();
            sdwExporter.Export(sdwConfig, path);
            
            Logger.Instance.LogSuccess("ExportConfig: Successfully saved .sdw");
        }

        public async void OpenDialog()
        {
            string path = await Open(FileExplorerDialogType.Open);
            if (string.IsNullOrEmpty(path))
                return; // ignore if no path

            // perform import
            DiscoveryWallConfigImporter sdwImporter = new DiscoveryWallConfigImporter();
            SDWConfigData sdwConfig = sdwImporter.Import(path);
            DiscoveryWallData discoveryWallData = sdwConfig.discoveryWallData;
            WidgetsData widgetData = sdwConfig.widgetsData;

            /*foreach (KeckDisplaySerializable keckDisplayData in discoveryWallData.keckDisplays)
                {
                    // SSH 
                    //string ip = keckDisplayData.ip;
                    string ip = "136.186.110.12";
                    string username = "localuser";
                    string password = "localuser";
                    string path = "/mnt/nfs/home/localuser/SSALab/Listener/Stable/";
                    SSHManager.Instance.LaunchClient(ip, username, password, path);
                }*/

            // need to do instantiation and object stuff on the Main thread or TODO: get rid of this and listen for IDENTIFY connections
            MainThreadDispatcher.Instance.Enqueue(() =>
            {
                // populate discovery wall and widgets with the imported data
                DiscoveryWall discoveryWall = DiscoveryWall.Instance;
                WidgetManager widgetManager = WidgetManager.Instance;
                discoveryWall.Populate(discoveryWallData);
                widgetManager.Populate(widgetData);

                Logger.Instance.LogSuccess("ImportConfig: Opened .sdw");
            });
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