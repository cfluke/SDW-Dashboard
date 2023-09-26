using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnMinimise : MonoBehaviour
{
    [SerializeField] GameObject minimisedTab;
    GameObject _widget = null;
    
    //Sets the widget that corresponds to this tab
    public void SetWidget(GameObject widget)
    {
        _widget = widget;
    }

    //Reopens the corresponding widget
    public void ReOpen()
    {
        if(_widget != null)
        {
            _widget.SetActive(true);
        }
        Destroy(minimisedTab);
    }
}
