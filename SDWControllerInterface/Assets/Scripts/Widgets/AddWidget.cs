using System.Collections;
using System.Collections.Generic;
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
        container.GetComponent<WidgetContainer>().SetCellsCount();
        //Set container title to widget name
        container.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = widgetName;

        GameObject cell = FindStartCell(container.GetComponent<WidgetContainer>().GetXCells(), container.GetComponent<WidgetContainer>().GetYCells());
        if(cell != null)
        {
            container.transform.position = cell.transform.position;
            string[] cellCoordinates = cell.name.Split(",");
            container.GetComponent<WidgetContainer>().SetOccupiedCells(new Vector2(float.Parse(cellCoordinates[0]), float.Parse(cellCoordinates[1])));
        }

        //Destroy widget selection menu
        Destroy(GameObject.Find("WidgetSelection(Clone)"));
    }

    public GameObject FindStartCell(int xCells, int yCells)
    {
        Transform widgetContainerParent = GameObject.Find("Widgets").transform;
        List<Vector2> allOccupiedCells = new List<Vector2>();
        for(int i = 0; i < widgetContainerParent.childCount - 1; i++)
        {
            allOccupiedCells.AddRange(widgetContainerParent.GetChild(i).GetComponent<WidgetContainer>().GetOccupiedCells());
        }

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
