using UnityEngine;

public class EnemyProjectileAttackBase : MonoBehaviourBase
{
    public EnemyProjectileBaseData AttackData;

    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
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
        if(other.tag == Constants.Tags.Player)
        {
            //HurtPlayer
            LogDebug("Hit Player");
        }

        Destroy(gameObject);
    }
}
