using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScriptableObjectBase : ScriptableObject
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

    public virtual void LogDebug(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        if((LoggingLevel & LoggingMask.Debug) != 0)
        {
            Debug.Log(ComposeLogMessage(message, lineNumber, sourceFilePath));
        }
    }

    public virtual void LogWarning(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        if((LoggingLevel & LoggingMask.Warning) != 0 ||
            (LoggingLevel & LoggingMask.Error) != 0)
        {
            Debug.Log("<color=yellow>" + ComposeLogMessage(message, lineNumber, sourceFilePath) + "</color>");
        }
    }

    public virtual void LogError(string message,
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

    public virtual void LogVerbose(string message,
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
