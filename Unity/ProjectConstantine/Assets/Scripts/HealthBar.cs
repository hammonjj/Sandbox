using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviourBase
{
    private Image _healthBar;

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
        _healthBar.fillAmount = fraction;
    }
}
