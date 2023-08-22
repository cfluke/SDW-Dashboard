using System.Collections;
using System.Collections.Generic;
using SerializableData;
using UnityEngine;

namespace DiscoveryWall
{
    public class KeckDisplay : MonoBehaviour
    {
        [SerializeField] private Monitor[] monitors;

        public void StartApps()
        {
            KeckDisplaySerializable keckDisplaySerializable = GetSerializable();
            foreach (Monitor monitor in monitors)
            {
                AppSerializable[] apps = monitor.GetSerializableApps();

                foreach (AppSerializable app in apps)
                {
                    // serialize config object to json
                    string json = JsonUtility.ToJson(app);
                    Debug.Log(json);
                }
            }
        }
        
        public void Clear()
        {
            foreach (Monitor monitor in monitors)
                monitor.Clear();
        }

        public KeckDisplaySerializable GetSerializable()
        {
            List<MonitorSerializable> m = new List<MonitorSerializable>();
            foreach (Monitor monitor in monitors)
                m.Add(monitor.GetSerializable());
            return new KeckDisplaySerializable(m);
        }

        public Monitor[] GetMonitors()
        {
            return monitors;
        }
    }
}