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
        _abilityBaseData = _playerTracker.SupportAbilityTracker.AbilityBaseData;

        EventManager.GetInstance().onSceneEnding += SceneEnding;
    }

    private void OnSupportAbilityUsed()
    {
        if(_abilityBaseData == null)
        {
            LogDebug("No support ability available");
            return;
        }

        if(_playerEnergy.CurrentEnergy < _abilityBaseData.EnergyRequired)
        {
            //Play VFX Clip
            LogDebug($"Not enough energy for {_abilityBaseData.Ability} - " +
                $"Required: {_abilityBaseData.EnergyRequired} - Current: {_playerEnergy.CurrentEnergy}");
            return;
        }

        _abilityBaseData.OnUse();
        _playerEnergy.UseEnergy(_abilityBaseData.EnergyRequired);
    }

    private void SceneEnding()
    {
        _playerTracker.SupportAbilityTracker.AbilityBaseData = _abilityBaseData;
    }
}
