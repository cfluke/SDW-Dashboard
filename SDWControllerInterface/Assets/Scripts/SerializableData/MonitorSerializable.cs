using System;
using System.Collections.Generic;

namespace SerializableData
{
    [Serializable]
    public class MonitorSerializable
    {
        public AppSerializable[] apps;

        public MonitorSerializable(AppSerializable[] a)
        {
            apps = a;
        }

        public MonitorSerializable(List<AppSerializable> a)
        {
            apps = a.ToArray();
        }
    }
}