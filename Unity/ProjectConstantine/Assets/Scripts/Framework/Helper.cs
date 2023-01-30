using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

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

    public static float HorizontalDistance(Vector3 vec1, Vector3 vec2)
    {
        var vec1_2d = new Vector2(vec1.x, vec1.z);
        var vec2_2d = new Vector2(vec2.x, vec2.z);

        return Vector2.Distance(vec1_2d, vec2_2d);
    }
}
