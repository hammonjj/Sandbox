using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHealth = 100;
    public Action onPlayerDeath;

    private int _currentHealth;
    private GameStateManager _gameStateManager;
    private HealthBar _healthBar;

    private void Awake()
    {
        _currentHealth = MaxHealth;

        _healthBar = GetComponent<HealthBar>();
        _gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
        onPlayerDeath += _gameStateManager.OnPlayerDeath;
    }

    public void TakeDamage(int incomingDamage)
    {
        _currentHealth -= incomingDamage;

        if(_currentHealth <= 0)
        {
            //Invoke Death Animation
            onPlayerDeath.Invoke();
        }

        _healthBar.UpdateHealth((float)_currentHealth / MaxHealth);
    }
}
