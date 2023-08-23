using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetContainer : MonoBehaviour
{
    public void Close(GameObject container)
    {
        Destroy(container);
    }
}
