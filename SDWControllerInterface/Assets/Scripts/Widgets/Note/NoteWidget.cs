using SerializableData;
using TMPro;
using UnityEngine;

namespace Widgets.Note
{
    public class NoteWidget : Widget
    {
        [SerializeField] private TMP_InputField notes;

        public override WidgetData Serialize()
        {
            return new NoteWidgetData(base.Serialize())
            {
                note = notes.text
            };
        }

        public override void Deserialize(WidgetData widgetData)
        {
            base.Deserialize(widgetData);
            notes.text = ((NoteWidgetData)widgetData).note;
        }
    }
}
