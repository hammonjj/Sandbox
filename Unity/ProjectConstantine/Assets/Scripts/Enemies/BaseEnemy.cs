using UnityEngine;

public class BaseEnemy : MonoBehaviourBase
{
    [Header("Debugging")]
    public bool DrawDebugLines;

    [Header("Enemy Base")]
    public BaseEnemyData EnemyData;
    public GameObject FloatingCombatText;

    private bool _foundPlayer = false;
    private bool _canAttack = true;
    private int _currentHealth;
    private float _attackCooldownCurrent;

    private HealthBar _healthBar;
    private GameObject _player;

    private void Start()
    {
        _healthBar = transform.Find(Constants.ObjectNames.EnemyHealthBarCanvas).GetComponent<HealthBar>();
        _currentHealth = EnemyData.MaxHealth;
        _player = GameObject.FindGameObjectWithTag(Constants.Tags.Player);
        
        EnemyData.Setup(gameObject);
    }

    private void Update()
    {
        _attackCooldownCurrent -= Time.deltaTime;
        _canAttack = _attackCooldownCurrent <= 0.0f;

        if(DrawDebugLines)
        {
            DebugLines();
        }

        if(!_foundPlayer)
        {
            EnemyData.Idle();
        }

        if(!_foundPlayer &&
            Vector3.Distance(_player.transform.position, gameObject.transform.position) <= EnemyData.DetectionRange)
        {
            _foundPlayer = true;
            EnemyData.PlayerFound();
        }

        if(_foundPlayer)
        {
            EnemyData.Move();
        }

        if(_foundPlayer &&
            _canAttack &&
            Vector3.Distance(_player.transform.position, gameObject.transform.position) <= EnemyData.AttackRange)
        {
            EnemyData.Attack();
            _attackCooldownCurrent = EnemyData.AttackCooldown;
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        LogDebug($"I got hit - Attack Damage: {damage} - Current Health: {_currentHealth}");

        ShowFloatingCombatText(damage);
        if(_currentHealth <= 0)
        {
            LogDebug("I Died");
            EnemyData.Death();
            EventManager.GetInstance().OnEnemyDeath();
            Destroy(gameObject);
            return;
        }

        _healthBar.UpdateHealth((float)_currentHealth / EnemyData.MaxHealth);
    }

    private void ShowFloatingCombatText(int damage)
    {
        var gObj = Instantiate(FloatingCombatText, transform.position, Quaternion.identity, transform);
        gObj.GetComponent<FloatingCombatText>().SetDamage(damage);
    }

    private void DebugLines()
    {
        var rotation = gameObject.transform.rotation;
        rotation *= Quaternion.Euler(90, 0, 0);

        EnemyData.DebugLines(rotation);

        //Attack Range
        Debug.DrawCircle(
            gameObject.transform.position,
            rotation,
            EnemyData.AttackRange,
            Color.red);

        if(_foundPlayer)
        {
            return;
        }

        //Detection Range
        Debug.DrawCircle(
            gameObject.transform.position,
            rotation,
            EnemyData.DetectionRange,
            Color.yellow);
    }
}
