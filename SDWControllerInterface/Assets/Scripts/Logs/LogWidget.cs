using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogWidget : MonoBehaviour
{
    [SerializeField] private Color normalColour = Color.white;
    [SerializeField] private Color warningColour = Color.yellow;
    [SerializeField] private Color errorColour = Color.red;

    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _logPrefab;
    // Start is called before the first frame update

    private void OnEnable()
    {
        Logger.Instance.OnLogReceived += HandleLogReceived;
        Logger.Instance.OnErrorReceived += HandleErrorReceived;
        Logger.Instance.OnWarningReceived += HandleWarningReceived;
    }
    private void OnDisable()
    {
        Logger.Instance.OnLogReceived -= HandleLogReceived;
        Logger.Instance.OnErrorReceived -= HandleErrorReceived;
        Logger.Instance.OnWarningReceived -= HandleWarningReceived;
    }

    private void HandleLogReceived(string obj)
    {
        AddLog(obj, normalColour);
    }

    private void HandleErrorReceived(string obj)
    {
        AddLog(obj, errorColour);
    }
    private void HandleWarningReceived(string obj)
    {
        AddLog(obj, warningColour);
    }
    
    private void AddLog(string message, Color color) 
    {
        GameObject logObject = Instantiate(_logPrefab, _content);
        logObject.GetComponent<TMP_Text>().text = message;
    }
}
