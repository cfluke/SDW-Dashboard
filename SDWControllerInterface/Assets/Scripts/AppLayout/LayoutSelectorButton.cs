using UnityEngine;

namespace AppLayout
{
    public class LayoutSelectorButton : MonoBehaviour
    {
        public void OpenLayoutSelector(RectTransform monitor)
        {
            LayoutSelectorDialog layoutSelector = FindObjectOfType<LayoutSelectorDialog>();
            layoutSelector.OpenDialog(monitor);
        }
    }
}