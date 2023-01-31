using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemyData", menuName = "Enemy/BaseEnemyData")]
public class BaseEnemyData : ScriptableObjectBase
{
    [Header("Base")]
    public string Name;
    public int MaxHealth = 100;

    [Header("Movement")]
    public float MovementSpeed = 2f;
    public float DetectionRange = 5f;

    [Header("Attack")]
    public float AttackRange = 5f;
    public float AttackCooldown = 1.5f;

    //Base Enemy resets cooldown on this
    public Action onAttackEnded;
    public Action onDeath;

    //Virtual Methods
    public virtual void Setup(GameObject parentGameObject) { }
    public virtual void Idle() { }
    public virtual void PlayerFound() { }
    public virtual void Update() { }
    public virtual void Attack() { }
    public virtual void Move() { }
    public virtual void Death() { }

    public virtual void DebugLines(Quaternion rotation) { }
}
