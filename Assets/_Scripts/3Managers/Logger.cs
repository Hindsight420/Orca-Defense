using Assets._Scripts._3Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Logger : Singleton<Logger>
{
    [SerializeField]
    private GameObject LogPrefab;
    private float timeOfLastLog;
    private readonly float MINIMUM_TIME_BETWEEN_LOGS = 1f;

    public enum LogType
    {
        Error,
        Debug,
        Information
    }

    public void LogMessage (string message, LogType logType)
    {
        StartCoroutine(LogMessageCoroutine(message, logType));
    }

    public IEnumerator LogMessageCoroutine (string message, LogType logType)
    {
        if (Time.time - timeOfLastLog < MINIMUM_TIME_BETWEEN_LOGS)
        {
            var timeUntilNextLogAllowed = MINIMUM_TIME_BETWEEN_LOGS - (Time.time - timeOfLastLog);
            Debug.Log(timeUntilNextLogAllowed);
            timeOfLastLog = Time.time + timeUntilNextLogAllowed;
            yield return new WaitForSeconds(timeUntilNextLogAllowed);
        }

        var log = Instantiate(LogPrefab, transform);
        log.GetComponent<LogMessage>().Initialise(message, logType);
        timeOfLastLog = Time.time;
    }

    public void LogMessages (List<string> messages, LogType logType)
    {
        if (!messages.Any()) { return; }
        foreach (string message in messages)
        {
            LogMessage(message, logType);
        }
    }
}
