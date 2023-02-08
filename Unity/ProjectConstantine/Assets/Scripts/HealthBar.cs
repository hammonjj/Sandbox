using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviourBase
{
    [SerializeField] private float _timeToDrain = 0.15f;
    private float _target;

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
        if(_healthBar == null)
        {
            _healthBar = transform.Find(Constants.ObjectNames.Health)?.gameObject?.GetComponent<Image>();
            if(_healthBar == null)
            {
                _healthBar = GameObject.FindGameObjectWithTag(Constants.Tags.PlayerHeathBar).GetComponent<Image>();
            }
        }

        _target = fraction;
        StartCoroutine(DrainHealthBar());
    }

    private IEnumerator DrainHealthBar()
    {
        var elapsedTime = 0f;
        var fillAmount = _healthBar.fillAmount;
        while(elapsedTime < _timeToDrain)
        {
            elapsedTime += Time.deltaTime;

            _healthBar.fillAmount = Mathf.Lerp(fillAmount, _target, elapsedTime / _timeToDrain);
            yield return null;
        }
    }
}
