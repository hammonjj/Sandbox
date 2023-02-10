using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager
{
    //Game Events
    public Action<Vector3> onEnemyHelpPing;
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
    public Guid InstanceId;

    private int _sceneIndex;

    //Private Members
    private static EventManager _instance;

    public static EventManager GetInstance()
    {
        if(_instance == null)
        {
            _instance = new EventManager();
            _instance.InstanceId = Guid.NewGuid();
            _instance._sceneIndex = SceneManager.GetActiveScene().buildIndex;
        }

        return _instance;
    }

    public static void Reset()
    {
        //Eventually convert this to use reflection to reset all Actions on Reset
        if(_instance == null)
        {
            return;
        }

        if(SceneManager.GetActiveScene().buildIndex == _instance._sceneIndex)
        {
            //No need to reset since we're still in the same scene
            return;
        }

        Debug.Log("Reseting EventManager");
        _instance.onEnemyHelpPing = null;
        _instance.onEnemyDeath = null;
        _instance.onPlayerDeath = null;
        _instance.onEncounterEnded = null;
        _instance.onSceneEnding = null;
        _instance.onGameReset = null;
        _instance.onUpgradePurchase = null;

        _instance.onSupportAbility = null;
        _instance.onPlayerDash = null;
        _instance.onPlayerPrimaryAttack = null;
        _instance.onPlayerSecondaryAttack = null;
        _instance.onPausePlayerController = null;

        _instance.onPause = null;
        _instance.onAdvanceScenePressed = null;
        _instance.onEnergyUsed = null;

        _instance.onSpawnEnemies = null;

        _instance = null;
    }

    public void OnEnemyHelpPing(Vector3 playerPos)
    {
        onEnemyHelpPing?.Invoke(playerPos);
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
        Debug.Log($"OnPrimaryAttackInvoked");
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
