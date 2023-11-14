using SerializableData;
using UnityEngine;

namespace Widgets
{
    public class Widget : MonoBehaviour
    {
        public string Title => name.Substring(0, name.Length - 7); // -7 for '(Clone)';
        protected int x, y;
        
        public virtual WidgetData Serialize()
        {
            return new WidgetData
            {
                title = Title,
                x = x,
                y = y
            };
        }
        
        public virtual void Deserialize(WidgetData widgetData)
        {
        }
    }
}
