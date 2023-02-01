using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviourBase
{
    private int _currentHealth;
    private int _maxHealth = 100;

    private EventManager _eventManager;
    private HealthBar _healthBar;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _eventManager = EventManager.GetInstance();
    }

    private void Start()
    {
        _healthBar = VerifyComponent<HealthBar>();
        //_healthBar = GetComponent<HealthBar>();
        _healthBar.UpdateHealth((float)_currentHealth / _maxHealth);
    }

    public void TakeDamage(int incomingDamage)
    {
        LogDebug("Updating Health");
        _currentHealth -= incomingDamage;

        if(_currentHealth <= 0)
        {
            //Invoke Death Animation
            _eventManager.onPlayerDeath();
        }

        _healthBar.UpdateHealth((float)_currentHealth / _maxHealth);
    }
}
