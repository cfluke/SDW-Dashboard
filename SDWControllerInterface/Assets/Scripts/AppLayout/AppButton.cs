using DialogManagement;
using DialogManagement.CreateApp;
using DiscoveryWall;
using SerializableData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AppLayout
{
    public class AppButton : MonoBehaviour
    {
        [SerializeField] private int buttonId;
        [Range(0, 1.0f)] [SerializeField] private float x, y;
        [Range(0, 1.0f)] [SerializeField] private float width = 1.0f, height = 1.0f;
        [SerializeField] private Color selectedColour;

        public int ID => buttonId;
        public Vector2 Position => new (x, y);
        public Vector2 Dimensions => new (width, height);
        
        private AppButtonIcon _appButtonIcon;
        public AppButtonIcon AppButtonIcon
        {
            get
            {
                if (_appButtonIcon == null)
                    _appButtonIcon = GetComponentInChildren<AppButtonIcon>(true);
                return _appButtonIcon;
            }
        }

        public async void ImportApp()
        {
            // colour the button so the user remembers which button they're adding the app to
            Image image = GetComponent<Image>();
            Color originalColour = image.color;
            image.color = selectedColour;
            
            AppLayout appLayout = GetComponentInParent<AppLayout>();
            AppCreationArgs args = new AppCreationArgs();

            // check to see if there's already an App in the button, if so add it to args so the AppCreationDialog pre-fills
            if (appLayout.Apps[buttonId] != null)
            {
                args = new AppCreationArgs
                {
                    App = new App(appLayout.Apps[buttonId])
                };
            }
            
            // open the "Create App" dialog for the user
            App app = await DialogManager.Instance.OpenAppCreationDialog<App, AppCreationArgs>(args);
            if (app != null)
                appLayout.AddApp(buttonId, app);
            
            // reset colour back
            image.color = originalColour;
        }

        public void AddApp(App app)
        {
            AppLayout appLayout = GetComponentInParent<AppLayout>();
            appLayout.AddApp(buttonId, app);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Button button = GetComponent<Button>();
            button.OnPointerEnter(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Button button = GetComponent<Button>();
            button.OnPointerExit(eventData);
        }
    }
}