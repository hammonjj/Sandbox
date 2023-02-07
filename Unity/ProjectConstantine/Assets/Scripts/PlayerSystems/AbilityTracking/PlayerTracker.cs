using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//Will need to update namespace later
public class PlayerTracker : MonoBehaviourBase
{
    public int Currency;
    public List<UpgradeData> PlayerUpgrades = new();
    public AbilityTracking.HealthTracker HealthTracker = new();
    public AbilityTracking.EnergyTracker EnergyTracking = new();
    public AbilityTracking.SupportAbilityTracker SupportAbilityTracker = new();

    //Secondary Ring Stats - Will delete with secondary weapon refactor
    public bool SecondaryRingActive = false;
    public int SecondaryMaxOrbs = 3;
    public GameObject SecondaryOrbPrefab;

    private static PlayerTracker _instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            EventManager.GetInstance().onEnemyDeath += OnEnemyDeath;
            EventManager.GetInstance().onUpgradePurchase += OnUpgradeAcquired;
            EventManager.GetInstance().onPlayerDeath += OnPlayerDeath;
            EditorApplication.playModeStateChanged += OnPlayModeChange;
        }
        else
        {
            Destroy(this);
        }
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
        Destroy(this);
        _instance = null;
    }

    private void OnEnemyDeath()
    {
        //Need to add currency to enemies - will hardcode for now
        LogDebug("EnemyDeath");
        Currency += 1;
    }
}

