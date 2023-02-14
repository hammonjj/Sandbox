using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;

//Will need to update namespace later
public class PlayerTracker : MonoBehaviourBase
{
    public int EnemiesKilled;
    public int Currency;
    public List<UpgradeData> PlayerUpgrades = new();
    public AbilityTracking.HealthTracker HealthTracker = new();
    public AbilityTracking.EnergyTracker EnergyTracking = new();
    public AbilityTracking.SupportAbilityTracker SupportAbilityTracker = new();

    private static PlayerTracker _instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
            EditorApplication.playModeStateChanged += OnPlayModeChange;

            RegisterEventListeners();
        }
        else
        {
            Destroy(this);
        }
    }

    private void RegisterEventListeners()
    {
        EventManager.GetInstance().onEnemyDeath += OnEnemyDeath;
        EventManager.GetInstance().onUpgradePurchase += OnUpgradeAcquired;
        EventManager.GetInstance().onPlayerDeath += OnPlayerDeath;   
    }

    public List<UpgradeData> GetCurrentUpgrades(Constants.Enums.UpgradeType upgradeType)
    {
        if(upgradeType == Constants.Enums.UpgradeType.All)
        {
            return PlayerUpgrades;
        }

        return PlayerUpgrades.Where(x => x.UpgradeType == upgradeType).ToList();
    }

    private void OnUpgradeAcquired(UpgradeData upgradeData)
    {
        if(upgradeData.Id == string.Empty || !PlayerUpgrades.Any(x => x.Id == upgradeData.Id))
        {
            PlayerUpgrades.Add(upgradeData);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LogDebug("OnSceneLoaded Called");
        RegisterEventListeners();
    }

    private static void OnPlayModeChange(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingPlayMode)
        {
            Helper.LogDebug("Reseting AbilityTracker");
            Destroy(_instance);
            _instance = null;
        }
    }

    private void OnPlayerDeath()
    {
        LogDebug("Player died: Reseting Tracking");
        Destroy(_instance);
        _instance = null;
    }

    private void OnEnemyDeath(int currency)
    {
        LogDebug("EnemyDeath");

        EnemiesKilled++;
        Currency += currency;
    }
}

