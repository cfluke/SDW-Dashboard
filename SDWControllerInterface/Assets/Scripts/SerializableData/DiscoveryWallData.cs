using System;
using JetBrains.Annotations;

namespace SerializableData
{
    [Serializable]
    public class DiscoveryWallData
    {
        public KeckDisplayData[] keckDisplays; // the Discovery Wall is comprised of many KeckDisplays
    }
    
    [Serializable]
    public class KeckDisplayData
    {
        public string id;
        public string ip;
        public string path;
        public MonitorData[] monitors; // each KeckDisplay can have multiple Monitors
    }
    
    [Serializable]
    public class MonitorData
    {
        public int x, y;
        public int w, h;
        public string layout;
        public AppData[] apps; // each Monitor can launch & position multiple Apps
    }
    
    [Serializable]
    public class AppData
    {
        public string path;
        public int x, y;
        public int w, h;
        [CanBeNull] public string name;
        [CanBeNull] public string args;
        [CanBeNull] public string icon;
    }
}