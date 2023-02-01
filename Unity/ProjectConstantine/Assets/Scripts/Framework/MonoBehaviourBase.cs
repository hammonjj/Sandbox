using System;
using System.IO;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MonoBehaviourBase : MonoBehaviour
{
    [Flags]
    public enum LoggingMask
    {
        Debug = (1 << 0),
        Warning = (1 << 1),
        Error = (1 << 2),
        Verbose = (1 << 3),
    }

    [Header("Base")]
    public LoggingMask LoggingLevel;

    protected string MessageEnding;

    protected T VerifyComponent<T>()
    {
        var ret = GetComponent<T>();
        if(ret == null)
        {
            LogError($"Dependency is null - Component Name: {typeof(T).FullName}");
            Debug.Break();
        }

        return ret;
    }

    protected T VerifyComponent<T>(string tag)
    {
        var ret = GameObject.FindGameObjectWithTag(tag).GetComponent<T>();
        if(ret == null)
        {
            LogError($"Dependency is null - Tag: {tag} - Component Name: {typeof(T).FullName}");
            Debug.Break();
        }

        return ret;
    }

    protected IEnumerator PauseForTime(float interval)
    {
        yield return new WaitForSeconds(interval);
    }

    protected virtual void LogDebug(string message, 
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        if((LoggingLevel & LoggingMask.Debug) != 0)
        {
            Debug.Log(ComposeLogMessage(message, lineNumber, sourceFilePath));
        }
    }

    protected virtual void LogWarning(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        if((LoggingLevel & LoggingMask.Warning) != 0 ||
            (LoggingLevel & LoggingMask.Error) != 0)
        {
            Debug.Log("<color=yellow>" + ComposeLogMessage(message, lineNumber, sourceFilePath) + "</color>");
        }
    }

    protected virtual void LogError(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        if((LoggingLevel & LoggingMask.Debug) != 0 ||
            (LoggingLevel & LoggingMask.Warning) != 0 ||
            (LoggingLevel & LoggingMask.Error) != 0)
        {
            Debug.Log("<color=red>" + ComposeLogMessage(message, lineNumber, sourceFilePath) + "</color>");
        }
    }

    protected virtual void LogVerbose(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        if((LoggingLevel & LoggingMask.Verbose) != 0)
        {
            Debug.Log(ComposeLogMessage(message, lineNumber, sourceFilePath));
        }
    }

    private string ComposeLogMessage(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        return $"{Path.GetFileName(sourceFilePath)}({lineNumber}) - {message}" + 
            (string.IsNullOrEmpty(MessageEnding) ? string.Empty : " - " + MessageEnding);
    }
}