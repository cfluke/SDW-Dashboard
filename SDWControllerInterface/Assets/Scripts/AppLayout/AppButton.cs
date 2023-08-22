using System;
using DialogManagement;
using DiscoveryWall;
using FileExplorer;
using UnityEngine;

namespace AppLayout
{
    public class AppButton : MonoBehaviour
    {
        [Range(0, 1.0f)] [SerializeField] private float x, y;
        [Range(0, 1.0f)] [SerializeField] private float width = 1.0f, height = 1.0f;

        [SerializeField] private AppButtonIcon appButtonIcon;
        
        public async void ImportApp()
        {
            DialogManager dialogManager = FindObjectOfType<DialogManager>();
            
            FileExplorerArgs args = new FileExplorerArgs
            {
                DialogType = FileExplorerDialogType.Open,
                Directory = "/",
                Extension = "*" // show any files
            };
            
            string path = await dialogManager.OpenFileDialog<string, FileExplorerArgs>(args);
            if (path != null)
            {
                Debug.Log(path);
                // show the app on the UI
                appButtonIcon.Show(path);
                
                // add app to monitor component (for later serialization)
                AppLayout appLayout = GetComponentInParent<AppLayout>();
                appLayout.AddToMonitor(path, x, y, width, height);
            }
        }

        public void Clear()
        {
            appButtonIcon.Hide();
        }
    }
}