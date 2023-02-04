using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerTracker : MonoBehaviourBase
{
    public int Currency;
    public List<UpgradeData> PlayerUpgrades = new();
    public PlayerHealthTracker PlayerHealthTracker = new();

    //Secondary Ring Stats
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
            EventManager.GetInstance().onEnemyDeath += EnemyDeath;
            EventManager.GetInstance().onUpgradePurchase += Upgrade;
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

    private void Upgrade(UpgradeData upgradeData)
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
            _instance = null;
        }
    }

    private void EnemyDeath()
    {
        //Need to add currency to enemies - will hardcode for now
        LogDebug("EnemyDeath");
        Currency += 1;
    }
}

