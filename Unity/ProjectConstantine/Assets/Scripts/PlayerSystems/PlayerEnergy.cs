using UnityEngine;

public class PlayerEnergy : MonoBehaviourBase
{
    public int CurrentEnergy { private set; get; }

    [SerializeField] private int _maxEnergy;
    [SerializeField] private float _rechargeRateTicks;
    [SerializeField] private float _rechargeRatePercent;

    private EventManager _eventManager;
    private PlayerTracker _abilityTracker;

    
    private void Start()
    {
        CurrentEnergy = 100;

        _abilityTracker = VerifyComponent<PlayerTracker>(Constants.Tags.GameStateManager);
        _maxEnergy = _abilityTracker.EnergyTracking.MaxEnergy;
        _rechargeRateTicks = _abilityTracker.EnergyTracking.RechargeRate == 0 ?
            _rechargeRateTicks : _abilityTracker.EnergyTracking.RechargeRate;

        _eventManager = EventManager.GetInstance();
        _eventManager.onSceneEnding += OnSceneEnding;
        InvokeRepeating(nameof(RechargeEnergy), 1.0f, _rechargeRateTicks);
    }

    public void UseEnergy(int energyUsed)
    {
        CurrentEnergy -= energyUsed;
        UpdateEnergy();
    }

    private void OnSceneEnding()
    {
        _abilityTracker.EnergyTracking.MaxEnergy = _maxEnergy;
        _abilityTracker.EnergyTracking.RechargeRate = _rechargeRateTicks;
    }

    private void UpdateEnergy()
    {
        _eventManager.OnEnergyUsed((float)CurrentEnergy / _maxEnergy);
    }

    private void RechargeEnergy()
    {
            if(CurrentEnergy >= _maxEnergy)
            {
                return;
            }

            CurrentEnergy += (int)(_maxEnergy * _rechargeRatePercent);
            if(CurrentEnergy > _maxEnergy)
            {
                CurrentEnergy = _maxEnergy;
            }

            UpdateEnergy();
    }
}
