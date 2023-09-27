using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Diagnostics;
using SSADataStreams;
using System.IO;
using System.Threading.Tasks;

public class DataGathererWidget : MonoBehaviour
{
    [SerializeField] public TMP_Text textOutput;
    [SerializeField] public ConsoleWidget consoleOutput;
    private List<APICaller> APICallerList = new List<APICaller>();
    void Start()
    {
        textOutput = this.GetComponent<TMP_Text>();
        consoleOutput = GetInterfaceConsole();

        //Create API callers
        SpaceWeatherServiceAPICaller SpaceWeatherServiceCaller = new SpaceWeatherServiceAPICaller();
        NOAASWPCAPICaller NOAASWPCCaller = new NOAASWPCAPICaller();
        //Add API callers to list to run through
        APICallerList.Add(SpaceWeatherServiceCaller);
        APICallerList.Add(NOAASWPCCaller);
    }

    // Update is called once per frame
    void Update()
    {
        //LogConsoleMessage("Testing...");
    }

    ConsoleWidget GetInterfaceConsole()
    {
        GameObject consoleText = GameObject.Find("Console(Clone)");

        UnityEngine.Debug.Log(consoleText);
        if (consoleText != null)
        {
            return consoleText.GetComponent<ConsoleWidget>();
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
            consoleOutput.LogMessage(consoleMessage);
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

    public async void RunDataGathererAsync()
    {
        //Run through each caller, write response to file
        foreach (APICaller APICaller in APICallerList)
        {
            //Run API calls and await results
            Console.WriteLine(await APICaller.CallAPIAsync());
        }
        //Move all collected data to new folder with date
        try
        {
            //Create CurrentRun directory to ensure it exists
            Directory.CreateDirectory(".\\Data\\CurrentRun");
            string sourceDirName = ".\\Data\\CurrentRun";
            string destDirName = ".\\Data\\DataCalls" + "_" + DateTime.Now.ToString("yyyyMMdd");
            //Append number to folder name if already exists
            int folderIndex = 0;
            bool folderMoveFailed = true;
            string destDirNameTemp = destDirName;
            while (folderMoveFailed)
            {
                try
                {
                    Directory.Move(sourceDirName, destDirNameTemp);
                    folderMoveFailed = false;
                }
                catch (IOException exp)
                {
                    //Console.WriteLine(exp.Message);
                    folderMoveFailed = true;
                    destDirNameTemp = destDirName + "_" + folderIndex;
                    folderIndex++;
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully collected data and stored in: " + destDirNameTemp);

        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.ToString());
        }
    }
}
