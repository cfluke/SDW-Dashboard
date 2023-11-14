using System;
using UnityEngine;
using UnityEngine.UI;

namespace Widgets
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class WidgetGrid : MonoBehaviour
    {
        private GridLayoutGroup _gridLayoutGroup;

        private Vector2 CellSize => _gridLayoutGroup.cellSize;
        private Vector2 Spacing => _gridLayoutGroup.spacing;

        private void Start()
        {
            _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        }
        
        
    }
}
