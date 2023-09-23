using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DataGathererWidget : MonoBehaviour
{
    TMP_Text GetInterfaceConsole()
    {
        GameObject consoleText = GameObject.Find("ConsoleText");
        if (consoleText != null)
        {
            return consoleText.GetComponent<TMP_Text>();
        }
        return null;
    }
    void LogConsoleMessage(string consoleMessage)
    {
        //Check if console is now available
        if (consoleOutput == null)
        {
            consoleOutput = GetInterfaceConsole();
        }
        if (consoleOutput != null)
        {
            consoleOutput.text += "\n" + consoleMessage;
        }
    }
    bool ReadAPIFiles()
    {
        try
        {





            return true;
        } catch(Exception e)
        {
            LogConsoleMessage(e.ToString());
            return false;
        }
    }
    bool CreateAPICallers()
    {
        try
        {





            return true;
        }
        catch (Exception e)
        {
            LogConsoleMessage(e.ToString());
            return false;
        }
    }
    bool GetDataFromCallers()
    {
        try
        {





            return true;
        }
        catch (Exception e)
        {
            LogConsoleMessage(e.ToString());
            return false;
        }
    }


    public TMP_Text textOutput;
    public TMP_Text consoleOutput;
    void Start()
    {
        textOutput = this.GetComponent<TMP_Text>();
        consoleOutput = GetInterfaceConsole();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
