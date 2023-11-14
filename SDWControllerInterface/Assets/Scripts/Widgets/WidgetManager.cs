using System.Collections.Generic;
using UnityEngine;
using Logger = Logs.Logger;

namespace Widgets
{
    public class WidgetManager : MonoBehaviour
    {
        [SerializeField] private GameObject widgetContainerObject;

        #region singleton
        
        private static WidgetManager _instance;
        public static WidgetManager Instance
        {
            get
            {
                if (_instance == null)
                    Logger.Instance.LogError("WidgetManager does not exist - it must.");

                return _instance;
            }
        }
        
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
            {
                Logger.Instance.LogWarning("Duplicate WidgetManager found in the scene! Deleting the duplicate.");
                Destroy(gameObject);
            }
        }

        #endregion
        
        public void AddNew(string widgetTitle)
        {
            string widgetPath = "WidgetPrefabs/" + widgetTitle;
            
            // Instantiate widget container
            GameObject container = Instantiate(widgetContainerObject, GameObject.Find("Widgets").transform);
            WidgetContainer widgetContainer = container.GetComponent<WidgetContainer>();
            
            // Instantiate widget inside the container
            GameObject widget = Instantiate((GameObject)Resources.Load(widgetPath), container.transform.GetChild(0));
            widgetContainer.Widget = widget.GetComponent<Widget>();

            // Find empty cells for the widget and move the widget to those cells
            WidgetGridCell cell = FindFirstEmptyCell(widgetContainer.Width, widgetContainer.Height);
            if (cell != null)
                widgetContainer.CurrentCell = cell;

            Logger.Instance.Log("Created " + widgetTitle + " widget");
        }

        public void AddExisting(Widget widget)
        {
            string title = widget.Title;
        }

        #region helper function/s

        // Find empty cells and return the start cell
        private WidgetGridCell FindFirstEmptyCell(int width, int height)
        {
            // Get all occupied cells and put them in a list
            Transform widgetContainerParent = GameObject.Find("Widgets").transform;
            List<Vector2Int> allOccupiedCells = new List<Vector2Int>();
            for(int i = 0; i < widgetContainerParent.childCount - 1; i++)
                allOccupiedCells.AddRange(widgetContainerParent.GetChild(i).GetComponent<WidgetContainer>().GetOccupiedCells());

            // Check each cell and if unoccupied, check all cells in the widgets dimensions. If all unoccupied, return first cell
            for (int i = 0; i < 17; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    if (!allOccupiedCells.Contains(new Vector2Int(i, k)))
                    {
                        bool isClear = true;
                        for (int x = i; x < i + width; x++)
                        {
                            for (int y = k; y < k + height; y++)
                            {
                                if (allOccupiedCells.Contains(new Vector2Int(x, y)))
                                    isClear = false;
                            }
                        }
                        if (isClear)
                            return GameObject.Find(i + "," + k).GetComponent<WidgetGridCell>();
                    }
                   
                }
            }

            return null;
        }
        
        #endregion
    }
}
