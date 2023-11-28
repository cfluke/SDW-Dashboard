using System.Collections.Generic;
using AppLayout;
using DialogManagement;
using DialogManagement.CreateApp;
using DraggableApps;
using SerializableData;
using UnityEngine;

namespace Widgets.Apps
{
    public class AppsWidget : Widget
    {
        [SerializeField] private RectTransform content;

        private List<App> _apps = new();
        
        public async void AddApp()
        {
            App app = await DialogManager.Instance.OpenAppCreationDialog<App, AppCreationArgs>(new AppCreationArgs());
            if (app == null) 
                return;
            _apps.Add(app);
            
            MainThreadDispatcher.Instance.Enqueue(() =>
            {
                AppDraggableFactory.Instance.CreateAppDraggable(content, app, OnAppDragBegin, OnAppDragEnd);
            });
        }

        private void OnAppDragBegin(App app)
        {
            int appSiblingIndex = _apps.IndexOf(app); // get hierarchy index

            Color transparent = new Color(1, 1, 1, 0.5f);
            GameObject ghostAppDraggableObject = AppDraggableFactory.Instance.CreateGhostApp(content, app, transparent);
            ghostAppDraggableObject.transform.SetSiblingIndex(appSiblingIndex);
        }

        private void OnAppDragEnd(App app)
        {
            int appSiblingIndex = _apps.IndexOf(app); // get hierarchy index
            
            Destroy(content.GetChild(appSiblingIndex).gameObject); // destroy ghost object
            GameObject appDraggableObject = AppDraggableFactory.Instance.CreateAppDraggable(content, app, OnAppDragBegin, OnAppDragEnd);
            appDraggableObject.transform.SetSiblingIndex(appSiblingIndex);
        }

        public override WidgetData Serialize()
        {
            return new AppsWidgetData(base.Serialize())
            {
                
            };
        }

        public override void Deserialize(WidgetData widgetData)
        {
            base.Deserialize(widgetData);
        }
    }
}