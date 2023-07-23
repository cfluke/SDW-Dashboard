using System;

namespace AppManagement
{
    [Serializable]
    public class App
    {
        public string path;
        public string appName;

        public App(string filePath)
        {
            path = filePath;
            appName = System.IO.Path.GetFileName(path);
        }
    }
}