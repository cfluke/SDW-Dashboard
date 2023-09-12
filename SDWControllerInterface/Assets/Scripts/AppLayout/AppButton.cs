using DialogManagement;
using FileExplorer;
using UnityEngine;

namespace AppLayout
{
    public class AppButton : MonoBehaviour
    {
        [SerializeField] private int buttonId;
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
                // add app to app layout
                AppLayout appLayout = GetComponentInParent<AppLayout>();
                appLayout.AddApp(buttonId, path, x, y, width, height);
            }
        }

        public void ShowAppIcon(string path)
        {
            // show the app on the UI
            appButtonIcon.Show(path);
        }

        public void Clear()
        {
            appButtonIcon.Hide();
        }
    }
}