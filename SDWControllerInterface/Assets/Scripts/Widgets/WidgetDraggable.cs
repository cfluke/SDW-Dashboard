using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Widgets
{
    public class WidgetDraggable : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        private WidgetContainer _widgetContainer;

        private void Start()
        {
            _widgetContainer = GetComponentInParent<WidgetContainer>(); // cache WidgetContainer
        }

        public void OnDrag(PointerEventData eventData)
        {
            // perform raycast
            List<RaycastResult> raycastResultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResultList);

            for (int i = 0; i < raycastResultList.Count; i++)
            {
                GameObject hitObject = raycastResultList[i].gameObject;
                
                // check if we hit a WidgetGridCell
                WidgetGridCell widgetGridCell = hitObject.GetComponent<WidgetGridCell>();
                if (widgetGridCell != null)
                {
                    // update WidgetContainer's current cell to the raycast cell
                    _widgetContainer.CurrentCell = widgetGridCell; 
                    return; // no need to check the rest of the raycast results
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Bring the widget to the front of all other widgets when clicked
            _widgetContainer.transform.SetAsLastSibling();
        }
    }
}
