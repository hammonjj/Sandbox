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

    public override void Initialize()
    {
        _abilityTracker = GameObject.FindGameObjectWithTag(
            Constants.Tags.GameStateManager).GetComponent<PlayerTracker>();

        GetUpgrades();
    }

    private void GetUpgrades()
    {
        var upgrades = _abilityTracker.GetCurrentUpgrades(Constants.Enums.UpgradeType.PrimaryAttack);
        foreach(var upgrade in upgrades)
        {
            switch(upgrade.AttackUpgrade)
            {
                case Constants.Enums.AttackUpgrade.AttackCrit:
                    CanCrit = true; ;
                    CritModifier = 50f;
                    CritPercent = 10f;
                    break;
                case Constants.Enums.AttackUpgrade.ProjectilePassThrough:
                    CanPassThroughEnemies = true;
                    break;
            }
        }
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
        else if(other.tag == Constants.Tags.Player || other.tag == Constants.Tags.Projectile)
        {
            destroy = false;
        }

        return destroy;
    }
}
