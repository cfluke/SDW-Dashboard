using SerializableData;
using TMPro;
using UnityEngine;

namespace Widgets.Note
{
    public class NoteWidget : Widget
    {
        [SerializeField] private TMP_Text notes;

        public override WidgetData Serialize()
        {
            NoteWidgetData noteData = new NoteWidgetData
            {
                note = notes.text
            };
            return noteData;
        }

        public override void Deserialize(WidgetData widgetData)
        {
            notes.text = ((NoteWidgetData)widgetData).note;
        }
    }
}
