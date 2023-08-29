using System.Collections;
using System.Collections.Generic;
using SerializableData;
using UnityEngine;

namespace DiscoveryWall
{
    public class KeckDisplay : MonoBehaviour
    {
        [SerializeField] private Monitor[] monitors;
        [SerializeField] private string id;
        
        public void StartApps()
        {
            
            foreach (Monitor monitor in monitors)
            {
                AppSerializable[] apps = monitor.GetSerializableApps();

                foreach (AppSerializable app in apps)
                {
                    if (app == null)
                        continue;
                    
                    // serialize app object to json
                    string json = JsonUtility.ToJson(app);
                    
                    // create TCP message
                    ServerToClientMessage message = new ServerToClientMessage
                    {
                        payload = json,
                        messageType = MessageTypes.Echo // <- change to "AppStart" or something?
                    };
                    
                    // send
                    Debug.Log("Attempting to send " + json + " to " + id);
                    if (!TCPHandler.SendMessage(id, message))
                    {
                        Debug.Log("Client doesn't exist!");
                    }
                }
            }
        }

        public void Populate(KeckDisplaySerializable keckDisplayData)
        {
            for (int i = 0; i < monitors.Length; i++)
            {
                Monitor monitor = monitors[i];
                MonitorSerializable monitorData = keckDisplayData.monitors[i];
                monitor.Populate(monitorData);
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
    }
}