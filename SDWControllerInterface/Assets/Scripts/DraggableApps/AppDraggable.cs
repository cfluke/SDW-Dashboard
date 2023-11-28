using System;
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
    public class AppDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text title;

        private App _app;
        private IAppDraggableTarget _prevTarget;
        private float _originalSize = 1.0f;

        public event Action<App> OnDragBegin;
        public event Action<App> OnDragEnd;

        public void Init(App app)
        {
            _app = app;
            
            // set UI elements
            if (_app.Icon != null)
                icon.sprite = _app.Icon;
            title.text = string.IsNullOrEmpty(_app.Name) ? app.Path : app.Name;
        }

        public void SetColor(Color color)
        {
            icon.color = color;
            title.color = new Color(title.color.r, title.color.g, title.color.b, color.a);
        }

        public void SetSize(float size)
        {
            transform.localScale = new Vector3(size, size, size);
            _originalSize = size;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            GameObject draggableAppsParent = GameObject.Find("DraggableApps");
            RectTransform draggableAppsRect = draggableAppsParent.GetComponent<RectTransform>();
            transform.SetParent(draggableAppsRect, true);
            
            OnDragBegin?.Invoke(_app);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position += (Vector3)eventData.delta;
            
            IAppDraggableTarget target = RaycastForTarget(eventData);
            if (target != _prevTarget)
            {
                _prevTarget?.OnAppExit(eventData);
                _prevTarget = target;
                _prevTarget?.OnAppEnter(eventData);
            }
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            OnDragEnd?.Invoke(_app);
            
            _prevTarget?.OnAppDrop(_app);
            
            Destroy(gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = new Vector3(_originalSize, _originalSize, _originalSize) * 1.2f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = new Vector3(_originalSize, _originalSize, _originalSize);
        }

        [CanBeNull]
        private IAppDraggableTarget RaycastForTarget(PointerEventData eventData)
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
                IAppDraggableTarget target = result.gameObject.GetComponent<IAppDraggableTarget>();
                if (target != null)
                    return target;
            }

            return null;
        }
    }
}