using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveOrbData", menuName = "Orbs/ExplosiveOrbData")]
public class ExplosiveOrbData : BaseOrbData
{
    [Header("Explosive Orb")]
    public int ExplosionDamage = 15;
    public float ExplosionRadius = 2f;

    public override void OnMaxRangePassed(Vector3 currPos)
    {
        Explode(currPos);
    }

    public override bool OnHit(Collider other, bool hasBeenFired)
    {
        if(!hasBeenFired)
        {
            return false;
        }

        Explode(other.transform.position);
        return true;
    }

    private void Explode(Vector3 pos)
    {
        //Add explosion visual effect
        LogDebug("Exploding");
        var collidersHit = Physics.OverlapSphere(pos, ExplosionRadius);
        foreach(var collider in collidersHit)
        {
            if(collider.tag != Constants.Tags.Enemy)
            {
                continue;
            }

            var enemyHealth = collider.gameObject.GetComponent<EnemyBase>();
            enemyHealth.TakeDamage(ExplosionDamage);

            LogDebug($"Did {ExplosionDamage} to enemy {collider.name}");
        }
    }
}
