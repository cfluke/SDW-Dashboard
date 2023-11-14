using SerializableData;
using TMPro;
using UnityEngine;

namespace Widgets.Firefox
{
    public class FirefoxWidget : Widget
    {
        [SerializeField] private TMP_Text url;

        public override WidgetData Serialize()
        {
            FirefoxWidgetData firefoxData = new FirefoxWidgetData
            {
                url = url.text
            };
            return firefoxData;
        }

        public override void Deserialize(WidgetData widgetData)
        {
            url.text = ((FirefoxWidgetData)widgetData).url;
        }
    }
}
