using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Action onPlayerDeath;

    private int _currentHealth;
    private int _maxHealth = 100;

    private GameStateManager _gameStateManager;
    private HealthBar _healthBar;

    private void Awake()
    {
        _healthBar = GetComponent<HealthBar>();
        _gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
        onPlayerDeath += _gameStateManager.OnPlayerDeath;

        _currentHealth = _maxHealth;
        _healthBar.UpdateHealth((float)_currentHealth / _maxHealth);
    }

    private void Update()
    {
        /*
        if(_playerHealth.CurrentHealth <= 0)
        {
            //Invoke Death Animation
            onPlayerDeath?.Invoke();
        }

        _healthBar.UpdateHealth((float)_playerHealth.CurrentHealth / _playerHealth.MaxHealth); 
        */
    }

    
    public void TakeDamage(int incomingDamage)
    {
        _currentHealth -= incomingDamage;

        if(_currentHealth <= 0)
        {
            //Invoke Death Animation
            onPlayerDeath?.Invoke();
        }

        _healthBar.UpdateHealth((float)_currentHealth / _maxHealth);
    }
    
}
