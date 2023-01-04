using UnityEngine;

[CreateAssetMenu(fileName = "MeleeEnemyBase", menuName = "Enemy/MeleeEnemyBase")]
public class EnemyBaseObj : ScriptableObject
{
    [Header("Base")]
    public string Name;
    public int MaxHealth;

    [Header("Movement")]
    public float MovementSpeed;
    public float DetectionRange;

    [Header("Attack")]
    [Tooltip("In Degrees, with Zero being direclty in front")]
    public float AttackWidth;
    public float AttackRange;
    public float AttackSpeed;
    public float AttackCooldown;
    public int AttackDamage;

    public virtual void OnAttack() { }
    public virtual int GetAttackAnimationID() { return 0; }
}
