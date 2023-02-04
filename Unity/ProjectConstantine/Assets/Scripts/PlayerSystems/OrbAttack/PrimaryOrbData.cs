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
    }

    private void GetUpgrades()
    {
        var upgrades = _abilityTracker.GetCurrentUpgrades(Constants.Enums.UpgradeType.PrimaryAttack);
        foreach(var upgrade in upgrades)
        {
            switch(upgrade.AttackUpgrade)
            {
                case Constants.Enums.AttackUpgrade.IncreaseCritChance:
                    CritChance += 10f;
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
}
