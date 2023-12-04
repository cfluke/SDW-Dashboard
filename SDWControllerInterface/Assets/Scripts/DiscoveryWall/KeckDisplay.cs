using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SerializableData;
using TMPro;
using UnityEngine;
using Logger = Logs.Logger;

namespace DiscoveryWall
{
    public class KeckDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject monitorPrefab;
        [SerializeField] private TMP_Text id;
        
        private string _id;
        private string _ip;
        private string _path;
        private List<Monitor> _monitors;

        public void ToggleApps(bool isOn)
        {
            if (isOn)
                StartApps();
            else 
                StopApps();
        }

        private void StartApps()
        {
            foreach (Monitor monitor in _monitors)
            {
                AppData[] apps = monitor.GetSerializableApps();

                foreach (AppData app in apps)
                {
                    if (app == null)
                        continue; // skip null AppData (since not every slot is guaranteed to be filled with an app)

                    // TODO: fix this and ensure that these extra firefox args are not visible to the user
                    string originalArgs = app.args;
                    if (app.path.Split('/').Last() == "firefox")
                    {
                        int random = Random.Range(10000000, 99999999);
                        app.args += " --new-instance -P " + random;
                        app.args = app.args.Trim();
                    }
                    
                    // serialize app object to json & create 'StartApp' TCP message
                    string json = JsonUtility.ToJson(app);
                    ServerToClientMessage message = new ServerToClientMessage
                    {
                        payload = json,
                        MessageType = MessageTypes.StartApp
                    };
                    app.args = originalArgs;

                    // send
                    if (!TCPHandler.SendMessage(_id, message))
                        Logger.Instance.LogError("StartApps: Client " + _id + " doesn't exist!");
                    else 
                        Logger.Instance.LogSuccess("StartApps: Sent " + json + " to " + _id);
                }
            }
        }

        private void StopApps()
        {
            // create 'StopApps' TCP message
            ServerToClientMessage message = new ServerToClientMessage
            {
                payload = "",
                MessageType = MessageTypes.StopApps
            };
        
            // send
            if (!TCPHandler.SendMessage(_id, message))
                Logger.Instance.LogError("StopApps: Client " + _id + " doesn't exist!");
            else 
                Logger.Instance.LogSuccess("StopApps: Sent " + message.MessageType + " to " + _id);
        }

        public void Init(KeckDisplayData keckDisplayData)
        {
            _id = keckDisplayData.id;
            _ip = keckDisplayData.ip;
            _path = keckDisplayData.path;
            _monitors = new List<Monitor>();

            foreach (MonitorData m in keckDisplayData.monitors)
            {
                GameObject monitorObject = Instantiate(monitorPrefab, transform);
                Monitor monitor = monitorObject.GetComponent<Monitor>();
                monitor.Init(m);
            
                // remember monitor
                _monitors.Add(monitor);
            }

            // update UI elements
            id.text = _id;
        }

        public KeckDisplayData GetSerializable()
        {
            return new KeckDisplayData
            {
                id = _id,
                ip = _ip,
                path = _path,
                monitors = _monitors.Select(monitor => monitor.GetSerializable()).ToArray()
            };
        }
    }
}