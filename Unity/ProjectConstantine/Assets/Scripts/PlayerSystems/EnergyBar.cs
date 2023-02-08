using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviourBase
{
    [SerializeField] private Image _energyBar;
    [SerializeField] private float _timeToDrain = 0.25f;

    private float _target;

    private void Start()
    {
        _energyBar.fillAmount = 1f;
        EventManager.GetInstance().onEnergyUsed += OnEnergyUsed;
    }

    private void OnEnergyUsed(float energyPercent)
    {
        _target = energyPercent;
        StartCoroutine(DrainEnergyBar());
    }

    private IEnumerator DrainEnergyBar()
    {
        var elapsedTime = 0f;
        var fillAmount = _energyBar.fillAmount;
        while(elapsedTime < _timeToDrain)
        {
            elapsedTime += Time.deltaTime;

            _energyBar.fillAmount = Mathf.Lerp(fillAmount, _target, elapsedTime / _timeToDrain);
            yield return null;
        }
    }
}
