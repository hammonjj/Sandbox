using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviourBase
{
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; } = 100;

    private EventManager _eventManager;
    private HealthBar _healthBar;

    private void Awake()
    {
        //Get this from the GameStateManager
        CurrentHealth = MaxHealth;
        _eventManager = EventManager.GetInstance();
        _eventManager.onSceneEnding += SceneEnding;
    }

    private void Start()
    {
        var gameStateManager = VerifyComponent<GameStateManager>(Constants.Tags.GameStateManager);
        MaxHealth = gameStateManager.CurrentMaxHealth;
        CurrentHealth = gameStateManager.CurrentHealth == 0 ? 100 : gameStateManager.CurrentHealth;

        _healthBar = VerifyComponent<HealthBar>();
        _healthBar.UpdateHealth((float)CurrentHealth / MaxHealth);
    }

    public void TakeDamage(int incomingDamage)
    {
        CurrentHealth -= incomingDamage;
        LogDebug($"Updating Health: {CurrentHealth} - MaxHealth: {MaxHealth}");

        if(CurrentHealth <= 0)
        {
            //Invoke Death Animation
            _eventManager.onPlayerDeath();
        }

        _healthBar.UpdateHealth((float)CurrentHealth / MaxHealth);
    }

    public void SceneEnding()
    {
        var gameStateManager = VerifyComponent<GameStateManager>(Constants.Tags.GameStateManager);
        gameStateManager.CurrentHealth = CurrentHealth;
        gameStateManager.CurrentMaxHealth = MaxHealth;
    }
}
