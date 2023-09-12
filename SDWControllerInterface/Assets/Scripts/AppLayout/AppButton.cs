using System;
using DialogManagement;
using DiscoveryWall;
using SerializableData;
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
            AppLayout appLayout = GetComponentInParent<AppLayout>();
            AppCreationArgs args;

            if (appLayout.Apps[buttonId] != null)
            {
                args = new ExistingAppCreationArgs
                {
                    App = appLayout.Apps[buttonId]
                };
            }
            else
            {
                Monitor monitor = appLayout.Monitor;
                args = new NewAppCreationArgs
                {
                    Position = new Vector2Int((int)(monitor.Offset.x + monitor.Dimensions.x * x), (int)(monitor.Offset.y + monitor.Dimensions.y * y)),
                    Dimensions = new Vector2Int((int)(monitor.Dimensions.x * width), (int)(monitor.Dimensions.y * height)) 
                };
            }
            

            AppSerializable app = await DialogManager.Instance.OpenAppCreationDialog<AppSerializable, AppCreationArgs>(args);
            if (app != null)
                appLayout.AddApp(buttonId, app);
        }

        public void ShowAppIcon(string path, string name)
        {
            // show the app on the UI
            appButtonIcon.Show(path, name);
        }

        public void Clear()
        {
            appButtonIcon.Hide();
        }
    }
}