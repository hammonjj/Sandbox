using System;
using UnityEditor;
using UnityEngine.SceneManagement;

public class EventManager
{
    //Game Events
    public Action<int> onEnemyDeath;
    public Action onPlayerDeath;
    public Action onEncounterEnded;
    public Action onSceneEnding;
    public Action onGameReset;
    public Action<UpgradeData> onUpgradePurchase;

    //Player Inputs
    public Action onSupportAbility;
    public Action onPlayerDash;
    public Action onPlayerPrimaryAttack;
    public Action onPlayerSecondaryAttack;
    public Action<bool> onPausePlayerController;

    //Misc. Player Actions
    public Action onPause;
    public Action<float> onEnergyUsed;
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
        onSceneEnding = null;
        onGameReset = null;
        onUpgradePurchase = null;

        onSupportAbility = null;
        onPlayerDash = null;
        onPlayerPrimaryAttack = null;
        onPlayerSecondaryAttack = null;
        onPausePlayerController = null;

        onPause = null;
        onAdvanceScenePressed = null;
        onEnergyUsed = null;

        onSpawnEnemies = null;
    }

    public void OnEnergyUsed(float energy)
    {
        onEnergyUsed?.Invoke(energy);
    }

    public void OnGameReset()
    {
        onGameReset?.Invoke();
    }

    public void OnUpgradePurchase(UpgradeData itemData)
    {
        onUpgradePurchase?.Invoke(itemData);
    }

    public void OnPausePlayerController(bool value)
    {
        onPausePlayerController?.Invoke(value);
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

    public void OnEnemyDeath(int currency)
    {
        onEnemyDeath?.Invoke(currency);
    }

    public void OnSupportAbility()
    {
        onSupportAbility?.Invoke();
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
