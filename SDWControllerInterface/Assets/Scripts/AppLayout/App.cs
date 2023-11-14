using JetBrains.Annotations;
using SerializableData;
using UnityEngine;
using Utility;

namespace AppLayout
{
    public class App
    {
        public string Path { get; }
        [CanBeNull] public string Name { get; }
        [CanBeNull] public string Args { get; }
        [CanBeNull] public Sprite Icon { get; private set; }

        private string _iconPath;
        public string IconPath
        {
            get => _iconPath;
            set
            {
                _iconPath = value;
                Icon = ImageLoader.Instance.LoadImageAsSprite(_iconPath);
            }
        }

        public App(string filePath, string appName = null, string arguments = null, string iconPath = null)
        {
            Path = filePath.Replace("\\", "/");
            
            // optional stuff
            Name = appName;
            Args = arguments;
            IconPath = iconPath;
        }
        
        public App(string filePath, string appName = null, string arguments = null, Sprite icon = null)
        {
            Path = filePath.Replace("\\", "/");
            
            // optional stuff
            Name = appName;
            Args = arguments;
            Icon = icon;
        }

        public App(AppData app)
        {
            Path = app.path;
            Name = app.name;
            Args = app.args;
            IconPath = app.icon;
        }
    }
}