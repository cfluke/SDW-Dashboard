using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class changeColourWidget : MonoBehaviour
{
    public TMP_Text txtBox;
    int i = 0;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        txtBox.text = i.ToString();
        i++;
    }
}