using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviourBase
{
    [Header("EnemyBase")]
    public int MaxHealth;
    public string Name;
    public float MovementSpeed;
    public bool Stop;

    //Detection Range

    [Header("Attack")]
    [Tooltip("In Degrees, with Zero being direclty in front")]
    public float AttackWidth;
    public float AttackRange;
    public float AttackSpeed;
    public float AttackDamage;

    private GameObject _player;
    private const string Player = "Player";

    private NavMeshAgent _navMeshAgent;

    private int _currentHealth;
    private HealthBar _healthBar;

    private void Awake()
    {
        MessageEnding = $"GameObject: {gameObject.name} - Name: {name}";

        _player = GameObject.FindGameObjectWithTag(Player);
        if(_player == null)
        {
            LogError($"Unable to find player");
        }

        _navMeshAgent = GetComponent<NavMeshAgent>();
        if(_navMeshAgent == null)
        {
            LogError($"Failed to get NavMeshAgent");
        }

        _currentHealth = MaxHealth;
        _healthBar = GetComponent<HealthBar>();
        if(_healthBar == null)
        {
            LogError($"Failed to get HealthBar");
        }

        SetupAi();
    }

    private void Update()
    {
        if(Stop)
        {
            return;
        }

        _navMeshAgent.SetDestination(_player.transform.position);

        if(_navMeshAgent.pathPending)
        {
        }

        //Attack if close enough
        if(Vector3.Distance(_player.transform.position, gameObject.transform.position) <= AttackRange)
        {
            Attack();
        }
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

        _healthBar.UpdateHealth((float)_currentHealth / MaxHealth);
    }

    private void Attack()
    {
        LogDebug("Attacking Player");
    }

    private void SetupAi()
    {
    }
}
