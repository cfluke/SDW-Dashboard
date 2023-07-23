using UnityEngine;
using SFB;

namespace AppManagement
{
    public class AppImporter : MonoBehaviour
    {
        private AppBar _appBar;

        void Start()
        {
            _appBar = FindObjectOfType<AppBar>();
        }

        public void ImportApp()
        {
            // use the StandaloneFileBrowser tool to open a native file browser
            string[] filePath = StandaloneFileBrowser.OpenFilePanel("Import Application", "", "", false);
            
            // if user selected something, add it to app bar
            if (filePath.Length > 0)
                _appBar.AddApp(new App(filePath[0]));
        }
    }
}