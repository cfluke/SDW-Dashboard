using System.Collections.Generic;
using AppLayout;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Utility;

namespace DraggableApps
{
    [RequireComponent(typeof(Image))]
    public class AppDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text title;

        private App _app;
        private AppButton _prevAppButton;
        private bool _isDragging;

        public void Init(App app)
        {
            _app = app;
            
            // set UI elements
            if (_app.Icon != null)
                icon.sprite = _app.Icon;
            title.text = string.IsNullOrEmpty(_app.Name) ? app.Path : app.Name;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position += (Vector3)eventData.delta;
            
            AppButton appButton = RaycastForAppButton(eventData);

            if (appButton != _prevAppButton)
            {
                if (_prevAppButton != null)
                    _prevAppButton.OnPointerExit(eventData);
                
                _prevAppButton = appButton;
                
                if (_prevAppButton != null)
                    _prevAppButton.OnPointerEnter(eventData);
            }
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            AppButton appButton = RaycastForAppButton(eventData);

            if (appButton != null)
                appButton.AddApp(_app);
            
            Destroy(gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isDragging)
                Destroy(gameObject);
        }

        [CanBeNull]
        private AppButton RaycastForAppButton(PointerEventData eventData)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = eventData.position
            };

            // raycast against UI elements
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);
            foreach (RaycastResult result in results)
            {
                // check if the object we hit has an AppButton component
                AppButton appButton = result.gameObject.GetComponent<AppButton>();
                if (appButton != null)
                {
                    return appButton;
                }
            }

            return null;
        }
    }
}