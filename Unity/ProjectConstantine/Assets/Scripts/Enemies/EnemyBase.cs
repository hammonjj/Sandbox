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
    private MeleeWeaponHit _meleeWeaponHit;
    private Rigidbody _rigidBody;

    private int _currentHealth;
    private HealthBar _healthBar;

    private bool _foundPlayer = false;
    private bool _isAttacking = false;
    
    public void AttackAnimationStarted()
    {
        LogDebug("AttackAnimationStarted");

        _isAttacking = true;
    }

    public void AttackAnimationEnded()
    {
        LogDebug("AttackAnimationEnded");

        _isAttacking = false;
        _animator?.SetBool(EnemyObj.GetAttackAnimationID(), false);
        _meleeWeaponHit.OnMeleeHit.RemoveListener(OnMeleeWeaponPlayerHit);
    }

    private void Awake()
    {
        MessageEnding = $"EnemyObj: {EnemyObj.name} - Name: {name}";
        SetupComponents();
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

        if(!_foundPlayer)
        {
            //Idle or path around looking for player
            return;
        }

        if(_foundPlayer &&
            Vector3.Distance(_player.transform.position, gameObject.transform.position) > EnemyObj.AttackRange)
        {
            _animator?.SetFloat(PlayerConstants.AnimID_Speed, 2f);
            _animator?.SetBool(EnemyObj.GetAttackAnimationID(), false);
            _navMeshAgent.SetDestination(_player.transform.position);
        }

        //Attack if close enough
        if(Vector3.Distance(_player.transform.position, gameObject.transform.position) <= EnemyObj.AttackRange)
        {
            Attack();
            return;
        }

        _animator?.SetFloat(
            PlayerConstants.AnimID_Speed,
            _rigidBody.velocity == Vector3.zero ? 0f : 2f);
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

    private void OnMeleeWeaponPlayerHit(Collider other)
    {
        LogDebug("OnMeleeWeaponPlayerHit");
        _meleeWeaponHit.OnMeleeHit.RemoveListener(OnMeleeWeaponPlayerHit);

        //Might get this earlier if there is a performance impact
        var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(EnemyObj.AttackDamage);
    }

    private void Attack()
    {
        if (_isAttacking)
        {
            return;
        }

        LogDebug("Attacking Player");
        _isAttacking = true;
        _animator?.SetBool(EnemyObj.GetAttackAnimationID(), true);
        _meleeWeaponHit.OnMeleeHit.AddListener(OnMeleeWeaponPlayerHit);
    }

    private void SetupAi()
    {
        _navMeshAgent.speed = EnemyObj.MovementSpeed;
        _navMeshAgent.stoppingDistance = EnemyObj.AttackRange * .75f;
    }

    private void SetupComponents()
    {
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

        _meleeWeaponHit = GetComponentInChildren<MeleeWeaponHit>();
        if(_meleeWeaponHit == null)
        {
            LogError($"Failed to get Melee Weapon Hit");
        }

        _rigidBody = GetComponent<Rigidbody>();
        if(_rigidBody == null)
        {
            LogError("Failed to get Rigid Body");
        }
    }

    private void DebugLines()
    {
        var rotation = gameObject.transform.rotation;
        rotation *= Quaternion.Euler(90, 0, 0);

        //Attack Stop Range
        Debug.DrawCircle(
            gameObject.transform.position,
            rotation,
            EnemyObj.AttackRange * .75f,
            Color.blue);

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
