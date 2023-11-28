using SerializableData;
using UnityEngine;

namespace Widgets
{
    public abstract class Widget : MonoBehaviour
    {
        public string Title => name.Substring(0, name.Length - 7); // -7 for '(Clone)';

        public virtual WidgetData Serialize()
        {
            Vector2Int currentCell = GetComponentInParent<WidgetContainer>().CurrentCell.Coords;
            return new WidgetData
            {
                title = Title,
                x = currentCell.x,
                y = currentCell.y
            };
        }

        public virtual void Deserialize(WidgetData widgetData)
        {
            WidgetContainer widgetContainer = GetComponentInParent<WidgetContainer>();
            widgetContainer.CurrentCell = WidgetManager.Instance.FindCell(widgetData.x, widgetData.y);
        }
    }
}
