using System;

namespace SerializableData
{
    [Serializable]
    public class SDWConfigSerializable
    {
        // top half of the Dashboard
        public DiscoveryWallData discoveryWallData;
        
        // bottom half of the Dashboard
        public WidgetsData widgetData;
    }
}