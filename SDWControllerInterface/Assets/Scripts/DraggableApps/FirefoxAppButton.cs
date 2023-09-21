using System;
using AppLayout;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Utility;

namespace DraggableApps
{
    public class FirefoxAppButton : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private TMP_InputField args;
        
        [SerializeField] private GameObject appDraggablePrefab;
        private AppDraggable _appDraggable;

        public void OnPointerEnter(PointerEventData eventData)
        {
            GameObject draggableAppsParent = GameObject.Find("DraggableApps");
            RectTransform draggableAppsRect = draggableAppsParent.GetComponent<RectTransform>();
            
            // instantiate the AppDraggable prefab and set it as a child of the Canvas.
            GameObject appDraggableObject = Instantiate(appDraggablePrefab, GetComponent<RectTransform>());
            appDraggableObject.transform.SetParent(draggableAppsRect, true);
            
            // put the app data on the AppDraggable
            _appDraggable = appDraggableObject.GetComponent<AppDraggable>();
            Sprite sprite = GetComponent<Image>().sprite;
            App app = new App("/usr/bin/firefox", null, args.text.Trim(), sprite);
            _appDraggable.Init(app);
        }
    }
}