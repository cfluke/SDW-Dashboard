using System;

namespace SerializableData
{
    [Serializable]
    public class SDWConfigData
    {
        // top half of the Dashboard
        public DiscoveryWallData discoveryWallData;
        
        // bottom half of the Dashboard
        public WidgetsData widgetsData;
    }
}