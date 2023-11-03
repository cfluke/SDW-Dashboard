using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Logger : MonoBehaviour
{
    // use these to make the logs a little more colourful and professional looking
    private static Logger _instance;

    private string logFileName; // Name of the log file
    private string logFilePath; // Full path to the log file

    private string sessionIdentifier; // Identifier for the current session.

    private int maxSessionCount = 10; // Maximum number of log entries to keep.

    private List<string> logEntries = new List<string>();

    public event Action<string> OnLogReceived;
    public event Action<string> OnWarningReceived;
    public event Action<string> OnErrorReceived;
        
    // singleton
    public static Logger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Logger>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("Logger");
                    _instance = singletonObject.AddComponent<Logger>();
                }
            }

            return _instance;
        }
    }

    public void LogSuccess(string message)
    {
    }

    public void Log(string message) 
    {
        Debug.Log(message);

        /*string colorTag = "<color=#" + _normalColour + ">";  //to HEX
        string dateTime = GetDateTime();
        string endTag = "</color>";

        string tag = colorTag + dateTime + endTag;
        AddLog(tag + message);*/

        //string dateTime = GetDateTime();
        //string formattedMessage = $"{dateTime} {message}";
        string formattedMessage = $"<color=\"white\">[Log]</color>: {message}";
        AddLog(formattedMessage);
        Debug.Log(formattedMessage);

        // Save the log message to the log file.
        //SaveLogToFile(formattedMessage);
        
        OnLogReceived?.Invoke(formattedMessage);
    }

    public void LogWarning(string message) 
    {
        Debug.LogWarning(message);

        /*string dateTime = GetDateTime();

        AddLog(message);*/

        string dateTime = GetDateTime();
        //string formattedMessage = $"{dateTime} [Warning] {message}";
        string formattedMessage = $"{dateTime}<color=\"yellow\">[Warning]</color>: {message}";

        AddLog(formattedMessage);

        // Save the warning message to the log file.
        //SaveLogToFile(formattedMessage);
        
        OnWarningReceived?.Invoke(formattedMessage);
    }

    public void LogError(string message) 
    {
        Debug.LogError(message);

        /*string dateTime = GetDateTime();

        AddLog(message);*/

        string dateTime = GetDateTime();
        //string formattedMessage = $"{dateTime} [Error] {message}";
        string formattedMessage = $"{dateTime}<color=\"red\">[Error]</color>: {message}";

        AddLog(formattedMessage);

        // Save the error message to the log file.
        //SaveLogToFile(formattedMessage);
        
        OnErrorReceived?.Invoke(formattedMessage);
    }
//}
    
    private void Start()
    {
        sessionIdentifier = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
        logFileName = sessionIdentifier + ".log";
        
        // Update logFilePath with the correct path in Start()
        //logFilePath = Path.Combine(Application.persistentDataPath, "/Logs/");
        
        //sessionIdentifier = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        
        // Check if we need to remove the oldest session identifier(s) to maintain the maximum count.
        //CheckAndRemoveOldestSessions();

        // Load log entries from the log file
        //LoadLogFromFile();
        
    }

    private List<string> GetLogFiles()
    {
        List<string> logFiles = new List<string>();

        string[] files = Directory.GetFiles(logFilePath, "*.log");

        foreach (string file in files)
        {
            logFiles.Add(file);
        }

        return logFiles;
    }
    
    private void CheckAndRemoveOldestSessions()
    {
        List<string> logFiles = GetLogFiles();
        

        if (logFiles.Count > maxSessionCount)
        {
            // Sort files by creation time in ascending order (oldest first).
            logFiles = logFiles.OrderBy(f => File.GetCreationTime(f)).ToList();

            // Calculate the number of files to delete.
            int filesToDelete = logFiles.Count - maxSessionCount;

            for (int i = 0; i < filesToDelete; i++)
            {
                File.Delete(logFiles[i]);
            }
        }
    }
    private static string GetDateTime()
    {
        // return date time in the format of "[dd/mm/yyyy]: "
       
        DateTime now = DateTime.Now;

        string formattedDateTime = now.ToString("dd/MM/yyyy HH:mm:ss");

        // Add the desired format
        formattedDateTime = "<color=#D3D3D3>[" + formattedDateTime + "]</color>";

        return formattedDateTime;
    }

    private void SaveLogToFile(string message)
    {
        try
        {
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                // Wrap the log message in an HTML paragraph tag.
                writer.WriteLine($"{message}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving log message to file: " + e.Message);
        }
    }

    private void LoadLogFromFile()
    {
        if (File.Exists(logFilePath))
        {
            try
            {
                // Read log entries from the log file.
                string[] lines = File.ReadAllLines(logFilePath);

                // Load the most recent log entries up to the maximum count.
                for (int i = 0; i < lines.Length; i++)
                {
                    logEntries.Add(lines[i]);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading log file: " + e.Message);
            }
        }
    }
    private void AddLog(string message)
    {
        logEntries.Add(message);
    }
}

