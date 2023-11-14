using UnityEngine;

namespace Widgets
{
    public class WidgetGridCell : MonoBehaviour
    {
        public Vector2Int Coords => new(_x, _y);
        private int _x;
        private int _y;
        
        void Start()
        {
            string[] cellNameSplit = gameObject.name.Split(',');
            _x = int.Parse(cellNameSplit[0]);
            _y = int.Parse(cellNameSplit[1]);
        }
    }
}
