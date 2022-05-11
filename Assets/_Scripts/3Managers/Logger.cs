using Assets._Scripts._3Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    [SerializeField]
    private GameObject LogPrefab;

    public enum LogType
    {
        Error,
        Debug,
        Information
    }

    public void LogMessage (string message, LogType logType)
    {
        var log = Instantiate(LogPrefab, transform);
        log.GetComponent<LogMessage>().Initialise(message, logType);
    }
}
