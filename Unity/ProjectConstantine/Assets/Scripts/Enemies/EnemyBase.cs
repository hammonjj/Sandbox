using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviourBase
{
    [Header("EnemyBase")]
    public int Health;
    public string Name;
    public float MovementSpeed;
    
    //TODO: Draw Gizmo Around Attack Range
    [Header("Attack")]
    [Tooltip("In Degrees, with Zero being direclty in front")]
    public float AttackWidth;
    public float AttackRange;
    public float AttackSpeed;
    public float AttackDamage;

    private GameObject _player;
    private const string Player = "Player";

    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        MessageEnding = $"GameObject: {gameObject.name} - Name: {name}";

        _player = GameObject.FindGameObjectWithTag(Player);
        if(_player == null)
        {
            LogError($"Unable to find player");
        }

        _navMeshAgent = GetComponent<NavMeshAgent>();
        if(_navMeshAgent)
        {
            LogError($"Failed to get NavMeshAgent");
        }

        SetuoAi();
    }

    private void Update()
    {
        _navMeshAgent.SetDestination(_player.transform.position);

        //If the path is still be computed, animate taunt
        if(_navMeshAgent.pathPending)
        {
        }

        //Attack if close enough
        if(Vector3.Distance(_player.transform.position, gameObject.transform.position) <= AttackRange)
        {
            Attack();
        }
    }

    private void Attack()
    {
        LogDebug("Attacking Player");
    }

    private void SetuoAi()
    {
    }
}
