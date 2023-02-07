using UnityEngine;

[CreateAssetMenu(fileName = "PrimaryWeaponUpgradeData", menuName = "ShopItems/PrimaryWeaponUpgradeData")]
public class PrimaryWeaponUpgradeData : UpgradeData
{
    public enum Upgrade
    {
        IncreaseCritChance,
        IncreaseOrbDamage,
        ProjectilePassThrough,
    }

    public Upgrade UpgradeName;
}
