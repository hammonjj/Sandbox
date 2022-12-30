using UnityEngine;

public class AttackProjectileBase : MonoBehaviourBase
{
    public AttackProjectile AttackProjectile;
    /*
    [Header("AttackBase")]
    public float AttackSpeed = 20f;
    public float AttackRange = 10f;
    public int AttackDamage = 5;
    */
    private Vector3 _InitialPosition;

    private void Awake()
    {
        _InitialPosition = gameObject.transform.position;    
    }

    void Update()
    {
        if(Vector3.Distance(_InitialPosition, gameObject.transform.position) > AttackProjectile.AttackRange)
        {
            LogDebug("Projectile passed AttackRange - Destroying");
            Destroy(gameObject);
        }

        //Update Position
        transform.Translate(Vector3.forward * Time.deltaTime * AttackProjectile.AttackSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        AttackProjectile.OnProjectileHit(other.gameObject, gameObject);
        /*
        if(other.gameObject.tag != "Enemy")
        {
            LogDebug($"Attack Hit Object - {other.gameObject.name}");
            Destroy(gameObject);
            return;
        } 
        else if(other.gameObject.tag == "Enemy")
        {
            LogDebug("Attack Hit Enemy");

            //Do damage to enemy
            var enemyBase = other.gameObject.GetComponent<EnemyBase>();
            if(enemyBase == null)
            {
                LogError($"Failed to get enemy base");
            }

            enemyBase.TakeDamage(AttackDamage);
            Destroy(gameObject);
            return;
        }
        */
    }
}
