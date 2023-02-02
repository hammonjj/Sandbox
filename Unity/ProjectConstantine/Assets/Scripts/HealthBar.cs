using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviourBase
{
    private Image _healthBar;

    //For draining the healthbar
    //  - https://www.youtube.com/watch?v=-UbElCzKwuA
    private void Start()
    {
        _healthBar = transform.Find(Constants.ObjectNames.Health)?.gameObject?.GetComponent<Image>();
        if(_healthBar == null)
        {
            _healthBar = GameObject.FindGameObjectWithTag(Constants.Tags.PlayerHeathBar).GetComponent<Image>();
        }
    }

    public void UpdateHealth(float fraction)
    {
        if(_healthBar == null)
        {
            _healthBar = transform.Find(Constants.ObjectNames.Health)?.gameObject?.GetComponent<Image>();
            if(_healthBar == null)
            {
                _healthBar = GameObject.FindGameObjectWithTag(Constants.Tags.PlayerHeathBar).GetComponent<Image>();
            }
        }

        _healthBar.fillAmount = fraction;
    }
}
