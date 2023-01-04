using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Action onPlayerDeath;

    [SerializeField]
    private PlayerHealthObj _playerHealth;

    private GameStateManager _gameStateManager;
    private HealthBar _healthBar;

    private void Awake()
    {
        _healthBar = GetComponent<HealthBar>();
        _gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
        onPlayerDeath += _gameStateManager.OnPlayerDeath;
    }

    private void Update()
    {
        if(_playerHealth.CurrentHealth <= 0)
        {
            //Invoke Death Animation
            onPlayerDeath?.Invoke();
        }

        _healthBar.UpdateHealth((float)_playerHealth.CurrentHealth / _playerHealth.MaxHealth);   
    }

    
    public void TakeDamage(int incomingDamage)
    {
        _playerHealth.CurrentHealth -= incomingDamage;

        if(_playerHealth.CurrentHealth <= 0)
        {
            //Invoke Death Animation
            onPlayerDeath?.Invoke();
        }

        _healthBar.UpdateHealth((float)_playerHealth.CurrentHealth / _playerHealth.MaxHealth);
    }
    
}
