using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace DialogManagement.Widget
{
    public class WidgetSelectionDialogArgs
    {
        public Object[] WidgetPrefabs { get; set; }
    }
    
    public class WidgetSelectionDialog : Dialog<object, WidgetSelectionDialogArgs>
    {
        [SerializeField] private GameObject widgetPrefab;
        
        public override void Init(WidgetSelectionDialogArgs parameters)
        {
            //Get all widget prefabs
            Transform scrollViewContent = GameObject.Find("WidgetScrollContainer").transform;

            foreach (var widget in parameters.WidgetPrefabs)
            {
                //Instantiate a widget details UI section and translate it down
                GameObject wd = Instantiate(widgetPrefab, scrollViewContent);
                //wd.transform.Translate(new Vector2(0, -1 * i * 440));

                //Set name textbox to the widget name
                GameObject txtName = wd.transform.GetChild(1).gameObject;
                txtName.GetComponent<TMP_Text>().text = widget.name;

                //Set the thumbnail image
                //GameObject txtThumb = wd.transform.GetChild(1).gameObject;
                //Texture2D thumbnail = AssetPreview.GetMiniThumbnail(widget);
                //txtThumb.GetComponent<Image>().sprite = Sprite.Create(thumbnail, new Rect(0, 0, thumbnail.width, thumbnail.height), new Vector2(1f, 1f));

                //Make scroll view longet vertically to fit the widget
                scrollViewContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, wd.GetComponent<RectTransform>().sizeDelta.y + 20);
            }
        }

        public override void Confirm()
        {
            
        }

        public override void Cancel()
        {
            OnConfirm.Invoke(null);
        }
    }
}