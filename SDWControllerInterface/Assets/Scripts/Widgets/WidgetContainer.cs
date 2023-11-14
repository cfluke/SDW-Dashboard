using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Widgets
{
    public class WidgetContainer : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        
        private List<Vector2Int> _occupiedCells = new(); 
        
        // the current cell the widget is associated with (top-left-most cell)
        private WidgetGridCell _currentCell; 
        public WidgetGridCell CurrentCell
        {
            get => _currentCell;
            set
            {
                _currentCell = value;
                transform.position = _currentCell.transform.position;
                SetOccupiedCells(_currentCell.Coords);
            }
        }

        private Widget _widget;
        public Widget Widget
        {
            get => _widget;
            set
            {
                _widget = value;
                title.text = _widget.Title;
                Resize(_widget.GetComponent<RectTransform>());
            }
        }

        public int Width => ((int)GetComponent<RectTransform>().rect.width + 10) / 110;
        public int Height => ((int)GetComponent<RectTransform>().rect.height + 35) / 110;

        // Closes the widget
        public void Close(GameObject widget)
        {
            Destroy(widget);
        }

        public void Minimise(GameObject miniTabPrefab)
        {
            gameObject.SetActive(false);
            GameObject tab = Instantiate(miniTabPrefab, GameObject.Find("Taskbar").transform);
            tab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text;
            tab.GetComponent<UnMinimise>().SetWidget(gameObject);
        }

        private void Resize(RectTransform targetSize)
        {
            GetComponent<RectTransform>().sizeDelta = targetSize.sizeDelta + new Vector2(0, 25);
        }

        // Sets/Updates the occupied cells list
        private void SetOccupiedCells(Vector2Int startCell)
        {
            _occupiedCells.Clear();

            int w = Width;
            int h = Height;
            for (int i = 0; i < w; i++)
            {
                for (int k = 0; k < h; k++)
                {
                    _occupiedCells.Add(startCell + new Vector2Int(i, k));
                }
            }
        }

        public List<Vector2Int> GetOccupiedCells()
        {
            return _occupiedCells;
        }
    }
}
