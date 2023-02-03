using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemData", menuName = "ShopItems/ShopItemData")]
public class ShopItemData : ScriptableObjectBase
{
    public string Name;
    public string Description;
    public int Cost;
    //Add ShopImage

    public Constants.Enums.UpgradeType UpgradeType;
    public Constants.Enums.AttackUpgrade AttackUpgrade;
}
