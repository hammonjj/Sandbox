using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ShopItems/UpgradeData")]
public class UpgradeData : ScriptableObjectBase
{
    public string Title;
    public string Description;
    public int Cost;
    public Guid UpgradeId;
    public string Id;
    //Add ShopImage

    public Constants.Enums.UpgradeType UpgradeType;
}
