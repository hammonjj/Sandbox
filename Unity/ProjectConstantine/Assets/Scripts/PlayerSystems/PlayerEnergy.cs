using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEnergy : MonoBehaviourBase
{
    private int _currentEnergy;
    [SerializeField] private int _maxEnergy;
    [SerializeField] private float _rechargeRateTicks;
    [SerializeField] private float _rechargeRatePercent;

    private PlayerTracker _abilityTracker;
    
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventManager.GetInstance().onSceneEnding += OnSceneEnding;

        StartCoroutine(RechargeEnergy());
        _abilityTracker = VerifyComponent<PlayerTracker>(Constants.Tags.GameStateManager);
    }

    private void UseEnergy(int energyUsed)
    {
        LogDebug($"Using Energy: {energyUsed}");
        _currentEnergy -= energyUsed;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _maxEnergy = _abilityTracker.EnergyTracking.MaxEnergy;
        _rechargeRateTicks = _abilityTracker.EnergyTracking.RechargeRate;
    }

    private void OnSceneEnding()
    {
        StopAllCoroutines();
        _abilityTracker.EnergyTracking.MaxEnergy = _maxEnergy;
        _abilityTracker.EnergyTracking.RechargeRate = _rechargeRateTicks;
    }

    private IEnumerator RechargeEnergy()
    {
        while(true)
        {
            if(_currentEnergy >= _maxEnergy)
            {
                continue;
            }

            _currentEnergy = (int)(_currentEnergy *_rechargeRatePercent);
            if(_currentEnergy > _maxEnergy)
            {
                _currentEnergy = _maxEnergy;
            }

            yield return new WaitForSeconds(_rechargeRateTicks);
        }
    }
}
