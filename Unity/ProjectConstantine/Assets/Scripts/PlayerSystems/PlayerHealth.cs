public class PlayerHealth : MonoBehaviourBase
{
    private int _maxHealth;
    private int _currentHealth;

    private HealthBar _healthBar;
    private EventManager _eventManager;

    private void Awake()
    {
        _eventManager = EventManager.GetInstance();
        _eventManager.onSceneEnding += SceneEnding;
    }

    private void Start()
    {
        var abilityTracker = VerifyComponent<AbilityTracker>(Constants.Tags.GameStateManager);
        _maxHealth = abilityTracker.PlayerHealthTracker.MaxHealth;
        _currentHealth = abilityTracker.PlayerHealthTracker.CurrentHealth;

        _healthBar = VerifyComponent<HealthBar>();
        _healthBar.UpdateHealth((float)_currentHealth / _maxHealth);
    }

    public void TakeDamage(int incomingDamage)
    {
        _currentHealth -= incomingDamage;
        LogDebug($"Updating Health: {_currentHealth} - MaxHealth: {_maxHealth}");

        if(_currentHealth <= 0)
        {
            //Invoke Death Animation
            _eventManager.onPlayerDeath();
        }

        _healthBar.UpdateHealth((float)_currentHealth / _maxHealth);
    }

    private void SceneEnding()
    {
        var abilityTracker = VerifyComponent<AbilityTracker>(Constants.Tags.GameStateManager);
        abilityTracker.PlayerHealthTracker.CurrentHealth = _currentHealth;
        abilityTracker.PlayerHealthTracker.MaxHealth = _maxHealth;
    }
}
