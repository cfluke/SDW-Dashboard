using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WidgetContainer : MonoBehaviour
{
    [SerializeField] private GameObject widget;
    [SerializeField] private List<Vector2> occupiedCells; //List of the cells being occupied by the widget
    private int xCells; //X dimension of the widget in terms of the number of cells
    private int yCells; //Y dimension of the widget in terms of the number of cells

    //Closes the widget
    public void Close(GameObject widget)
    {
        Destroy(widget);
    }

    public void Minimise(GameObject miniTabPrefab)
    {
        widget.SetActive(false);
        GameObject tab = Instantiate(miniTabPrefab, GameObject.Find("Taskbar").transform);
        tab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = widget.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        tab.GetComponent<UnMinimise>().SetWidget(widget);
    }


    //Sets/Updates the occupied cells list
    public void SetOccupiedCells(Vector2 startCell)
    {
        occupiedCells.Clear();

        for (int i = 0; i < xCells; i++)
        {
            for (int k = 0; k < yCells; k++)
            {
                occupiedCells.Add(startCell + new Vector2(i, k));
            }
        }
    }

    //Sets the xCells and yCells value based on the dimensions of the widget
    public void SetDimensions()
    {
        xCells = ((int)widget.GetComponent<RectTransform>().rect.width + 10) / 110;
        yCells = ((int)widget.GetComponent<RectTransform>().rect.height + 35) / 110;
    }

    //Getters for the fields

    public int GetXCells()
    {
        return xCells;
    }

    public int GetYCells()
    {
        return yCells;
    }
    public List<Vector2> GetOccupiedCells()
    {
        return occupiedCells;
    }
}
