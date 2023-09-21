using System;
using System.Collections.Generic;

namespace SerializableData
{
    [Serializable]
    public class KeckDisplaySerializable
    {
        public string id;
        public string ip;
        public string path;
        public MonitorSerializable[] monitors;

        public KeckDisplaySerializable(string clientId, string ipAddress, string listenerPath, List<MonitorSerializable> m)
        {
            id = clientId;
            ip = ipAddress;
            path = listenerPath;
            monitors = m.ToArray();
        }
    }
}