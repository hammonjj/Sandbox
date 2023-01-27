using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemyData", menuName = "Enemy/BaseEnemyData")]
public class BaseEnemyData : ScriptableObject
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

    //Virtual Methods
    public virtual void Setup(GameObject parentGameObject) { }
    public virtual void Idle() { }
    public virtual void PlayerFound() { }
    public virtual void Attack() { }
    public virtual void Move() { }
    public virtual void Death() { }

    public virtual void DebugLines(Quaternion roation) { }
}
