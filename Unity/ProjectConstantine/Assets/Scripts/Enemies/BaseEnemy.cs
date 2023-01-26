using UnityEngine;

//RangedEnemy
public class BaseEnemy : MonoBehaviourBase
{
    [Header("Debugging")]
    //public bool Stop;
    public bool DrawDebugLines;

    [Header("Enemy Base")]
    [Tooltip("Where on the mesh the player will shoot the enemy")]
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
        

        EnemyData.Setup(gameObject); //-> Virtual function to setup things like contraints for turrets
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
            //EnemyData.Idle -> Before the player is found
        }

        if(!_foundPlayer &&
            Vector3.Distance(_player.transform.position, gameObject.transform.position) <= EnemyData.DetectionRange)
        {
            _foundPlayer = true;
            EnemyData.PlayerFound(); //-> Used for animations or initial actions after finding the player
        }

        if(_foundPlayer)
        {
            //transform.LookAt(_playerBodyAttackTarget.transform);
            EnemyData.Move(); //-> Used to determine what the enemy should do if we have found the player
            //  ex. Chase player, not move but rotate to face, etc.
        }

        if(_foundPlayer &&
            _canAttack &&
            Vector3.Distance(_player.transform.position, gameObject.transform.position) <= EnemyData.AttackRange)
        {
            //Attack();
            EnemyData.Attack();
            _attackCooldownCurrent = EnemyData.AttackCooldown;
        }
    }

    /*
    private void Attack() //-> Will need to be a virtually overridden function in derived classes or a method in the enemydata
    {
        LogDebug("Attacking Player");
        var enemyProjectile = Instantiate(
            EnemyData.ProjectileAttackData.ProjectilePrefab,
            _firingPosition.position,
            _firingPosition.rotation);

        enemyProjectile.name = "ProjectileAttack";
    }
    */
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
