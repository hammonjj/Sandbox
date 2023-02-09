using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviourBase
{
    [Tooltip("Where on the mesh the player will shoot the enemy")]
    public GameObject AttackTarget;
    public GameObject FloatingCombatText;
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
    private bool _canAttack = true;
    private float _attackCooldownCurrent;

    public void AttackAnimationStarted()
    {
        LogDebug("AttackAnimationStarted");
    }

    public void AttackAnimationEnded()
    {
        LogDebug("AttackAnimationEnded");

        _isAttacking = false;
        _attackCooldownCurrent = EnemyObj.AttackCooldown;
        _animator?.SetBool(EnemyObj.GetAttackAnimationID(), false);
        _meleeWeaponHit.OnMeleeHit.RemoveListener(OnMeleeWeaponPlayerHit);
    }

    private void Awake()
    {
        MessageEnding = $"EnemyObj: {EnemyObj.name} - Name: {name}";
        SetupComponents();
    }

    private void Update()
    {
        _attackCooldownCurrent -= Time.deltaTime;
        _canAttack = _attackCooldownCurrent <= 0.0f;

        //Debugging
        if (DrawDebugLines)
        {
            DebugLines();
        }

        if(Stop)
        {
            return;
        }

        //Before the player has been found
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

        //After the player has been found
        if(_foundPlayer &&
            !_isAttacking &&
            Vector3.Distance(_player.transform.position, gameObject.transform.position) > EnemyObj.AttackRange)
        {
            _animator?.SetFloat(Constants.Animations.AnimID_Speed, 2f);
            _navMeshAgent.SetDestination(_player.transform.position);
        }

        if(!_isAttacking &&
            _canAttack &&
            Vector3.Distance(_player.transform.position, gameObject.transform.position) <= EnemyObj.AttackRange)
        {
            Attack();
            //_navMeshAgent.ResetPath();
        }
    }

    public void TakeDamage(int damage)
    {
        LogDebug($"I got hit - Current Health: {_currentHealth}");

        _currentHealth -= damage;
        LoadFloatingCombatText(damage);
        if(_currentHealth <= 0)
        {
            LogDebug("I Died");
            EventManager.GetInstance().OnEnemyDeath(1);
            Destroy(gameObject);
            return;
        }

        _healthBar.UpdateHealth((float)_currentHealth / EnemyObj.MaxHealth);
    }

    private void LoadFloatingCombatText(int damage)
    {
        var gObj = Instantiate(FloatingCombatText, transform.position, Quaternion.identity, transform);
        var combatTextObj = gObj.GetComponent<TextMeshPro>();
        combatTextObj.text = damage.ToString();
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
        LogDebug("Attacking Player");

        _isAttacking = true;
        _animator?.SetBool(EnemyObj.GetAttackAnimationID(), true);
        
        _meleeWeaponHit.OnMeleeHit.AddListener(OnMeleeWeaponPlayerHit);
    }

    private void SetupComponents()
    {
        _player = GameObject.FindGameObjectWithTag(Constants.Tags.Player);
        if(_player == null)
        {
            LogError($"Unable to find player");
        }

        _navMeshAgent = GetComponent<NavMeshAgent>();
        if(_navMeshAgent == null)
        {
            LogError($"Failed to get NavMeshAgent");
        }
        else
        {
            _navMeshAgent.speed = EnemyObj.MovementSpeed;
            _navMeshAgent.stoppingDistance = EnemyObj.AttackRange * .75f;
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
