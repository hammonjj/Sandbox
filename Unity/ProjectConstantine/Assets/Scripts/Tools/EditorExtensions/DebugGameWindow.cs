using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            var enemies = GameObject.FindGameObjectsWithTag(Constants.Tags.Enemy);
            foreach(var enemy in enemies)
            {
                EventManager.GetInstance().OnEnemyDeath(1);
                Destroy(enemy);
            }
        }

        if(GUI.Button(new Rect(0, 90, 200, 25), "Spawn Enemies"))
        {
            Helper.LogDebug("Spawning Enemies");
            EventManager.GetInstance().OnSpawnEnemies();
        }

        if(GUI.Button(new Rect(0, 120, 200, 25), "Load Zone 1 Start"))
        {
            Helper.LogDebug("Load Zone 1 Start");
            EditorSceneManager.OpenScene("Assets/Scenes/ZoneOne/" +
                Constants.Enums.Scenes.Zone1_Start.ToString() + ".unity");
        }

        if(GUI.Button(new Rect(0, 150, 200, 25), "Load Scene"))
        {
            var menu = new GenericMenu();
            foreach(int scene in Enum.GetValues(typeof(Constants.Enums.Scenes)))
            {
                var sceneName = ((Constants.Enums.Scenes)scene).ToString();
                AddSceneToMenu(menu, sceneName, sceneName);
            }

            menu.ShowAsContext();
        }

        if(GUI.Button(new Rect(0, 180, 200, 25), "Use 10 Energy"))
        {
            Helper.LogDebug("Using 10 Energy");
            var playerEnergy = GameObject.FindGameObjectWithTag(Constants.Tags.Player)
                .GetComponent<PlayerEnergy>();

            playerEnergy.UseEnergy(10);
        }
    }

    private void AddSceneToMenu(GenericMenu menu, string menuPath, string sceneName)
    {
        menu.AddItem(new GUIContent(menuPath), true, () => OnSceneSelected(sceneName));
    }

    private void OnSceneSelected(string sceneName)
    {
        if(SceneManager.GetActiveScene().isDirty)
        {
            if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                //Eventually this will need to be fixed
                EditorSceneManager.OpenScene(sceneName == "WorldHub" ?
                    $"Assets/Scenes/{sceneName}.unity" :
                    $"Assets/Scenes/ZoneOne/{sceneName}.unity");
            }
        }
        else
        {
            EditorSceneManager.OpenScene(sceneName == "WorldHub" ?
                    $"Assets/Scenes/{sceneName}.unity" :
                    $"Assets/Scenes/ZoneOne/{sceneName}.unity");
        }
    }
}
