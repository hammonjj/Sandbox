using UnityEngine;

public class SupportAbilityHandler : MonoBehaviourBase
{
    [SerializeField] private SupportAbilityBaseData _abilityBaseData;
    
    private PlayerEnergy _playerEnergy;
    private PlayerTracker _playerTracker;

    private void Start()
    {
        EventManager.GetInstance().onSupportAbility += OnSupportAbilityUsed;
        _playerTracker = VerifyComponent<PlayerTracker>(Constants.Tags.GameStateManager);

        _playerEnergy = VerifyComponent<PlayerEnergy>();
    }

    private void OnSupportAbilityUsed()
    {
        if(_playerEnergy.CurrentEnergy < _abilityBaseData.EnergyRequired)
        {
            //Play VFX Clip
            LogDebug($"Not enough energy for {_abilityBaseData.Ability.ToString()} - " +
                $"Required: {_abilityBaseData.EnergyRequired} - Current: {_playerEnergy.CurrentEnergy}");
            return;
        }

        _abilityBaseData.OnUse();
        _playerEnergy.UseEnergy(_abilityBaseData.EnergyRequired);
    }
}
