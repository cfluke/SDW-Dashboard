using System.Collections.Generic;
using SerializableData;
using UnityEngine;

namespace DiscoveryWall
{
    public class DiscoveryWall : MonoBehaviour
    {
        [SerializeField] private KeckDisplay[] keckDisplays;

        public void Clear()
        {
            foreach (KeckDisplay keckDisplay in keckDisplays)
                keckDisplay.Clear();
        }

        public void Set(DiscoveryWallSerializable discoveryWallData)
        {
            for (int i = 0; i < discoveryWallData.keckDisplays.Length; i++)
            {
                var keckDisplay = keckDisplays[i];
                var keckDisplayData = discoveryWallData.keckDisplays[i];
                for (int j = 0; j < keckDisplayData.monitors.Length; j++)
                {
                    var monitor = keckDisplay.GetMonitors()[j];
                    var monitorData = keckDisplayData.monitors[j];
                    for (int k = 0; k < monitorData.apps.Length; k++)
                    {
                        var appData = monitorData.apps[k];
                        monitor.AddApp(appData.path, appData.x, appData.y, appData.w, appData.h);
                    }
                }
            }
        }

        public DiscoveryWallSerializable GetSerializable()
        {
            List<KeckDisplaySerializable> k = new List<KeckDisplaySerializable>();
            foreach (KeckDisplay keckDisplay in keckDisplays)
            {
                k.Add(keckDisplay.GetSerializable());
            }
            return new DiscoveryWallSerializable(k);
        }
    }
}