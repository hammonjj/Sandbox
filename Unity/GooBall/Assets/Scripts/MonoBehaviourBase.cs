using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Enums;

public class MonoBehaviourBase : MonoBehaviour
{
    [Header("Base")]
    public LoggingMask LoggingLevel;

    public virtual void LogDebug(string message, 
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        if((LoggingLevel & LoggingMask.Debug) != 0)
        {
            Debug.Log($"{Path.GetFileName(sourceFilePath)}({lineNumber}) - {message}");
        }
    }

    public virtual void LogWarning(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        if((LoggingLevel & LoggingMask.Warning) != 0)
        {
            Debug.LogWarning($"{Path.GetFileName(sourceFilePath)}({lineNumber}) - {message}");
        }
    }

    public virtual void LogError(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        if((LoggingLevel & LoggingMask.Error) != 0)
        {
            Debug.LogError($"{Path.GetFileName(sourceFilePath)}({lineNumber}) - {message}");
        }
    }

    public virtual void LogVerbose(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        if((LoggingLevel & LoggingMask.Verbose) != 0)
        {
            Debug.Log($"{Path.GetFileName(sourceFilePath)}({lineNumber}) - {message}");
        }
    }
}
