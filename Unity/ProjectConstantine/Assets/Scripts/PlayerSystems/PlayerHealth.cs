using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviourBase
{
    public Action onPlayerDeath;

    private int _currentHealth;
    private int _maxHealth = 100;

    private SceneStateManager _gameStateManager;
    private HealthBar _healthBar;

    private void Awake()
    {
        _healthBar = GetComponent<HealthBar>();
        _gameStateManager = GameObject.FindGameObjectWithTag(Constants.SceneStateManager).GetComponent<SceneStateManager>();
        //_gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
        onPlayerDeath += _gameStateManager.OnPlayerDeath;

        _currentHealth = _maxHealth;
        _healthBar.UpdateHealth((float)_currentHealth / _maxHealth);
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
