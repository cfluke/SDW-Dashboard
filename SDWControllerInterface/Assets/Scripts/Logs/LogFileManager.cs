using System;
using System.IO;

namespace Logs
{
    public class LogFileManager
    {
        private string _logFilePath;

        public LogFileManager(string filePath)
        {
            _logFilePath = filePath;

            // subscribe to Logger
            Logger.Instance.OnAnyLog += LogToFile;
        }

        // Callback method to handle log events and save to file
        private void LogToFile(string logMessage)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(_logFilePath, true);
                writer.WriteLine(logMessage);
            }
            catch (Exception e)
            {
                Logger.Instance.LogError($"Error writing to log file: {e.Message}");
            }
        }

        public void UnsubscribeFromLoggerEvents()
        {
            Logger.Instance.OnAnyLog -= LogToFile;
        }
    }
}