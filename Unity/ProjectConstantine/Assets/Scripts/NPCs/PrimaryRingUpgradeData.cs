using UnityEngine;

[CreateAssetMenu(fileName = "PrimaryRingUpgradeData", menuName = "ShopItems/PrimaryRingUpgradeData")]
public class PrimaryRingUpgradeData : UpgradeData
{
    public enum Upgrade
    {
        IncreaseOrbs,
    }

    public Upgrade UpgradeName;
}
