using System.Collections;
using System.Collections.Generic;
using AppManagement;
using UnityEngine;

namespace DiscoveryWall
{
    public class MonitorManager : MonoBehaviour
    {
        [SerializeField] private Monitor[] monitors;

        /// <summary>
        /// connected to the "Load" button, opens a file picker to load a discovery wall config
        /// </summary>
        public void Import()
        {
            // use importer class which opens a file picker using the SFB library
            DiscoveryWallConfigImporter importer = new DiscoveryWallConfigImporter();
            DiscoveryWallConfig config = importer.Import();
            
            // handle invalid import (i.e. user cancels importing)
            if (config == null)
                return;
            
            ClearMonitors();
            UpdateMonitors(config);
        }

        /// <summary>
        /// connected to the "Save" button, opens a file picker to save a discovery wall config
        /// </summary>
        public void Export()
        {
            DiscoveryWallConfig config = CheckMonitors();
            
            // use exporter class which opens a file picker using the SFB library
            DiscoveryWallConfigExporter exporter = new DiscoveryWallConfigExporter();
            exporter.Export(config);
        }

        private void ClearMonitors()
        {
            foreach (Monitor monitor in monitors)
                monitor.Clear();
        }

        private void UpdateMonitors(DiscoveryWallConfig config)
        {
            List<App> apps = new List<App>();
            for (int i = 0; i < 5; i++)
            {
                KeckDisplayConfig keckDisplay = config.keckDisplays[i];
                apps.Add(keckDisplay.monitor1);
                apps.Add(keckDisplay.monitor2);
            }

            for (int i = 0; i < 10; i++)
            {
                App app = apps[i];
                if (app.path != "") 
                    monitors[i].NewApp(apps[i]);
            }
        }
        
        private DiscoveryWallConfig CheckMonitors()
        {
            List<App> apps = new List<App>();
            foreach (Monitor monitor in monitors)
                apps.Add(monitor.AppDraggable ? monitor.AppDraggable.App : null);

            DiscoveryWallConfig config = new DiscoveryWallConfig();
            for (int i = 0; i < 5; i++)
                config.SetKeckDisplay(i + 1, apps[i * 2], apps[i * 2 + 1]);
            return config;
        }
    }
}