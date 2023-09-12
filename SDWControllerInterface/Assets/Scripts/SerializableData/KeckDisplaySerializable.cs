using System;
using System.Collections.Generic;

namespace SerializableData
{
    [Serializable]
    public class KeckDisplaySerializable
    {
        public MonitorSerializable[] monitors;
        public string id;
        public string ip; // TODO: this might need to change to permanent DNS address or MAC address or something

        public KeckDisplaySerializable(string clientId, string ipAddress, List<MonitorSerializable> m)
        {
            monitors = m.ToArray();
            id = clientId;
            ip = ipAddress;
        }
    }
}