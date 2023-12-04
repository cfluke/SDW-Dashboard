using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DialogManagement;
using DialogManagement.FileExplorer;
using JetBrains.Annotations;
using SerializableData;
using SSH;
using UnityEngine;
using Widgets;
using Logger = Logs.Logger;

namespace DiscoveryWall
{
    public class DiscoveryWallConfigManager : MonoBehaviour
    {
        public Dictionary<string, KeckDisplayData> _keckDisplayData = new();
        [CanBeNull] public KeckDisplayData GetKeckDisplayData(string keckDisplayId)
        {
            return _keckDisplayData.TryGetValue(keckDisplayId, out var data) ? data : null;
        }

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

            foreach (KeckDisplayData keckDisplayData in discoveryWallData.keckDisplays)
            {
                    // SSH 
                    string id = keckDisplayData.id;
                    string ip = keckDisplayData.ip;
                    Logger.Instance.Log(keckDisplayData.ip);
                    string username = "localuser";
                    string password = "localuser";
                    string listenerPath = "mnt/nfs/home/localuser/SSALab/Listener/Stable";
                    SSHManager.Instance.LaunchClient(id, ip, username, password, listenerPath);
                    _keckDisplayData.Add(id, keckDisplayData);
            }

            // need to do instantiation and object stuff on the Main thread or TODO: get rid of this and listen for IDENTIFY connections
            MainThreadDispatcher.Instance.Enqueue(() =>
            {
                // populate discovery wall and widgets with the imported data
                DiscoveryWall discoveryWall = DiscoveryWall.Instance;
                WidgetManager widgetManager = WidgetManager.Instance;
                discoveryWall.Destroy();
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