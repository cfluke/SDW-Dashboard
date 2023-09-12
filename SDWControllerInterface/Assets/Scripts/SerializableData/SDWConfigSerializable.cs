using System;
using System.Collections.Generic;

namespace SerializableData
{
    [Serializable]
    public class SDWConfigSerializable
    {
        public DiscoveryWallSerializable discoveryWallData;
        public string notes;

        public SDWConfigSerializable(DiscoveryWallSerializable d, string userNotes)
        {
            discoveryWallData = d;
            notes = userNotes;
        }
    }
}