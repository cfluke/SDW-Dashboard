using TMPro;
using UnityEngine;

namespace Widgets
{
    public class AddWidgetButton : MonoBehaviour
    {
        public void AddWidget(TMP_Text title)
        {
            WidgetManager.Instance.AddNew(title.text);
        }
    }
}
