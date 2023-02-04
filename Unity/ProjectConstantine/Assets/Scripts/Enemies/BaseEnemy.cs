using UnityEngine;

public class BaseEnemy : MonoBehaviourBase
{
    [Header("Debugging")]
    public bool DrawDebugLines;

    [Header("Enemy Base")]
    public BaseEnemyData EnemyData;
    public GameObject FloatingCombatText;
    public Transform FiringPosition;
    public bool EmitDeathEvent = true;

    private bool _resetAttack;
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
        EnemyData.onAttackEnded += OnAttackEnded;
        EnemyData.onDeath += OnDeath;
    }

    private void Update()
    {
        if(_resetAttack)
        {
            _attackCooldownCurrent -= Time.deltaTime;
        }

        _canAttack = _attackCooldownCurrent <= 0.0f;

        if(DrawDebugLines)
        {
            DebugLines();
        }

        if(!_foundPlayer)
        {
            EnemyData.Idle(gameObject);
        }

        if(!_foundPlayer &&
            Vector3.Distance(_player.transform.position, gameObject.transform.position) <= EnemyData.DetectionRange)
        {
            _foundPlayer = true;
            EnemyData.PlayerFound();
        }

        if(_foundPlayer)
        {
            EnemyData.Move(gameObject);
            EnemyData.UpdateDataObj(gameObject);
        }

        if(_foundPlayer &&
            _canAttack &&
            Vector3.Distance(_player.transform.position, gameObject.transform.position) <= EnemyData.AttackRange)
        {
            _resetAttack = false;
            _attackCooldownCurrent = EnemyData.AttackCooldown;
            EnemyData.Attack(gameObject, FiringPosition);
        }
    }

    private void OnAttackEnded()
    {
        _resetAttack = true;
    }

    private void OnDeath()
    {
        if(EmitDeathEvent)
        {
            EventManager.GetInstance().OnEnemyDeath();
        }

        LogDebug("I Died");
        EnemyData.Death();
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        LogDebug($"I got hit - Attack Damage: {damage} - Current Health: {_currentHealth}");

        ShowFloatingCombatText(damage);
        if(_currentHealth <= 0)
        {
            OnDeath();
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
        if(!Application.isEditor)
        {
            return;
        }

        var rotation = gameObject.transform.rotation;
        rotation *= Quaternion.Euler(90, 0, 0);

        EnemyData.DebugLines(rotation, gameObject);

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
