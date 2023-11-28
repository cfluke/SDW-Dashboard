using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logs
{
    public class Logger : MonoBehaviour
    {
        // callback functions for any class which wants to listen in on logs
        public event Action<string> OnAnyLog;
        public event Action<string> OnSuccess;
        public event Action<string> OnLog;
        public event Action<string> OnWarning;
        public event Action<string> OnError;

        public List<string> Logs { get; } = new();
    
        #region singleton
    
        private static Logger _instance;
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
    
        #endregion

        private void Awake()
        {
            OnAnyLog += s => Logs.Add(s);
        }

        public void LogSuccess(string message)
        {
#if UNITY_EDITOR || UNITY_DEBUG
            Debug.Log(message);
#endif

            // log in the format [DD/MM/YYYY HH:MM:SS][Log]: <message>
            string formattedMessage = FormatMessage(Color.green, "Success", message);
            // Debug.Log to check if this line is being executed
            Debug.Log("LogSuccess invoked: " + formattedMessage);
            OnSuccess?.Invoke(formattedMessage);
            OnAnyLog?.Invoke(formattedMessage);
        }

        public void Log(string message) 
        {
#if UNITY_EDITOR || UNITY_DEBUG
            Debug.Log(message);
#endif

            // log in the format [DD/MM/YYYY HH:MM:SS][Log]: <message>
            string formattedMessage = FormatMessage(Color.white, "Log", message);
            OnLog?.Invoke(formattedMessage);
            OnAnyLog?.Invoke(formattedMessage);
        }

        public void LogWarning(string message) 
        {
#if UNITY_EDITOR || UNITY_DEBUG
            Debug.LogWarning(message);
#endif

            // log in the format [DD/MM/YYYY HH:MM:SS][Warning]: <message>
            string formattedMessage = FormatMessage(Color.yellow, "Warning", message);
            OnWarning?.Invoke(formattedMessage);
            OnAnyLog?.Invoke(formattedMessage);
        }

        public void LogError(string message) 
        {
#if UNITY_EDITOR || UNITY_DEBUG
            Debug.LogError(message);
#endif
        
            // log in the format [DD/MM/YYYY HH:MM:SS][Error]: <message>
            string formattedMessage = FormatMessage(Color.red, "Error", message);
            OnError?.Invoke(formattedMessage);
            OnAnyLog?.Invoke(formattedMessage);
        }

        private string FormatMessage(Color colour, string logType, string message)
        {
            string dateTime = GetDateTime();
            return $"{dateTime}<color=#{ColorToHex(colour)}>[{logType}]</color>: {message}";
        }
    
        private string ColorToHex(Color color)
        {
            Color32 color32 = color;
            return color32.r.ToString("X2") + color32.g.ToString("X2") + color32.b.ToString("X2") + color32.a.ToString("X2");
        }
    
        private static string GetDateTime()
        {
            // return date time in the format of "[dd/mm/yyyy]"
            DateTime now = DateTime.Now;
            string formattedDateTime = now.ToString("dd/MM/yy HH:mm:ss");

            // Add the desired format
            formattedDateTime = "<color=#D3D3D3>[" + formattedDateTime + "]</color>";
            return formattedDateTime;
        }
    }
}

