using UnityEngine;

[CreateAssetMenu(fileName = "PersonalForceFieldData", menuName = "SupportAbilities/PersonalForceFieldData")]
public class PersonalForceFieldAbilityData : SupportAbilityBaseData
{
    public GameObject ForceFieldPrefab;

    public override void OnUse()
    {
        LogDebug("Initializing Forcefield");

        var player = GameObject.FindGameObjectWithTag(Constants.Tags.Player);
        Instantiate(ForceFieldPrefab, player.transform);
    }
}
