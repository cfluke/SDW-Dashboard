using System;
using AppLayout;
using DraggableApps;
using SerializableData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Widgets.Firefox
{
    public class FirefoxWidget : Widget
    {
        [SerializeField] private TMP_InputField url;
        [SerializeField] private RectTransform firefoxAnchor;
        [SerializeField] private Sprite firefoxIcon;

        private void Start()
        {
            UpdateFirefoxDraggable("");
            url.onValueChanged.AddListener(UpdateFirefoxDraggable);
        }

        private void UpdateFirefoxDraggable(string val)
        {
            foreach (Transform child in firefoxAnchor.transform)
                Destroy(child.gameObject); // delete children

            App app = new App("/usr/bin/firefox", "Firefox", val.Trim(), firefoxIcon);
            AppDraggableFactory.Instance.CreateAppDraggable(firefoxAnchor, app, OnAppDragBegin, OnAppDragEnd);
        }

        private void OnAppDragBegin(App app)
        {
            Color transparent = new Color(1, 1, 1, 0.5f);
            AppDraggableFactory.Instance.CreateGhostApp(firefoxAnchor, app, transparent);
        }

        private void OnAppDragEnd(App app)
        {
            Destroy(firefoxAnchor.GetChild(0).gameObject); // destroy ghost object
            AppDraggableFactory.Instance.CreateAppDraggable(firefoxAnchor, app, OnAppDragBegin, OnAppDragEnd);
        }

        public override WidgetData Serialize()
        {
            return new FirefoxWidgetData(base.Serialize())
            {
                url = url.text
            };
        }

        public override void Deserialize(WidgetData widgetData)
        {
            base.Deserialize(widgetData);
            url.text = ((FirefoxWidgetData)widgetData).url;
        }
    }
}