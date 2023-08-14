using System.Collections;
using System.Collections.Generic;
using AppManagement;
using UnityEngine;

namespace DiscoveryWall
{
    public class MonitorManager : MonoBehaviour
    {
        [SerializeField] private Monitor[] monitors;

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