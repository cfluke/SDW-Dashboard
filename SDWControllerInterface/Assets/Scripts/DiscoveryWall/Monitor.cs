using System.Collections.Generic;
using SerializableData;
using UnityEngine;

namespace DiscoveryWall
{
    public class Monitor : MonoBehaviour
    {
        private List<AppSerializable> _apps;

        private void Start()
        {
            _apps = new List<AppSerializable>();
        }
        
        public void Clear()
        {
            AppLayout.AppLayout appLayout = GetComponentInChildren<AppLayout.AppLayout>();
            if (appLayout != null)
                appLayout.Clear();
            
            _apps.Clear();
        }

        public MonitorSerializable GetSerializable()
        {
            return new MonitorSerializable(_apps);
        }

        public AppSerializable[] GetSerializableApps()
        {
            return _apps.ToArray();
        }

        public void AddApp(string path, float x, float y, float w, float h)
        {
            int xVal = (int)(x * 3840);
            int yVal = (int)(y * 2160);
            int width = (int)(w * 3840);
            int height = (int)(h * 2160);
            
            AppSerializable appSerializable = new AppSerializable(path, xVal, yVal, width, height);
            _apps.Add(appSerializable);
        }
    }
}
