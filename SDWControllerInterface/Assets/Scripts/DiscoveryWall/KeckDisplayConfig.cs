using System;
using AppManagement;

namespace DiscoveryWall
{
    [Serializable]
    public class KeckDisplayConfig
    {
        // TODO: could potentially turn these into App arrays and execute multiple applications per monitor
        public App monitor1;
        public App monitor2;

        public KeckDisplayConfig(App app1, App app2)
        {
            monitor1 = app1;
            monitor2 = app2;
        }
    }
}