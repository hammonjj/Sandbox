using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrimaryOrbData", menuName = "Orbs/PrimaryOrbData")]
public class PrimaryOrbData : BaseOrbData
{
    public bool CanCrit = false;
    public float CritPercent = 0f;
    public float CritModifier = 10f;

    public bool CanPassThroughEnemies = false;

    private PlayerTracker _abilityTracker;

    private void Awake()
    {
        _abilityTracker = GameObject.FindGameObjectWithTag(
            Constants.Tags.GameStateManager).GetComponent<PlayerTracker>();

        CanCrit = _abilityTracker.PrimaryOrbUpgradeTracker.CanCrit;
        CritModifier = _abilityTracker.PrimaryOrbUpgradeTracker.CritModifier;
        CritPercent = _abilityTracker.PrimaryOrbUpgradeTracker.CritPercent;

        EventManager.GetInstance().onUpgradePurchase += Upgrade;
        EventManager.GetInstance().onSceneEnding += SceneEnding;
    }

    private void Upgrade(ShopItemData itemData)
    {
        LogDebug($"Upgrade: {itemData.Name}");
        if(itemData.UpgradeType != Constants.Enums.UpgradeType.PrimaryAttack)
        {
            return;
        }

        switch(itemData.AttackUpgrade)
        {
            case Constants.Enums.AttackUpgrade.AttackCrit:
                CanCrit = true;
                CritPercent = 10f;
                break;
            case Constants.Enums.AttackUpgrade.ProjectilePassThrough:
                CanPassThroughEnemies = true;
                break;
        }
    }

    private void SceneEnding()
    {
        LogDebug("Scene Ending");
        _abilityTracker = GameObject.FindGameObjectWithTag(Constants.Tags.GameStateManager).GetComponent<PlayerTracker>();
        _abilityTracker.PrimaryOrbUpgradeTracker.CanCrit = CanCrit;
        _abilityTracker.PrimaryOrbUpgradeTracker.CritModifier = CritModifier;
        _abilityTracker.PrimaryOrbUpgradeTracker.CritPercent = CritPercent;
    }

    public override bool OnHit(Collider other, bool hasBeenFired)
    {
        var destroy = true;
        if(other.tag == Constants.Tags.Enemy)
        {
            LogDebug("Orb Hitting Enemy (Fired)");
            var baseEnemy = other.GetComponent<BaseEnemy>();
            if(baseEnemy == null)
            {
                LogError($"Failed to get enemy base - {other.gameObject.name}");
                Debug.Break();
            }
            else
            {
                baseEnemy.TakeDamage(AttackDamage);
                destroy = !CanPassThroughEnemies;
            }
        }

        return destroy;
    }
}
