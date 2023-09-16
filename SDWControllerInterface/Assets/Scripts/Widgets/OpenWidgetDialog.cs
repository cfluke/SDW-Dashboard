using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class OpenWidgetDialog : MonoBehaviour
{
    
    [SerializeField] private RectTransform dialogPanel;
    [SerializeField] private GameObject widgetSelectionDialogPrefab;
    [SerializeField] private GameObject widgetDetails;

    public void OpenDialog()
    {
        //Instantiate widget selection dialog 
        Instantiate(widgetSelectionDialogPrefab, dialogPanel);
        
        //Get all widget prefabs
        Object[] widgetPrefabs = Resources.LoadAll("WidgetPrefabs", typeof(GameObject));

        int i = 0;
        Transform scrollViewContent = GameObject.Find("WidgetScrollContainer").transform;

        
        foreach (var widget in widgetPrefabs)
        {
            //Instantiate a widget details UI section and translate it down
            GameObject wd = Instantiate(widgetDetails, scrollViewContent);
            //wd.transform.Translate(new Vector2(0, -1 * i * 440));

            //Set name textbox to the widget name
            GameObject txtName = wd.transform.GetChild(0).gameObject;
            txtName.GetComponent<TextMeshProUGUI>().text = widget.name;

            //Set the thumbnail image
            //GameObject txtThumb = wd.transform.GetChild(1).gameObject;
            //Texture2D thumbnail = AssetPreview.GetMiniThumbnail(widget);
            //txtThumb.GetComponent<Image>().sprite = Sprite.Create(thumbnail, new Rect(0, 0, thumbnail.width, thumbnail.height), new Vector2(1f, 1f));

            //Make scroll view longet vertically to fit the widget
            scrollViewContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, wd.GetComponent<RectTransform>().sizeDelta.y + 20);
            

            i++;
        }
    }

}
