using UnityEngine;

[RequireComponent(typeof(EnemyFollow))]
public class EnemyAi : MonoBehaviourBase
{
    [Header("Enemy Ai")]
    public float AttackRange = 5.0f;
    public float ChaseRange = 10.0f;
    
    private Vector2 _startingPos;
    private Vector2 _roamingPos;
    private EnemyFollow _enemyFollow;
    private Transform _playerTransform;

    //Get component for enemy attacking - hide behind interface 
    //IEnemyAttackAnims enemyAttackAnims;

    private enum EnemyState
    {
        Roaming,
        ChaseTarget,
        Attacking
    }

    private EnemyState _currentState;

    private void Awake()
    {
        //Get pathfinding
        _currentState = EnemyState.Roaming;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        _enemyFollow = GetComponent<EnemyFollow>();
        _enemyFollow.TransformToFollow = _playerTransform;
        //enemyAttackAnims = GetComponent<IEnemyAttackAnims>();
    }

    private void Start()
    {
        _startingPos = transform.position;
        _roamingPos = _GetRoamingPosition();
    }

    private void Update()
    {
        switch(_currentState)
        {
            case EnemyState.Roaming:
                _Roam();
                _FindTarget();
                break;
            case EnemyState.ChaseTarget:
                _ChasePlayer();
                _AttackPlayer();
                _CheckChaseTether();
                break;
            case EnemyState.Attacking:
                _CheckChaseTether();
                break;
        }   
    }

    private void _CheckChaseTether()
    {
        if(Vector2.Distance(transform.position, _playerTransform.position) > ChaseRange)
        {
            LogDebug("Player out of chase range");
            _enemyFollow.EnableFollow(false);
            _roamingPos = _GetRoamingPosition();
            _currentState = EnemyState.Roaming;
        }
    }

    private void _AttackPlayer()
    {
        if(Vector2.Distance(transform.position, _playerTransform.position) < AttackRange)
        {
            LogDebug("Player in attack range");
            _currentState = EnemyState.Attacking;
        }
    }

    private void _Roam()
    {
        //Use pathfinding to get to our new roaming position

        //Once we are there, get a new roaming position
        var reachedPositionDistance = 1.0f;
        if(Vector2.Distance(transform.position, _roamingPos) < reachedPositionDistance)
        {
            _roamingPos = _GetRoamingPosition();
        }
    }

    private void _ChasePlayer()
    {
        //Use pathfinding to chase after the player
    }

    private Vector2 _GetRoamingPosition()
    {
        var randomDirection = new Vector2(Random.Range(-1f, 1f), transform.position.y).normalized;
        return _startingPos + randomDirection * Random.Range(10f, 50f);
    }

    private void _FindTarget()
    {
        if(Vector2.Distance(transform.position, _playerTransform.position) < ChaseRange)
        {
            LogDebug("Player in chase range");
            _enemyFollow.EnableFollow(true);
            _currentState = EnemyState.ChaseTarget;
        }
    }
}
