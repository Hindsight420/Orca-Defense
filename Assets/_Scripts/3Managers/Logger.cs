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
    private readonly Queue MessageQueue = new();
    private bool isCurrentlyLogging;

    public enum LogType
    {
        Error,
        Debug,
        Information
    }

    internal void LogMessage(string message, LogType logType)
    {
        LogMessages(new List<string> { message }, logType);
    }

    public void LogMessages(List<string> messages, LogType logType)
    {
        if (!messages.Any()) return;
        foreach (string message in messages)
        {
            var logMessage = Instantiate(LogPrefab, transform).GetComponent<LogMessage>();
            logMessage.Initialise(message, logType);

            MessageQueue.Enqueue(logMessage);
        }

        if (!isCurrentlyLogging) StartCoroutine(LogMessagesCoroutine());
    }

    public void LogError (string message)
    {
        LogMessage(message, LogType.Error);
    }

    public void LogErrors(List<string> messages)
    {
        LogMessages(messages, LogType.Error);
    }

    public void LogDebug(string message)
    {
        LogMessage(message, LogType.Debug);
    }

    public void LogDebug(List<string> messages)
    {
        LogMessages(messages, LogType.Debug);
    }

    public IEnumerator LogMessagesCoroutine()
    {
        isCurrentlyLogging = true;
        if (Time.time - timeOfLastLog < MINIMUM_TIME_BETWEEN_LOGS)
        {
            var timeUntilNextLogAllowed = MINIMUM_TIME_BETWEEN_LOGS - (Time.time - timeOfLastLog);
            Debug.Log(timeUntilNextLogAllowed);
            timeOfLastLog = Time.time + timeUntilNextLogAllowed;
            yield return new WaitForSeconds(timeUntilNextLogAllowed);
        }

        LogMessage logMessage = (LogMessage)MessageQueue.Dequeue();
        logMessage.Show();

        timeOfLastLog = Time.time;
        if (MessageQueue.Count > 0) StartCoroutine(LogMessagesCoroutine());
        else isCurrentlyLogging = false;
    }
}
