using DialogManagement;
using DialogManagement.CreateApp;
using DraggableApps;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AppLayout
{
    public class AppButton : MonoBehaviour, IAppDraggableTarget
    {
        [SerializeField] private int buttonId;
        [Range(0, 1.0f)] [SerializeField] private float x, y;
        [Range(0, 1.0f)] [SerializeField] private float width = 1.0f, height = 1.0f;
        [SerializeField] private Color selectedColour;

        [SerializeField] private Transform appDraggableAnchor;

        public int ID => buttonId;
        public Vector2 Position => new (x, y);
        public Vector2 Dimensions => new (width, height);

        public void AddApp(App app)
        {
            foreach (Transform child in appDraggableAnchor.transform)
                Destroy(child.gameObject); // delete children
            
            // let AppLayout know about the new app (for later serialization)
            AppLayout appLayout = GetComponentInParent<AppLayout>();
            appLayout.AddApp(buttonId, app);

            // create an AppDraggable on the middle of the button
            AppDraggableFactory.Instance.CreateAppDraggable(appDraggableAnchor, app, OnAppDragBegin, OnAppDragEnd, 1.2f);
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
                AddApp(app);
            
            // reset colour back
            image.color = originalColour;
        }

        public void OnAppDrop(App app)
        {
            AddApp(app);
        }

        public void OnAppEnter(PointerEventData eventData)
        {
            Button button = GetComponent<Button>();
            button.OnPointerEnter(eventData);
        }

        public void OnAppExit(PointerEventData eventData)
        {
            Button button = GetComponent<Button>();
            button.OnPointerExit(eventData);
        }
        
        private void OnAppDragBegin(App app)
        {
            AppLayout appLayout = GetComponentInParent<AppLayout>();
            appLayout.RemoveApp(buttonId);

            // create ghost
            Color transparent = new Color(1, 1, 1, 0.5f);
            AppDraggableFactory.Instance.CreateGhostApp(appDraggableAnchor, app, transparent, 1.2f);
            
            // hide '+' sign
            GetComponentInChildren<TMP_Text>().enabled = false;
        }
        
        private void OnAppDragEnd(App app)
        {
            // destroy ghost
            Destroy(appDraggableAnchor.GetChild(0).gameObject); 
            
            // show '+' sign
            GetComponentInChildren<TMP_Text>().enabled = true;
        }
    }
}