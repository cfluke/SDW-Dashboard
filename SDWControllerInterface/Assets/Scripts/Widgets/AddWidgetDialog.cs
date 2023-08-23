using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWidgetDialog : MonoBehaviour
{
    
    [SerializeField] private RectTransform dialogPanel;
    [SerializeField] private GameObject widgetSelectorDialogPrefab;
    [SerializeField] private GameObject widgetContainer;
    [SerializeField] private RectTransform widgetPanel;

    [SerializeField] private GameObject widgetSelectionDialog;

    public void OpenDialog()
    {
        //GameObject fileExplorerObject = Instantiate(widgetSelectorDialogPrefab, dialogPanel);
        widgetSelectionDialog.SetActive(true);
    }

    public void OnSelect(GameObject widgetPrefab)
    {

        //Instantiate Widget Container
        
        GameObject container = Instantiate(widgetContainer, widgetPanel);
        container.GetComponent<RectTransform>().sizeDelta = widgetPrefab.GetComponent<RectTransform>().sizeDelta + new Vector2(0, 25);

        // Instantiate selected widget inside panel
        GameObject widget = Instantiate(widgetPrefab, container.transform.GetChild(0));
        widgetSelectionDialog.SetActive(false);


        /*layout.transform.SetAsFirstSibling();
        layoutSelectionDialog.SetActive(false);
        _monitor = null;
        layoutSelectionDialog.SetActive(false);*/
    }
}
