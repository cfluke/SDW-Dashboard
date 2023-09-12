using System;
using JetBrains.Annotations;

namespace SerializableData
{
    [Serializable]
    public class AppSerializable
    {
        public string path;
        public int x, y;
        public int w, h;
        
        [CanBeNull] public string name;
        [CanBeNull] public string args;
        [CanBeNull] public string icon;

        public AppSerializable(string filePath, int xVal, int yVal, int width, int height, 
                               string appName, string arguments, string iconPath)
        {
            path = filePath.Replace("\\", "/");
            x = xVal;
            y = yVal;
            w = width;
            h = height;
            
            // optional stuff
            name = appName;
            args = arguments;
            icon = iconPath;
        }
    }
}