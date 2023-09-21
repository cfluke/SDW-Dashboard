using System.Collections;
using System.Collections.Generic;
using AppLayout;
using DialogManagement;
using DialogManagement.CreateApp;
using DraggableApps;
using SerializableData;
using UnityEngine;

namespace Widgets
{
    public class AppsWidget : MonoBehaviour
    {
        [SerializeField] private GameObject widgetAppButtonPrefab;
        [SerializeField] private RectTransform content;
        
        public async void AddApp()
        {
            // open the "Create App" dialog for the user
            App app = await DialogManager.Instance.OpenAppCreationDialog<App, AppCreationArgs>(new AppCreationArgs());
            if (app != null)
            {
                GameObject widgetAppButtonObject = Instantiate(widgetAppButtonPrefab, content);
                WidgetAppButton widgetAppButton = widgetAppButtonObject.GetComponent<WidgetAppButton>();
                widgetAppButton.Init(app);
            }
        }
    }
}