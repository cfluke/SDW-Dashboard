using AppLayout;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Utility;

namespace DraggableApps
{
    public class WidgetAppButton : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text title;
        
        [SerializeField] private GameObject appDraggablePrefab;
        private AppDraggable _appDraggable;

        private App _app;
        
        public void Init(App app)
        {
            _app = app;
            
            // set UI elements
            if (_app.Icon != null)
                icon.sprite = _app.Icon;
            title.text = string.IsNullOrEmpty(_app.Name) ? app.Path : app.Name;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GameObject draggableAppsParent = GameObject.Find("DraggableApps");
            RectTransform draggableAppsRect = draggableAppsParent.GetComponent<RectTransform>();
            
            // Instantiate the AppDraggable prefab and set it as a child of the Canvas.
            GameObject appDraggableObject = Instantiate(appDraggablePrefab, GetComponent<RectTransform>());
            appDraggableObject.transform.SetParent(draggableAppsRect, true);
            
            // put the app data on the AppDraggable
            _appDraggable = appDraggableObject.GetComponent<AppDraggable>();
            _appDraggable.Init(_app);
        }
    }

}