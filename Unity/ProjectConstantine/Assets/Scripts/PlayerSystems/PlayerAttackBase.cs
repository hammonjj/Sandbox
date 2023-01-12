using System.Collections;
using UnityEngine;

public class PlayerAttackBase : MonoBehaviourBase
{
    [Tooltip("Time required to pass before being able to attack again")]
    public float AttackCooldown = 0.50f;

    [Tooltip("Maximum range the projectile will go before destroying itself")]
    public float AttackRange = 5f;

    [Tooltip("How wide, in radian, the soft aim lock will target")]
    public float SoftAimLockWidth = 0.5f; //0 = 180 degree arc - 0.5 = 90 degree arc

    [Tooltip("Animation constraint weight when running with a weapon")]
    public float RunAnimationConstraintWeight = 0.115f;

    [Tooltip("Animation constraint weight when shooting a weapon")]
    public float ShootAnimationConstraintWeight = 1f;

    [Tooltip("Amount of time it takes for the weapon to return to it's original position after firing")]
    public float AimAnimationReturnTime = 0.33f;

    [Tooltip("Where the player's attacks spawn from")]
    public Transform[] AttackSpawnPoints;

    [Tooltip("Prefab for the attack projectile")]
    public GameObject AttackProjectile;

    protected bool _canAttack;
    protected float _attackCooldownCurrent;

    protected virtual void OnAttack()
    {
    }

    protected void UpdateAttackCooldown()
    {
        _attackCooldownCurrent -= Time.deltaTime;
        _canAttack = _attackCooldownCurrent <= 0.0f;
    }

    protected IEnumerator CoroutineWaiter()
    {
        var totalWaitTime = 1f;
        var waitTimeDuration = 0f;
        while(waitTimeDuration < totalWaitTime)
        {
            waitTimeDuration += Time.deltaTime;
            yield return null;
        }
    }

    protected (Vector3, Quaternion) FindEnemiesToAttack()
    {
        var attackSpawnPoint = AttackSpawnPoints[0];
        RotateAttackSpawnPoints();

        var projectileRotation = attackSpawnPoint.rotation;

        Vector3 retVector = Vector3.zero;

        if(this == null)
        {
            return (new Vector3(0, 0, 0), Quaternion.identity);
        }

        //Watch for performance issues - might need to put enemies on their own layer
        var collidersHit = Physics.OverlapSphere(gameObject.transform.position, AttackRange);
        if(collidersHit.Length > 0)
        {
            foreach(var colliderHit in collidersHit)
            {
                if(colliderHit.gameObject.tag != "Enemy")
                {
                    continue;
                }

                var normalizedColliderVector = colliderHit.transform.position - attackSpawnPoint.position;
                normalizedColliderVector.Normalize();

                //0 = 180 degree arc - 0.5 = 90 degree arc
                if(Vector3.Dot(normalizedColliderVector, attackSpawnPoint.forward) > SoftAimLockWidth)
                {
                    LogDebug("Enemy Detected in Range");

                    var attackTarget = colliderHit.gameObject.GetComponent<EnemyBase>().AttackTarget;
                    retVector = attackTarget.transform.position;
                    projectileRotation = Quaternion.LookRotation(normalizedColliderVector, Vector3.up);
                    break;
                }
            }
        }

        return (retVector, projectileRotation);
    }

    private void RotateAttackSpawnPoints()
    {
        if(AttackSpawnPoints.Length == 2)
        {
            var spawnPointZero = AttackSpawnPoints[0];
            var spawnPointOne = AttackSpawnPoints[1];

            AttackSpawnPoints[0] = spawnPointOne;
            AttackSpawnPoints[1] = spawnPointZero;
        }
        else if(AttackSpawnPoints.Length > 2)
        {
            LogError("AttackSpawnPoints > 2 not implemented!");
        }
    }
}
