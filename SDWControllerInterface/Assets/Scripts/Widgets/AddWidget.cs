using System.Collections;
using System.Collections.Generic;
using DialogManagement.Widget;
using UnityEngine;
using TMPro;

public class AddWidget : MonoBehaviour
{
    [SerializeField] private GameObject widgetContainer;
    public void Add(GameObject txtWidgetName)
    {
        //Get Widget name and widget prefab filepath
        string widgetName = txtWidgetName.GetComponent<TextMeshProUGUI>().text;
        string widgetPath = "WidgetPrefabs/" + widgetName;
        //Instantiate widget container and Instantiate widget inside the container
        GameObject container = Instantiate(widgetContainer, GameObject.Find("Widgets").transform);
        GameObject widget = Instantiate((GameObject)Resources.Load(widgetPath), container.transform.GetChild(0));
     

        //Resize widget container to fit the widget
        container.GetComponent<RectTransform>().sizeDelta = widget.GetComponent<RectTransform>().sizeDelta + new Vector2(0, 25);
        container.GetComponent<WidgetContainer>().SetDimensions();
        //Set container title to widget name
        container.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = widgetName;

        //Find empty cells for the widget and move the widget to those cells
        GameObject cell = FindEmptyCells(container.GetComponent<WidgetContainer>().GetXCells(), container.GetComponent<WidgetContainer>().GetYCells());
        if(cell != null)
        {
            container.transform.position = cell.transform.position;
            string[] cellCoordinates = cell.name.Split(",");
            container.GetComponent<WidgetContainer>().SetOccupiedCells(new Vector2(float.Parse(cellCoordinates[0]), float.Parse(cellCoordinates[1])));
        }

        //Destroy widget selection menu
        WidgetSelectionDialog widgetSelectionDialog = GetComponentInParent<WidgetSelectionDialog>();
        widgetSelectionDialog.Confirm();
    }

    //Find empty cells and return the start cell
    public GameObject FindEmptyCells(int xCells, int yCells)
    {
        //Get all occupied cells and put them in a list
        Transform widgetContainerParent = GameObject.Find("Widgets").transform;
        List<Vector2> allOccupiedCells = new List<Vector2>();
        for(int i = 0; i < widgetContainerParent.childCount - 1; i++)
        {
            allOccupiedCells.AddRange(widgetContainerParent.GetChild(i).GetComponent<WidgetContainer>().GetOccupiedCells());
        }

        //Check each cell and if unoccupied, check all cells in the widgets dimensions. If all unoccupied, return first cell
        for (int i = 0; i < 17; i++)
        {
            for (int k = 0; k < 4; k++)
            {
                if (!allOccupiedCells.Contains(new Vector2(i, k)))
                {
                    bool isClear = true;
                    for (int x = i; x < i + xCells; x++)
                    {
                        for (int y = k; y < k + yCells; y++)
                        {
                            if (allOccupiedCells.Contains(new Vector2(x, y)))
                                isClear = false;
                        }
                    }
                    if (isClear)
                        return GameObject.Find(i.ToString() + "," + k.ToString());
                }
                   
            }
        }

        return null;
    }

}
