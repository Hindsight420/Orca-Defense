using Assets._Scripts._3Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Logger : Singleton<Logger>
{
    [SerializeField]
    private GameObject _logPrefab;
    private float _timeOfLastLog;
    private const float MINIMUM_TIME_BETWEEN_LOGS = 1f;
    private readonly Queue _messageQueue = new();
    private bool _isCurrentlyLogging;

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
            var logMessage = Instantiate(_logPrefab, transform).GetComponent<LogMessage>();
            logMessage.Initialise(message, logType);

            _messageQueue.Enqueue(logMessage);
        }

        if (!_isCurrentlyLogging) StartCoroutine(LogMessagesCoroutine());
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
        _isCurrentlyLogging = true;
        if (Time.time - _timeOfLastLog < MINIMUM_TIME_BETWEEN_LOGS)
        {
            var timeUntilNextLogAllowed = MINIMUM_TIME_BETWEEN_LOGS - (Time.time - _timeOfLastLog);
            Debug.Log(timeUntilNextLogAllowed);
            _timeOfLastLog = Time.time + timeUntilNextLogAllowed;
            yield return new WaitForSeconds(timeUntilNextLogAllowed);
        }

        LogMessage logMessage = (LogMessage)_messageQueue.Dequeue();
        logMessage.Show();

        _timeOfLastLog = Time.time;
        if (_messageQueue.Count > 0) StartCoroutine(LogMessagesCoroutine());
        else _isCurrentlyLogging = false;
    }
}
