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
        //Check if snap to grid is enabled or not
        if (!GameObject.Find("SnapToGrid").GetComponent<Button>().enabled)
        {
            //Send raycast down from pointer position and if a gridCell is returned, set the widget position to that cell
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
            //If snap to grid is off, move the widget with the mouse. If the mouse leaves the widget panel, end drag
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
        
    }

    //Update the occupied cells of the widget on end drag
    public void OnEndDrag(PointerEventData eventData)
    {
        if(startCell != null)
        {
            string[] cellCoordinates = startCell.name.Split(",");
            widgetContainer.GetComponent<WidgetContainer>().SetOccupiedCells(new Vector2(float.Parse(cellCoordinates[0]), float.Parse(cellCoordinates[1])));
            startCell = null;
        }
    }

    //Bring the widget to the front of all other widgets when clicked
    public void OnPointerDown(PointerEventData eventData)
    {
        widgetContainer.SetAsLastSibling();
    }

}
