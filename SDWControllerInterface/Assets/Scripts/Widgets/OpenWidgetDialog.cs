using DialogManagement;
using DialogManagement.Widget;
using UnityEngine;

namespace Widgets
{
    public class OpenWidgetDialog : MonoBehaviour
    {
        [SerializeField] private GameObject widgetSelectionDialogPrefab;

        public async void OpenDialog()
        {
            Object[] widgetPrefabs = Resources.LoadAll("WidgetPrefabs", typeof(GameObject));
            WidgetSelectionDialogArgs args = new WidgetSelectionDialogArgs
            {
                WidgetPrefabs = widgetPrefabs
            };
            await DialogManager.Instance.Open<object, WidgetSelectionDialogArgs>(widgetSelectionDialogPrefab, args);
        }
    }
}