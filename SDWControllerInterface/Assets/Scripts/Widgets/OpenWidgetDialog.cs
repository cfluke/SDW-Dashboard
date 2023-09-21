using DialogManagement;
using DialogManagement.Widget;
using UnityEngine;

public class OpenWidgetDialog : MonoBehaviour
{
    [SerializeField] private GameObject widgetSelectionDialogPrefab;
    private Object[] _widgetPrefabs;

    public async void OpenDialog()
    {
        _widgetPrefabs = Resources.LoadAll("WidgetPrefabs", typeof(GameObject));
        WidgetSelectionDialogArgs args = new WidgetSelectionDialogArgs
        {
            WidgetPrefabs = _widgetPrefabs
        };
        await DialogManager.Instance.Open<object, WidgetSelectionDialogArgs>(widgetSelectionDialogPrefab, args);
    }

}
