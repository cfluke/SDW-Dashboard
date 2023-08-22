using System;

namespace SerializableData
{
    [Serializable]
    public class AppSerializable
    {
        public string path;
        public int x, y;
        public int w, h;

        public AppSerializable(string filePath, int xVal, int yVal, int width, int height)
        {
            path = filePath;
            x = xVal;
            y = yVal;
            w = width;
            h = height;
        }
    }
}