using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WidgetDraggable : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler
{
    [SerializeField] private RectTransform widgetContainer;
    [SerializeField] private GridLayout grid;
    private GameObject startCell;
    public void OnDrag(PointerEventData eventData)
    {
        if (!GameObject.Find("SnapToGrid").GetComponent<Button>().enabled)
        {
            List<RaycastResult> raycastResultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResultList);

            for (int i = 0; i < raycastResultList.Count; i++)
            {
                if (raycastResultList[i].gameObject.tag == "gridCell")
                {
                    widgetContainer.position = raycastResultList[i].gameObject.transform.position;
                    startCell = raycastResultList[i].gameObject;
                    return;
                }
            }
        }
        else
        {
            float yPos = widgetContainer.anchoredPosition.y;
            if (yPos > 0)
            {
                widgetContainer.Translate(new Vector2(0, -30));
                eventData.pointerDrag = null;
            }
            else if (yPos < -475)
                widgetContainer.Translate(new Vector2(0, 5));
            else
                widgetContainer.position += (Vector3)eventData.delta;
        }
        
        
            //widgetContainer.position += (Vector3)eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(startCell != null)
        {
            string[] cellCoordinates = startCell.name.Split(",");
            widgetContainer.GetComponent<WidgetContainer>().SetOccupiedCells(new Vector2(float.Parse(cellCoordinates[0]), float.Parse(cellCoordinates[1])));
            startCell = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        widgetContainer.SetAsLastSibling();
    }

}
