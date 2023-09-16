using System.Collections;
using System.Collections.Generic;
using SerializableData;
using TMPro;
using UnityEngine;

namespace DiscoveryWall
{
    public class KeckDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject monitorPrefab;
        [SerializeField] private TMP_Text id;
        [SerializeField] private TMP_Text ip;
        
        private string _id;
        private string _ip;
        private List<Monitor> _monitors;
        
        public void StartApps()
        {
            foreach (Monitor monitor in _monitors)
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
                        MessageType = MessageTypes.StartApp // <- change to "AppStart" or something?
                    };
                    
                    // send
                    Debug.Log("Sending " + json + " to " + _id);
                    if (!TCPHandler.SendMessage(_id, message))
                    {
                        Debug.Log("Client doesn't exist!");
                    }
                }
            }
        }
        
        
        public void StopApps()
        {
            // create TCP message
            ServerToClientMessage message = new ServerToClientMessage
            {
                payload = "",
                MessageType = MessageTypes.StopApps
            };
        
            // send
            Debug.Log("Sending " + message.MessageType + " to " + _id);
            if (!TCPHandler.SendMessage(_id, message))
            {
                Debug.Log("Client doesn't exist!");
            }
        }

        public void Init(KeckDisplaySerializable keckDisplayData)
        {
            _id = keckDisplayData.id;
            _ip = keckDisplayData.ip;
            _monitors = new List<Monitor>();

            foreach (MonitorSerializable m in keckDisplayData.monitors)
            {
                GameObject monitorObject = Instantiate(monitorPrefab, transform);
                Monitor monitor = monitorObject.GetComponent<Monitor>();
                monitor.Init(m);
            
                // remember monitor
                _monitors.Add(monitor);
            }

            // update UI elements
            id.text = _id;
            ip.text = _ip;
        }
        
        public void Clear()
        {
            foreach (Monitor monitor in _monitors)
                monitor.Clear();
        }

        public KeckDisplaySerializable GetSerializable()
        {
            List<MonitorSerializable> m = new List<MonitorSerializable>();
            foreach (Monitor monitor in _monitors)
                m.Add(monitor.GetSerializable());
            return new KeckDisplaySerializable(_id, _ip, m);
        }
    }
}