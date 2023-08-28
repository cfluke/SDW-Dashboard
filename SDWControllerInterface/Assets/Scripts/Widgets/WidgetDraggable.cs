using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WidgetDraggable : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform widgetContainer;
    public void OnDrag(PointerEventData eventData)
    {
        float yPos = widgetContainer.anchoredPosition.y; //yPos value of the widget container
        //Since the yPos is measured from the center of widget container halfHeigh helps to get y value of top and bottom of widget container
        float halfHeight = widgetContainer.rect.height / 2; 

        //If widget container y position is not n widgetpanel area then reposition, else allow dragging
        if (yPos > -halfHeight)
            widgetContainer.Translate(new Vector2(0, 2 * (-halfHeight - yPos) - 1));
        else if (yPos < -475 - halfHeight)
            widgetContainer.Translate(new Vector2(0, 2 * (-475 - halfHeight - yPos))); 
        else
            widgetContainer.position += (Vector3)eventData.delta;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        widgetContainer.SetAsLastSibling();
    }

}
