using UnityEngine;

[CreateAssetMenu(fileName = "DualPistolProjectile", menuName = "Projectile/DualPistol")]
public class DualPistolProjectile : AttackProjectile
{
    public override void OnProjectileHit(GameObject objectHit, GameObject parent) 
    {
        if(objectHit.tag == "Enemy")
        {
            //Do damage to enemy
            var enemyBase = objectHit.GetComponent<EnemyBase>();
            if(enemyBase == null)
            {
                //Need to develop logger for non-monobehavior object
                //LogError($"Failed to get enemy base");
            }

            enemyBase.TakeDamage(AttackDamage);
        }

        if(parent != null)
        {
            Destroy(parent);
        }
    }
}
