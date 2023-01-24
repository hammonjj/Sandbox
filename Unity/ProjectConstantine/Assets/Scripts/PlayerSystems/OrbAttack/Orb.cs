using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviourBase
{
    public bool HasBeenFired = false;
    public float ProjectileSpeed = 20f;
    public float AttackRange = 10f;

    private Vector3 _InitialPosition;
    private Transform _playerTransform;
    private void Awake()
    {
        _InitialPosition = gameObject.transform.position;
    }

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag(Constants.Tags.Player)
            .GetComponent<Transform>();
    }

    private void Update()
    {
        if(!HasBeenFired)
        {
            //Keep the rotation pointed in the same direction of the player
            transform.rotation = _playerTransform.rotation;

            return;
        }

        if(Vector3.Distance(_InitialPosition, gameObject.transform.position) > AttackRange)
        {
            LogDebug("Projectile passed AttackRange - Destroying");
            Destroy(gameObject);
        }

        //Update Position
        transform.Translate(Vector3.forward * Time.deltaTime * ProjectileSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == Constants.Tags.Enemy)
        {
            if(!HasBeenFired)
            {
                LogDebug("Orb Hitting Enemy (Orbiting)");
            }
            else
            {
                LogDebug("Orb Hitting Enemy (Fired)");
                var enemyBase = other.GetComponent<EnemyBase>();
                if(enemyBase == null)
                {
                    //Need to develop logger for non-monobehavior object
                    //LogError($"Failed to get enemy base");
                }

                //enemyBase.TakeDamage(AttackDamage);
                Destroy(gameObject);
            }
        }
    }
}
