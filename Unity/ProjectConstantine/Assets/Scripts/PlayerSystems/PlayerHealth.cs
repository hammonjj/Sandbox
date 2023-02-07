public class PlayerHealth : MonoBehaviourBase
{
    private int _maxHealth;
    private int _currentHealth;

    private bool _playerDead = false;
    private HealthBar _healthBar;
    private EventManager _eventManager;
    private PlayerTracker _abilityTracker;

    private void Awake()
    {
        _eventManager = EventManager.GetInstance();
        _eventManager.onSceneEnding += SceneEnding;
    }

    private void Start()
    {
        _abilityTracker = VerifyComponent<PlayerTracker>(Constants.Tags.GameStateManager);
        _maxHealth = _abilityTracker.HealthTracker.MaxHealth;
        _currentHealth = _abilityTracker.HealthTracker.CurrentHealth;

        _healthBar = VerifyComponent<HealthBar>();
        _healthBar.UpdateHealth((float)_currentHealth / _maxHealth);
    }

    public void TakeDamage(int incomingDamage)
    {
        if(_playerDead)
        {
            return;
        }

        _currentHealth -= incomingDamage;
        LogDebug($"Updating Health: {_currentHealth} - MaxHealth: {_maxHealth}");

        if(_currentHealth <= 0)
        {
            _playerDead = true;
            _eventManager.onPlayerDeath();
            _eventManager.onSceneEnding -= SceneEnding;
        }

        _healthBar.UpdateHealth((float)_currentHealth / _maxHealth);
    }

    private void SceneEnding()
    {
        _abilityTracker.HealthTracker.MaxHealth = _maxHealth;
        _abilityTracker.HealthTracker.CurrentHealth = _currentHealth;
    }
}
