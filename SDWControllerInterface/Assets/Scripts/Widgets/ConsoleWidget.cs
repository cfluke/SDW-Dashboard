using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleWidget : MonoBehaviour
{
    private Color normalColour = Color.white;
    private Color warningColour = Color.yellow;
    private Color errorColour = Color.red;
    public TMP_Text txtBox;
    int i = 0;

    public void LogMessage(string message)
    {
        txtBox.text += "\n" + message;
    }

    void Start()
    {
        txtBox.text = "Console initialised...";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
