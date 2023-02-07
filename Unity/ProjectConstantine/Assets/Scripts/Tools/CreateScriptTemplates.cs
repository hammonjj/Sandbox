using UnityEditor;
using UnityEngine;

public static class CreateScriptTemplates
{
    private static string _scriptTemplatesPath = Application.dataPath + "/Scripts/Tools/ScriptTemplates";

    [MenuItem("Assets/Create/Code/MonoBehaviorObject", priority = 39)]
    public static void CreateMonoBehaviorObjectBaseMenuItem()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
            _scriptTemplatesPath + "/NewMonoBehaviourScript.cs.txt", 
            "NewMonoBehaviourScript.cs");
    }

    [MenuItem("Assets/Create/Code/ScriptableObject", priority = 40)]
    public static void CreateScriptableObjectBaseMenuItem()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
            _scriptTemplatesPath + "/NewScriptableObjectScript.cs.txt", 
            "NewScriptableObjectScript.cs");
    }
}
