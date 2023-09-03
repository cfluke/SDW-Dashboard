using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitWidgetSelection : MonoBehaviour
{
    
    public void Exit(GameObject widgetSelectionDialog)
    {
        Destroy(widgetSelectionDialog);
    }

}
