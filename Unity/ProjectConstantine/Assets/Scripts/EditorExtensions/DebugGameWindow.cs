using UnityEditor;
using UnityEngine;

public class DebugGameWindow : EditorWindow
{
    [MenuItem("Window/DebugGameWindow")]
    public static void ShowWindow()
    {
        GetWindow(typeof(DebugGameWindow));
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(0, 0, 200, 25), "Hurt Player (10)"))
        {
            Helper.LogDebug("Hurting player for 10 damage");
        }

        if(GUI.Button(new Rect(0, 30, 200, 25), "Kill Player"))
        {
            Helper.LogDebug("Killing player");
            EventManager.GetInstance().OnPlayerDeath();
        }

        if(GUI.Button(new Rect(0, 60, 200, 25), "Kill All Enemies"))
        {
            Helper.LogDebug("Killing all enemies");
        }
    }
}
