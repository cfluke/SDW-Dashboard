using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetContainer : MonoBehaviour
{
    [SerializeField] private GameObject widget;
    [SerializeField] private List<Vector2> occupiedCells;
    private int xCells;
    private int yCells;

    public void Close(GameObject container)
    {
        Destroy(container);
    }

    public void SetOccupiedCells(Vector2 startCell)
    {
        occupiedCells.Clear();
        
        for(int i = 0; i < xCells; i++)
        {
            for (int k = 0; k < yCells; k++)
            {
                occupiedCells.Add(startCell + new Vector2(i,k));
            }
        } 
    }

    public void SetCellsCount()
    {
        xCells = ((int)widget.GetComponent<RectTransform>().rect.width + 10) / 110;
        yCells = ((int)widget.GetComponent<RectTransform>().rect.height + 35) / 110;
    }

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
