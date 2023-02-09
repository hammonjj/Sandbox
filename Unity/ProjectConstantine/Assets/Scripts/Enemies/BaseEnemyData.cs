using System;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "BaseEnemyData", menuName = "Enemy/BaseEnemyData")]
public class BaseEnemyData : ScriptableObjectBase
{
    [Header("Base")]
    public string Name;
    public int MaxHealth = 100;
    public int Currency = 1;

    [Header("Movement")]
    public float MovementSpeed = 2f;
    public float DetectionRange = 5f;
    public float PatrolPathRange = 1f;

    [Header("Attack")]
    public float AttackRange = 5f;
    public float AttackCooldown = 1.5f;

    //Base Enemy resets cooldown on this
    public Action onAttackEnded;
    public Action onDeath;

    //Virtual Methods
    public virtual void Setup(GameObject parentGameObject) { }

    public virtual void Idle(GameObject parentGameObject)
    {
        RandomPatrol(parentGameObject);
    }

    public virtual void PlayerFound() { }
    public virtual void UpdateDataObj(GameObject parentGameObject) { }
    public virtual void Attack(GameObject parentGameObject, Transform firingPosition) { }
    public virtual void Move(GameObject parentGameObject) { }
    public virtual void Death() { }

    public virtual void DebugLines(Quaternion rotation, GameObject parentGameObject) { }

    protected void RandomPatrol(GameObject parentGameObject)
    {
        //Simple random pathing while searching for player
        var navMeshAgent = parentGameObject.GetComponent<NavMeshAgent>();
        if(navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            return;
        }

        var patrolPoint = RandomPoint(parentGameObject.transform.position, PatrolPathRange);
        if(patrolPoint != Vector3.zero)
        {
            navMeshAgent.SetDestination(patrolPoint);
        }
    }

    private Vector3 RandomPoint(Vector3 center, float range)
    {
        NavMeshHit hit;
        Vector3 result = Vector3.zero;
        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
        if(NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) 
        {
            //Documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
            result = hit.position;
        }

        return result;
    }
}
