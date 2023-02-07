using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEnergy : MonoBehaviourBase
{
    public int CurrentEnergy { private set; get; }

    [SerializeField] private int _maxEnergy;
    [SerializeField] private float _rechargeRateTicks;
    [SerializeField] private float _rechargeRatePercent;

    private IEnumerator rechargeEnergyCoroutine;
    private EventManager _eventManager;
    private PlayerTracker _abilityTracker;

    
    private void Start()
    {
        CurrentEnergy = 100;

        _abilityTracker = VerifyComponent<PlayerTracker>(Constants.Tags.GameStateManager);
        _maxEnergy = _abilityTracker.EnergyTracking.MaxEnergy;
        _rechargeRateTicks = _abilityTracker.EnergyTracking.RechargeRate;

        _eventManager = EventManager.GetInstance();
        _eventManager.onSceneEnding += OnSceneEnding;

        //rechargeEnergyCoroutine = RechargeEnergy();
        //StartCoroutine(rechargeEnergyCoroutine);
    }

    public void UseEnergy(int energyUsed)
    {
        CurrentEnergy -= energyUsed;
        UpdateEnergy();
    }

    private void OnSceneEnding()
    {
        //StopAllCoroutines();
        _abilityTracker.EnergyTracking.MaxEnergy = _maxEnergy;
        _abilityTracker.EnergyTracking.RechargeRate = _rechargeRateTicks;
    }

    private void UpdateEnergy()
    {
        _eventManager.OnEnergyUsed((float)CurrentEnergy / _maxEnergy);
    }

    private IEnumerator RechargeEnergy()
    {
        while(true)
        {
            if(CurrentEnergy >= _maxEnergy)
            {
                continue;
            }

            var tmpEnergy = CurrentEnergy;
            CurrentEnergy = (int)(CurrentEnergy * _rechargeRatePercent);
            if(CurrentEnergy > _maxEnergy)
            {
                CurrentEnergy = _maxEnergy;
            }

            LogDebug($"Energy Recharged - Previous: {tmpEnergy} - Current: {CurrentEnergy}");
            UpdateEnergy();
            yield return new WaitForSeconds(_rechargeRateTicks);
        }
    }
}
