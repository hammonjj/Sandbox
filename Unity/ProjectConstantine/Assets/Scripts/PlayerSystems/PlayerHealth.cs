using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviourBase
{
    public Action onPlayerDeath;

    private int _currentHealth;
    private int _maxHealth = 100;

    private SceneStateManager _sceneStateManager;
    private HealthBar _healthBar;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _healthBar = GetComponent<HealthBar>();
        _healthBar.UpdateHealth((float)_currentHealth / _maxHealth);
    }

    private void Update()
    {
        if(_sceneStateManager == null)
        {
            _sceneStateManager = GameObject.FindGameObjectWithTag(Constants.SceneStateManager)?.GetComponent<SceneStateManager>();
            if(_sceneStateManager == null)
            {
                return;
            }

            LogDebug("Acquired Scene Manager");
            onPlayerDeath += _sceneStateManager.OnPlayerDeath;
        }
    }
    public void TakeDamage(int incomingDamage)
    {
        LogDebug("Updating Health");
        _currentHealth -= incomingDamage;

        if(_currentHealth <= 0)
        {
            //Invoke Death Animation
            onPlayerDeath?.Invoke();
        }

        _healthBar.UpdateHealth((float)_currentHealth / _maxHealth);
    }
    
}
