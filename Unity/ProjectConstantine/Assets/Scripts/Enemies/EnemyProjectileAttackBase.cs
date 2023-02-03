using UnityEngine;

public class EnemyProjectileAttackBase : MonoBehaviourBase
{
    public EnemyProjectileBaseData AttackData;

    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
        LogDebug($"Starting: {_initialPosition}");
    }

    private void Update()
    {
        if(Vector3.Distance(_initialPosition, transform.position) > AttackData.ProjectileRange)
        {
            LogDebug($"Projectile passed Range - Destroying");
            Destroy(gameObject);
        }

        transform.Translate(Vector3.forward * Time.deltaTime * AttackData.MovementSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool destroyObject = true;
        if(other.tag == Constants.Tags.Player)
        {
            LogDebug("Hit Player");
            destroyObject = true;
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth?.TakeDamage(AttackData.ProjectileDamage);
        }
        else if(other.tag == Constants.Tags.Enemy || other.tag == Constants.Tags.Projectile)
        {
            //Pass on through
            destroyObject = false;
        }
        else
        {
            destroyObject = true;
        }

        if(destroyObject)
        {
            LogDebug($"Destroying Projectile: {transform.position} - Other Object: {other.gameObject.name}");
            Destroy(gameObject);
        }        
    }
}
