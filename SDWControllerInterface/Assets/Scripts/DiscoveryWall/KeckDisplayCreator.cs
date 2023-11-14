using System;
using System.Collections.Generic;
using AppLayout;
using SerializableData;
using UnityEngine;
using Logger = Logs.Logger;

namespace DiscoveryWall
{
    [RequireComponent(typeof(DiscoveryWall))]
    public class KeckDisplayCreator : MonoBehaviour
    {
        private DiscoveryWall _discoveryWall;
        private int _keckDisplayCount = 1;
        
        private void Start()
        {
            _discoveryWall = GetComponent<DiscoveryWall>();
            
            // start TCPHandler (just in case) and register MessageReceived event listener
            TCPHandler.Instantiate();
            TCPHandler.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, TCPMessageReceivedEventArgs tcpMessageReceivedEventArgs)
        {
            if (tcpMessageReceivedEventArgs.message.MessageType == MessageTypes.Identify)
            {
                Logger.Instance.Log(tcpMessageReceivedEventArgs.message.payload);
                IdentifyMessage identifyMessage = JsonUtility.FromJson<IdentifyMessage>(tcpMessageReceivedEventArgs.message.payload);

                CreateKeckDisplay(identifyMessage);
            }
        }

        private void CreateKeckDisplay(IdentifyMessage identifyMessage)
        {
            // create MonitorData
            List<MonitorData> monitors = new List<MonitorData>();
            foreach (DisplayDetails displayDetails in identifyMessage.displayDetails)
            {
                monitors.Add(new MonitorData
                {
                    x = displayDetails.x,
                    y = displayDetails.y,
                    w = displayDetails.w,
                    h = displayDetails.h,
                    apps = Array.Empty<AppData>(),
                    layout = AppLayouts.One.ToString()
                });
            }

            // create KeckDisplayData and use it to 
            KeckDisplayData keckDisplayData = new KeckDisplayData
            {
                id = identifyMessage.id,
                ip = identifyMessage.ip,
                monitors = monitors.ToArray(),
                path = identifyMessage.path
            };
            
            // create the KeckDisplay (need MainThreadDispatcher to execute on Unity main thread)
            MainThreadDispatcher.Instance.Enqueue(() => { _discoveryWall.AddKeckDisplay(keckDisplayData); });
        }
        
#if UNITY_EDITOR || UNITY_DEBUG
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                DisplayDetails monitor1 = new DisplayDetails
                {
                    w = 3840,
                    h = 2160,
                    x = 0,
                    y = 0
                };
                DisplayDetails monitor2 = new DisplayDetails
                {
                    w = 3840,
                    h = 2160,
                    x = 0,
                    y = 2160
                };

                IdentifyMessage identifyMessage = new IdentifyMessage
                {
                    id = "debug" + _keckDisplayCount++,
                    ip = "localhost",
                    displayDetails = new[]
                    {
                        monitor1,
                        monitor2
                    }
                };
                CreateKeckDisplay(identifyMessage);
            }
        }
#endif
    }
}