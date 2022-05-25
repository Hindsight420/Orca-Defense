using Assets._Scripts._3Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Logger : Singleton<Logger>
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

    private IEnumerator LogMultipleMessagesAsync (List<string> messages, LogType logType)
    {
        foreach (string message in messages)
        {
            LogMessage(message, logType);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void LogMessages (List<string> messages, LogType logType)
    {
        if (!messages.Any()) { return; }
        StartCoroutine(LogMultipleMessagesAsync(messages, logType));
    }
}
