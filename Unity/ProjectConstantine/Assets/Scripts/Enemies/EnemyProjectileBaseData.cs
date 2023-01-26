using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProjectileBaseData", menuName = "Enemy/EnemyProjectileBaseData")]
public class EnemyProjectileBaseData : ScriptableObject
{
    public float MovementSpeed = 5f;
    public float ProjectileDamage = 10f;
    public float ProjectileRange = 10;
    public GameObject ProjectilePrefab;
}
