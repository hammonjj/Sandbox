using UnityEngine;

[CreateAssetMenu(fileName = "PersonalForceFieldData", menuName = "SupportAbilities/PersonalForceFieldData")]
public class PersonalForceFieldAbilityData : SupportAbilityBaseData
{
    public GameObject ForceFieldPrefab;

    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(Constants.Tags.Player);    
    }

    public override void OnUse()
    {
        LogDebug("Initializing Forcefield");
        Instantiate(ForceFieldPrefab, _player.transform);
    }
}
