using UnityEngine;

[CreateAssetMenu(fileName = "SupportAbilityBaseData", menuName = "SupportAbilities/SupportAbilityBaseData")]
public class SupportAbilityBaseData : ScriptableObjectBase
{
    public int EnergyRequired;
    public Constants.Enums.SupportAbilities Ability;

    public virtual void OnUse() { }
}
