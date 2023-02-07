using UnityEngine;

public class AttackProjectileBase : MonoBehaviourBase
{
    public AttackProjectile AttackProjectile;

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
    }
}
