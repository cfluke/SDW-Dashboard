using TMPro;
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

                //Set name textbox to the widget name
                GameObject txtName = wd.transform.GetChild(1).gameObject;
                txtName.GetComponent<TMP_Text>().text = widget.name;

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