using System;
using System.Collections.Generic;

namespace SerializableData
{
    [Serializable]
    public class DiscoveryWallSerializable
    {
        public KeckDisplaySerializable[] keckDisplays;

        public DiscoveryWallSerializable(KeckDisplaySerializable[] k)
        {
            keckDisplays = k;
        }

        public DiscoveryWallSerializable(List<KeckDisplaySerializable> k)
        {
            keckDisplays = k.ToArray();
        }
    }
}