using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProjectileBaseData", menuName = "Enemy/EnemyProjectileBaseData")]
public class EnemyProjectileBaseData : ScriptableObject
{
    public float MovementSpeed = 5f;
    public int ProjectileDamage = 10;
    public float ProjectileRange = 10;
    public GameObject ProjectilePrefab;
}
