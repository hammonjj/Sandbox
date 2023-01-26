using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Orb : MonoBehaviourBase
{
    public bool HasBeenFired = false;
    public float ProjectileSpeed = 20f;
    public float AttackRange = 10f;
    public int AttackDamage = 10;

    private Vector3 _initialPosition;
    private Transform _playerTransform;

    private void Awake()
    {
        MessageEnding = gameObject.name;
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

        if(HasBeenFired &&
            Vector3.Distance(_initialPosition, transform.position) > AttackRange)
        {
            LogDebug($"Projectile passed AttackRange - Destroying - " +
                $"InitialPostion: {_initialPosition} - Transform Position: {transform.position}");

            Destroy(gameObject);
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * ProjectileSpeed);
        }
    }

    public void Fire()
    {
        HasBeenFired = true;

        //Can also pass this in if this becomes a problem
        _initialPosition = GameObject.FindGameObjectWithTag(Constants.Tags.OrbStartPos).transform.position;
        LogDebug($"Initial Position: {_initialPosition}");
    }

    private void OnTriggerEnter(Collider other)
    {
        //OrbDataObject.OnHit(other);
        if(other.tag == Constants.Tags.Enemy)
        {
            if(!HasBeenFired)
            {
                //LogDebug("Orb Hitting Enemy (Orbiting)");
            }
            else
            {
                LogDebug("Orb Hitting Enemy (Fired)");
                var baseEnemy = other.GetComponent<BaseEnemy>();
                if(baseEnemy == null)
                {
                    LogError($"Failed to get enemy base");
                    Debug.Break();
                }
                else
                {
                    baseEnemy.TakeDamage(AttackDamage);
                }
                
                Destroy(gameObject);
            }
        }

        //Need to deal with hitting walls and other objects
    }
}
