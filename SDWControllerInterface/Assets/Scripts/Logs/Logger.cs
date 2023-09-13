using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Logger : MonoBehaviour
{
    // use these to make the logs a little more colourful and professional looking
    [SerializeField] private Color _normalColour = Color.white;
    [SerializeField] private Color _warningColour = Color.yellow;
    [SerializeField] private Color _errorColour = Color.red;

    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _logPrefab;

    private static Logger _instance;

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

    public void Log(string message) {
        Debug.Log(message);

        /*string colorTag = "<color=#" + _normalColour + ">";  //to HEX
        string dateTime = GetDateTime();
        string endTag = "</color>";

        string tag = colorTag + dateTime + endTag;
        AddLog(tag + message);*/
    }

    public void LogWarning(string message) {
        Debug.LogWarning(message);

        /*string dateTime = GetDateTime();

        AddLog(message);*/
    }

    public void LogError(string message) {
        Debug.LogError(message);

        /*string dateTime = GetDateTime();

        AddLog(message);*/
    }

    private static string GetDateTime()
    {
        // return date time in the format of "[dd/mm/yyyy]: "
        return "";
    }

    private void AddLog(string message)
    {
        GameObject logObject = Instantiate(_logPrefab, _content);
        logObject.GetComponent<TMP_Text>().text = message;
    }
}
