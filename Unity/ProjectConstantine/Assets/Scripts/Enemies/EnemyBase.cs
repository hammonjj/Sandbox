using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviourBase
{
    public GameObject AttackTarget;
    public EnemyBaseObj EnemyObj;
    
    [Header("Debugging")]
    public bool Stop;
    public bool DrawDebugLines;

    private GameObject _player;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private int _currentHealth;
    private HealthBar _healthBar;

    private bool _foundPlayer = false;

    private void Awake()
    {
        MessageEnding = $"EnemyObj: {EnemyObj.name} - Name: {name}";

        _player = GameObject.FindGameObjectWithTag(PlayerConstants.Player);
        if(_player == null)
        {
            LogError($"Unable to find player");
        }

        _navMeshAgent = GetComponent<NavMeshAgent>();
        if(_navMeshAgent == null)
        {
            LogError($"Failed to get NavMeshAgent");
        }

        _currentHealth = EnemyObj.MaxHealth;
        _healthBar = GetComponent<HealthBar>();
        if(_healthBar == null)
        {
            LogError($"Failed to get HealthBar");
        }

        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            LogError($"Failed to get Animator");
        }

        SetupAi();
    }

    private void Update()
    {
        if(DrawDebugLines)
        {
            DebugLines();
        }

        if(Stop)
        {
            return;
        }

        if(!_foundPlayer && 
            Vector3.Distance(_player.transform.position, gameObject.transform.position) <= EnemyObj.DetectionRange)
        {
            _foundPlayer = true;
        }

        //Hang out if we haven't found the player
        if(!_foundPlayer)
        {
            return;
        }

        if(_foundPlayer)
        {
            _navMeshAgent.SetDestination(_player.transform.position);
        }

        //Attack if close enough
        if(Vector3.Distance(_player.transform.position, gameObject.transform.position) <= EnemyObj.AttackRange)
        {
            Attack();
            _animator?.SetFloat(PlayerConstants.AnimID_Speed, 0f);
            return;
        }
        else
        {
            _animator?.SetBool(PlayerConstants.AnimID_MutantAttack, false);
        }

        _animator?.SetFloat(PlayerConstants.AnimID_Speed, 2f);
    }

    public void TakeDamage(int damage)
    {
        LogDebug($"I got hit - Current Health: {_currentHealth}");

        _currentHealth -= damage;
        if(_currentHealth <= 0)
        {
            LogDebug("I Died");
            //Play death animation and despawn
            Destroy(gameObject);
            return;
        }

        _healthBar.UpdateHealth((float)_currentHealth / EnemyObj.MaxHealth);
    }

    private void Attack()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return;
        }

        LogDebug("Attacking Player");
        _animator?.SetBool(EnemyObj.GetAttackAnimationID(), true);
    }

    private void SetupAi()
    {
        _navMeshAgent.speed = EnemyObj.MovementSpeed;
        //_navMeshAgent.stoppingDistance = EnemyObj.AttackRange > 1 ? EnemyObj.AttackRange - 1 : 1;
    }

    private void DebugLines()
    {
        var rotation = gameObject.transform.rotation;
        rotation *= Quaternion.Euler(90, 0, 0);

        //Attack Range
        Debug.DrawArc(
            90 - (EnemyObj.AttackWidth / 2),
            90 + (EnemyObj.AttackWidth / 2),
            gameObject.transform.position,
            rotation,
            EnemyObj.AttackRange,
            Color.red,
            false,
            true);

        if(_foundPlayer)
        {
            return;
        }

        //Detection Range
        Debug.DrawCircle(
            gameObject.transform.position,
            rotation,
            EnemyObj.DetectionRange,
            Color.yellow);
    }
}
