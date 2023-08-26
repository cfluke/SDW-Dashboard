using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddWidget : MonoBehaviour
{
    [SerializeField] private GameObject widgetContainer;
    public void Add(GameObject txtWidgetName)
    {
        //Get Widget name and widget prefab filepath
        string widgetName = txtWidgetName.GetComponent<TextMeshProUGUI>().text;
        string widgetPath = "WidgetPrefabs/" + widgetName;

        //Instantiate widget container and Instantiate widget inside the container
        GameObject container = Instantiate(widgetContainer, GameObject.Find("WidgetPanel").transform);
        GameObject widget = Instantiate((GameObject)Resources.Load(widgetPath), container.transform.GetChild(0));
     
        //Resize widget container to fit the widget
        container.GetComponent<RectTransform>().sizeDelta = widget.GetComponent<RectTransform>().sizeDelta + new Vector2(0, 25);

        //Set container title to widget name
        container.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = widgetName;

        //Destroy widget selection menu
        Destroy(GameObject.Find("WidgetSelection(Clone)"));
    }

}
