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
    [Tooltip("In Degrees, with Zero being direclt in front")]
    public float AttackWidth;
    public float AttackRange;
    public float AttackSpeed;
    public float AttackDamage;

    private GameObject _player;
    private const string Player = "Player";

    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(Player);
        if(_player == null)
        {
            LogError($"Unable to find player - GameObject: {gameObject.name} - Name: {name}");
        }

        _navMeshAgent = GetComponent<NavMeshAgent>();
        if(_navMeshAgent)
        {
            LogError($"Failed to get NavMeshAgent - GameObject: {gameObject.name} - Name: {name}");
        }

        SetAiPreferences();
    }

    private void Update()
    {
        _navMeshAgent.SetDestination(_player.transform.position);

        //If the path is still be computed, animate taunt
        if(_navMeshAgent.pathPending)
        {
        }

        //Attack if close enough
    }

    private void SetAiPreferences()
    {
    }
}
