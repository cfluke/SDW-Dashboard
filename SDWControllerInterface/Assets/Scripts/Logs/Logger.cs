using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class Logger : MonoBehaviour
{
    // use these to make the logs a little more colourful and professional looking
    [SerializeField] private Color _normalColour = Color.white;
    [SerializeField] private Color _warningColour = Color.yellow;
    [SerializeField] private Color _errorColour = Color.red;

    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _logPrefab;

    private static Logger _instance;

    private string logFileName = "log.txt"; // Name of the log file
    private string logFilePath; // Full path to the log file

    private string sessionIdentifier; // Identifier for the current session.
    private List<string> sessionIdentifiers = new List<string>(); // List to store session identifiers.

    private int maxSessionCount = 10; // Maximum number of log entries to keep.

    private List<string> logEntries = new List<string>();


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

    public void Log(string message) 
    {
        Debug.Log(message);

        /*string colorTag = "<color=#" + _normalColour + ">";  //to HEX
        string dateTime = GetDateTime();
        string endTag = "</color>";

        string tag = colorTag + dateTime + endTag;
        AddLog(tag + message);*/

        string dateTime = GetDateTime();
        //string formattedMessage = $"{dateTime} {message}";
        string formattedMessage = $"{dateTime} [Session {sessionIdentifier}] {message}";

        AddLog(formattedMessage);

        // Save the log message to the log file.
        SaveLogToFile(formattedMessage);
    }

    public void LogWarning(string message) 
    {
        Debug.LogWarning(message);

        /*string dateTime = GetDateTime();

        AddLog(message);*/

        string dateTime = GetDateTime();
        //string formattedMessage = $"{dateTime} [Warning] {message}";
        string formattedMessage = $"{dateTime} [Session { sessionIdentifier}] { message}";

        AddLog(formattedMessage);

        // Save the warning message to the log file.
        SaveLogToFile(formattedMessage);
    }

    public void LogError(string message) 
    {
        Debug.LogError(message);

        /*string dateTime = GetDateTime();

        AddLog(message);*/

        string dateTime = GetDateTime();
        //string formattedMessage = $"{dateTime} [Error] {message}";
        string formattedMessage = $"{dateTime} [Error] [Session {sessionIdentifier}] {message}";

        AddLog(formattedMessage);

        // Save the error message to the log file.
        SaveLogToFile(formattedMessage);
    }
//}
    
    private void Start()
    {
        // Update logFilePath with the correct path in Start()
        logFilePath = Path.Combine(Application.persistentDataPath, logFileName);

        // Generate a unique session identifier based on the current date and time when the application starts.
        sessionIdentifier = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        // Load session identifiers from the log file
        LoadSessionIdentifiers();

        // Check if we need to remove the oldest session identifier(s) to maintain the maximum count.
        CheckAndRemoveOldestSessions();

        // Load log entries from the log file
        LoadLogFromFile();
    }

    private void LoadSessionIdentifiers()
    {
        if (File.Exists(logFilePath))
        {
            try
            {
                // Read session identifiers from the log file.
                string[] lines = File.ReadAllLines(logFilePath);
                sessionIdentifiers.AddRange(lines);
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading session identifiers: " + e.Message);
            }
        }
    }

    private void CheckAndRemoveOldestSessions()
    {
        // Sort the session identifiers based on their timestamps.
        sessionIdentifiers.Sort();

        // Check if the number of session identifiers exceeds the maximum count.
        if (sessionIdentifiers.Count > maxSessionCount)
        {
            // Determine how many session identifiers need to be removed.
            int removeCount = sessionIdentifiers.Count - maxSessionCount;

            // Remove the oldest session identifiers.
            sessionIdentifiers.RemoveRange(0, removeCount);
        }
    }
    private static string GetDateTime()
    {
        // return date time in the format of "[dd/mm/yyyy]: "
       
        DateTime now = DateTime.Now;

        string formattedDateTime = now.ToString("dd/MM/yyyy HH:mm:ss");

        // Add the desired format
        formattedDateTime = "[" + formattedDateTime + "]: ";

        return formattedDateTime;
    }

    private void AddLog(string message)
    {
        GameObject logObject = Instantiate(_logPrefab, _content);
        logObject.GetComponent<TMP_Text>().text = message;

        // Add the log entry to the list.
        logEntries.Add(message);

        // If the number of log entries exceeds the maximum, remove the oldest one.
        if (logEntries.Count > maxSessionCount)
        {
            logEntries.RemoveAt(0);
        }
    }

    private void SaveLogToFile(string message)
    {
        try
        {
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                // Wrap the log message in an HTML paragraph tag.
                writer.WriteLine($"<p>{message}</p>");
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
                int startIndex = Mathf.Max(0, lines.Length - maxSessionCount);
                for (int i = startIndex; i < lines.Length; i++)
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

    
}
