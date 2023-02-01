using System;
using UnityEditor;
using UnityEngine.SceneManagement;

public class EventManager
{
    //Game Events
    public Action onEnemyDeath;
    public Action onPlayerDeath;
    public Action onEncounterEnded;
    public Action onSceneEnding;

    //Player Inputs
    public Action onUseItem;
    public Action onPlayerDash;
    public Action onPlayerPrimaryAttack;
    public Action onPlayerSecondaryAttack;

    //Misc. Player Actions
    public Action onPause;
    public Action<bool> onAdvanceScenePressed;

    //Debugging
    public Action onSpawnEnemies;

    //Private Members
    private static EventManager _instance;

    public static EventManager GetInstance()
    {
        if(_instance == null)
        {
            _instance = new EventManager();
            SceneManager.activeSceneChanged += _instance.OnSceneLoaded;
            EditorApplication.playModeStateChanged += OnPlayModeChange;
        }

        return _instance;
    }

    private static void OnPlayModeChange(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingPlayMode)
        {
            Helper.LogDebug("Reseting Event Manager");
            _instance = null;
        }
    }

    public void OnSceneLoaded(Scene scene, Scene mode)
    {
        Helper.LogDebug("EventManager Reloading");

        onEnemyDeath = null;
        onPlayerDeath = null;
        onEncounterEnded = null;
        onUseItem = null;
        onPlayerDash = null;
        onPlayerPrimaryAttack = null;
        onPlayerSecondaryAttack = null;
        onPause = null;
        onAdvanceScenePressed = null;
        onSceneEnding = null;
    }

    public void OnSceneEnding()
    {
        onSceneEnding?.Invoke();
    }

    public void OnSpawnEnemies()
    {
        onSpawnEnemies?.Invoke();
    }

    public void OnPlayerDeath()
    {
        onPlayerDeath?.Invoke();
    }

    public void OnEnemyDeath()
    {
        onEnemyDeath?.Invoke();
    }

    public void OnUseItem()
    {
        onUseItem?.Invoke();
    }

    public void OnPlayerDash()
    {
        onPlayerDash?.Invoke();
    }

    public void OnPlayerPrimaryAttack()
    {
        onPlayerPrimaryAttack?.Invoke();
    }

    public void OnPlayerSecondaryAttack()
    {
        onPlayerSecondaryAttack?.Invoke();
    }

    public void OnAdvanceScenePressed(bool value)
    {
        onAdvanceScenePressed?.Invoke(value);
    }

    public void OnPause()
    {
        onPause?.Invoke();
    }

    public void OnEncounterEnded()
    {
        onEncounterEnded?.Invoke();
    }
}
