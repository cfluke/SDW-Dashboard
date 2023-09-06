using System;
using System.Collections.Generic;
using AppLayout;
using SerializableData;
using UnityEngine;

namespace DiscoveryWall
{
    [RequireComponent(typeof(DiscoveryWall))]
    public class KeckDisplayCreator : MonoBehaviour
    {
        private DiscoveryWall _discoveryWall;
        private MainThreadDispatcher _mainThreadDispatcher;
        
        private void Start()
        {
            _discoveryWall = GetComponent<DiscoveryWall>();
            _mainThreadDispatcher = GetComponent<MainThreadDispatcher>();
            
            // start TCPHandler (just in case) and register MessageReceived event listener
            TCPHandler.Instantiate();
            TCPHandler.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, TCPMessageReceivedEventArgs tcpMessageReceivedEventArgs)
        {
            if (tcpMessageReceivedEventArgs.message.MessageType == MessageTypes.Identify)
            {
                Debug.Log(tcpMessageReceivedEventArgs.message.payload);
                IdentifyMessage identifyMessage = JsonUtility.FromJson<IdentifyMessage>(tcpMessageReceivedEventArgs.message.payload);

                // get id, ip, and monitors to add to KeckDisplay
                string id = identifyMessage.id;
                string ip = identifyMessage.ip;
                List<MonitorSerializable> monitors = new List<MonitorSerializable>();
                foreach (DisplayDetails displayDetails in identifyMessage.displayDetails)
                {
                    int x = displayDetails.x;
                    int y = displayDetails.y;
                    int w = displayDetails.w;
                    int h = displayDetails.h;
                    string appLayout = AppLayouts.None.ToString();
                    AppSerializable[] apps = Array.Empty<AppSerializable>();
                    monitors.Add(new MonitorSerializable(x, y, w, h, appLayout, apps));
                }
                
                // create KeckDisplay data and use it to create a new KeckDisplay UI element in the SDW
                KeckDisplaySerializable keckDisplayData = new KeckDisplaySerializable(id, ip, monitors);
                _mainThreadDispatcher.Enqueue(() =>
                {
                    _discoveryWall.AddKeckDisplay(keckDisplayData);
                });
            }
        }
    }
}