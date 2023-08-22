using System;
using System.Collections.Generic;

namespace SerializableData
{
    [Serializable]
    public class KeckDisplaySerializable
    {
        public MonitorSerializable[] monitors;

        public KeckDisplaySerializable(MonitorSerializable[] m)
        {
            monitors = m;
        }

        public KeckDisplaySerializable(List<MonitorSerializable> m)
        {
            monitors = m.ToArray();
        }
    }
}