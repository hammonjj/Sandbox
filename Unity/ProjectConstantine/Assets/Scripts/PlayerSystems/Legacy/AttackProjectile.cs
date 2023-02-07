using UnityEngine;

[CreateAssetMenu(fileName = "BaseProjectile", menuName = "Projectile/Base")]
public class AttackProjectile : ScriptableObject
{
    public int AttackDamage = 5;
    public float AttackSpeed = 20f;
    public float AttackRange = 10f;
    
    public virtual void OnProjectileHit(GameObject objectHit, GameObject parent) { }
}
