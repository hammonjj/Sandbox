using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "PrimaryOrbData", menuName = "Orbs/PrimaryOrbData")]
public class PrimaryOrbData : BaseOrbData
{
    public float CritChance = 0f;
    public float CritModifier = 1.50f;

    public bool CanPassThroughEnemies = false;

    private PlayerTracker _abilityTracker;

    public override void Initialize()
    {
        _abilityTracker = GameObject.FindGameObjectWithTag(
            Constants.Tags.GameStateManager).GetComponent<PlayerTracker>();

        GetUpgrades();
        EditorApplication.playModeStateChanged += OnPlayModeChange;
        EventManager.GetInstance().onPlayerDeath += OnPlayerDeath;
    }

    private void GetUpgrades()
    {
        var upgrades = _abilityTracker.GetCurrentUpgrades(Constants.Enums.UpgradeType.PrimaryAttack);
        foreach(var upgrade in upgrades)
        {
            var upgradeCast = (PrimaryWeaponUpgradeData)upgrade;
            switch(upgradeCast.UpgradeName)
            {
                case PrimaryWeaponUpgradeData.Upgrade.IncreaseCritChance:
                    CritChance += 10f;
                    break;
                case PrimaryWeaponUpgradeData.Upgrade.ProjectilePassThrough:
                    CanPassThroughEnemies = true;
                    break;
                case PrimaryWeaponUpgradeData.Upgrade.IncreaseOrbDamage:
                    AttackDamage += 2;
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
                int attackDamage = AttackDamage;
                if(CritChance >= Helper.RandomInclusiveRange(1, 100))
                {
                    attackDamage = (int)(attackDamage * CritModifier);
                    LogDebug($"Critical Strike -  Crit: {attackDamage} - Attack: {AttackDamage}");
                }

                baseEnemy.TakeDamage(attackDamage);
                destroy = !CanPassThroughEnemies;
            }
        }
        else if(other.tag == Constants.Tags.Player || other.tag == Constants.Tags.Projectile)
        {
            destroy = false;
        }

        return destroy;
    }

    protected override void OnPlayModeChange(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingPlayMode)
        {
            Helper.LogDebug("Reseting PrimaryOrbData");
            base.OnPlayModeChange(state);
            CritChance = 0f;
            CritModifier = 1.50f;
            CanPassThroughEnemies = false;
        }
    }

    private void OnPlayerDeath()
    {
        LogDebug("Player Died: Reseting PrimaryOrbData");
        CritChance = 0f;
        CritModifier = 1.50f;
        CanPassThroughEnemies = false;
    }
}
