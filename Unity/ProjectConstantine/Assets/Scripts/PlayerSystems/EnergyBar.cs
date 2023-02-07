using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviourBase
{
    [SerializeField] private Image _energyBar;

    private void Start()
    {
        _energyBar.fillAmount = 1f;
        EventManager.GetInstance().onEnergyUsed += OnEnergyUsed;
    }

    private void OnEnergyUsed(float energyPercent)
    {
        _energyBar.fillAmount = energyPercent;
    }
}
