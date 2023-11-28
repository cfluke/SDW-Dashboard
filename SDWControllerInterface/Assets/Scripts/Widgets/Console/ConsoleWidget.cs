using SerializableData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = Logs.Logger;

namespace Widgets.Console
{
    public class ConsoleWidget : Widget
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private GameObject logPrefab;

        private void Start()
        {
            // fetch all previous logs (that may have happened before this console widget opened)
            Logger.Instance.Logs.ForEach(QueuePrintLog);
            Logger.Instance.OnAnyLog += QueuePrintLog; // subscribe to logs
        }
    
        private void OnDestroy()
        {
            Logger.Instance.OnAnyLog -= QueuePrintLog; // unsubscribe to logs
        }

        private void QueuePrintLog(string message)
        {
            MainThreadDispatcher.Instance.Enqueue(() =>
            {
                PrintLog(message);
                LayoutRebuilder.ForceRebuildLayoutImmediate(content); // fixes weird grid layout group issues
            });
        }

        private void PrintLog(string message)
        {
            GameObject logObject = Instantiate(logPrefab, content);
            TMP_Text logText = logObject.GetComponent<TMP_Text>();
            logText.text = message;
        }
        
#if UNITY_EDITOR || UNITY_DEBUG
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                Logger.Instance.Log("Log test!");
        
            if (Input.GetKeyDown(KeyCode.Alpha2))
                Logger.Instance.LogWarning("Warning test!");
        
            if (Input.GetKeyDown(KeyCode.Alpha3))
                Logger.Instance.LogError("Error test!");
        
            if (Input.GetKeyDown(KeyCode.Alpha4))
                Logger.Instance.LogSuccess("Success test!");
        }
#endif
    }
}
