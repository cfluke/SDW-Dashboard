using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleWidget : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject logPrefab;

    private void Start()
    {
        Logger.Instance.OnLogReceived += QueuePrintLog;
        Logger.Instance.OnWarningReceived += QueuePrintLog;
        Logger.Instance.OnErrorReceived += QueuePrintLog;
    }
    
    private void OnDestroy()
    {
        Logger.Instance.OnLogReceived -= QueuePrintLog;
        Logger.Instance.OnErrorReceived -= QueuePrintLog;
        Logger.Instance.OnWarningReceived -= QueuePrintLog;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Logger.Instance.Log("Log test! Log test! Log test! Log test! Log test! Log test! Log test! ");
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Logger.Instance.LogWarning("Warning test!");
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Logger.Instance.LogError("Error test!");
    }

    private void QueuePrintLog(string message)
    {
        MainThreadDispatcher.Instance.Enqueue(() => { PrintLog(message); });
    }

    private void PrintLog(string message)
    {
        GameObject logObject = Instantiate(logPrefab, content);
        TMP_Text logText = logObject.GetComponent<TMP_Text>();
        logText.text = message;
    }
}
