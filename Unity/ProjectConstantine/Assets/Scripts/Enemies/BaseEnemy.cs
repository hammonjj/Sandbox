using UnityEngine;

public class BaseEnemy : MonoBehaviourBase
{
    [Header("Debugging")]
    //public bool Stop;
    public bool DrawDebugLines;

    [Header("Enemy Base")]
    public BaseEnemyData EnemyData;

    private bool _foundPlayer = false;
    private bool _canAttack = true;
    private int _currentHealth;
    private float _attackCooldownCurrent;

    private GameObject _player;
    private GameObject _playerBodyAttackTarget;
    private Transform _firingPosition;

    private void Start()
    {
        _currentHealth = EnemyData.MaxHealth;
        _firingPosition = transform.Find("FiringPosition");
        _player = GameObject.FindGameObjectWithTag(Constants.Tags.Player);
        _playerBodyAttackTarget = GameObject.FindGameObjectWithTag(Constants.Tags.PlayerBodyAttackTarget);
        

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

        //LoadFloatingCombatText(damage);
        if(_currentHealth <= 0)
        {
            LogDebug("I Died");
            EventManager.GetInstance().OnEnemyDeath();
            Destroy(gameObject);
            return;
        }

        //_healthBar.UpdateHealth((float)_currentHealth / EnemyObj.MaxHealth);
    }

    private void DebugLines()
    {
        EnemyData.DebugLines();
        var rotation = gameObject.transform.rotation;
        rotation *= Quaternion.Euler(90, 0, 0);
        /*
        //Attack Stop Range
        Debug.DrawCircle(
            gameObject.transform.position,
            rotation,
            EnemyObj.AttackRange * .75f,
            Color.blue);
        */
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
