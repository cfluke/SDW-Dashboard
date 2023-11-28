using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SerializableData;
using UnityEngine;
using Logger = Logs.Logger;

namespace Widgets
{
    public class WidgetManager : MonoBehaviour
    {
        [SerializeField] private GameObject widgetContainerObject;
        [SerializeField] private Transform widgetsContent;

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
        
        public WidgetContainer Add(string widgetTitle)
        {
            string widgetPath = "WidgetPrefabs/" + widgetTitle;
            
            // Instantiate widget container
            GameObject container = Instantiate(widgetContainerObject, widgetsContent);
            WidgetContainer widgetContainer = container.GetComponent<WidgetContainer>();
            
            // Instantiate widget inside the container
            GameObject widget = Instantiate((GameObject)Resources.Load(widgetPath), container.transform.GetChild(0));
            widgetContainer.Widget = widget.GetComponent<Widget>();

            // Find empty cells for the widget and move the widget to those cells
            WidgetGridCell cell = FindFirstEmptyCell(widgetContainer.Width, widgetContainer.Height);
            if (cell != null)
                widgetContainer.CurrentCell = cell;

            Logger.Instance.Log("Created " + widgetTitle + " widget");
            return widgetContainer;
        }

        public void Populate(WidgetsData widgetsData)
        {
            Clear(); // clear old widgets
            foreach (WidgetData widgetData in widgetsData.widgetData)
            {
                WidgetContainer widgetContainer = Add(widgetData.title); // create widget
                widgetContainer.Widget.Deserialize(widgetData); // populate widget with persistent data
            }
        }

        public WidgetsData Serialize()
        {
            WidgetContainer[] widgetContainers = widgetsContent.GetComponentsInChildren<WidgetContainer>();
            return new WidgetsData
            {
                widgetData = widgetContainers.Select(container => container.Widget.Serialize()).ToArray() // serialize all widgets
            };
        }

        public void Clear()
        {
            WidgetContainer[] widgetContainers = widgetsContent.GetComponentsInChildren<WidgetContainer>();
            foreach (WidgetContainer container in widgetContainers)
                Destroy(container.gameObject); // destroy widget/s
        }

        #region helper function/s

        // Find empty cells and return the start cell
        private WidgetGridCell FindFirstEmptyCell(int width, int height)
        {
            // Get all occupied cells and put them in a list
            List<Vector2Int> allOccupiedCells = new List<Vector2Int>();
            for(int i = 0; i < widgetsContent.childCount - 1; i++)
                allOccupiedCells.AddRange(widgetsContent.GetChild(i).GetComponent<WidgetContainer>().GetOccupiedCells());

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
                            return FindCell(i, k);
                    }
                }
            }

            return null;
        }

        public WidgetGridCell FindCell(int x, int y)
        {
            return GameObject.Find(x + "," + y).GetComponent<WidgetGridCell>();
        }
        
        #endregion
    }
}
