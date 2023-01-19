﻿using System.IO;
using System.Runtime.CompilerServices;

public class Helper
{
    public static void LogDebug(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        Debug.Log(ComposeLogMessage(message, lineNumber, sourceFilePath));
    }

    public static void LogError(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        Debug.Log("<color=red>" + ComposeLogMessage(message, lineNumber, sourceFilePath) + "</color>");
    }

    private static string ComposeLogMessage(string message,
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string sourceFilePath = "")
    {
        return $"{Path.GetFileName(sourceFilePath)}({lineNumber}) - {message}";
    }

    public static int RandomInclusiveRange(int minVal, int maxVal)
    {
        var random = new System.Random();
        return random.Next(minVal, maxVal + 1);
    }
}
