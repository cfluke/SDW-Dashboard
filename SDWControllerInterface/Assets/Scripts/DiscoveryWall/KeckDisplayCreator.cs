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
        private int _keckDisplayCount = 1;
        
        private void Start()
        {
            _discoveryWall = GetComponent<DiscoveryWall>();
            
            // start TCPHandler (just in case) and register MessageReceived event listener
            TCPHandler.Instantiate();
            TCPHandler.MessageReceived += OnMessageReceived;
        }

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

        private void OnMessageReceived(object sender, TCPMessageReceivedEventArgs tcpMessageReceivedEventArgs)
        {
            if (tcpMessageReceivedEventArgs.message.MessageType == MessageTypes.Identify)
            {
                Debug.Log(tcpMessageReceivedEventArgs.message.payload);
                IdentifyMessage identifyMessage = JsonUtility.FromJson<IdentifyMessage>(tcpMessageReceivedEventArgs.message.payload);

                CreateKeckDisplay(identifyMessage);
            }
        }

        private void CreateKeckDisplay(IdentifyMessage identifyMessage)
        {
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
            MainThreadDispatcher.Instance.Enqueue(() => { _discoveryWall.AddKeckDisplay(keckDisplayData); });
        }
    }
}