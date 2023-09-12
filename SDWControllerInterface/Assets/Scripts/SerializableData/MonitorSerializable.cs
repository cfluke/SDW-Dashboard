using System;
using System.Collections.Generic;

namespace SerializableData
{
    [Serializable]
    public class MonitorSerializable
    {
        public int x, y;
        public int w, h;
        public string layout;
        public AppSerializable[] apps;

        public MonitorSerializable(int xOffset, int yOffset, int width, int height, string appLayout, AppSerializable[] a)
        {
            x = xOffset;
            y = yOffset;
            w = width;
            h = height;
            layout = appLayout;
            apps = a;
        }
    }
}