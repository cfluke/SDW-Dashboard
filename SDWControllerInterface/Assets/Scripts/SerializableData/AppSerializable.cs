using System;

namespace SerializableData
{
    [Serializable]
    public class AppSerializable
    {
        public string path;
        public int x, y;
        public int w, h;
        public string args;

        public AppSerializable(string filePath, int xVal, int yVal, int width, int height, string arguments = "")
        {
            path = filePath.Replace("\\", "/");
            x = xVal;
            y = yVal;
            w = width;
            h = height;
            args = arguments;
        }
    }
}