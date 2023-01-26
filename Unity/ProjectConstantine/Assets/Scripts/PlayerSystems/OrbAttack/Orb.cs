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
        MessageEnding = $"Orb-{Guid.NewGuid()}";
        _playerTransform = GameObject.FindGameObjectWithTag(Constants.Tags.Player)
           .GetComponent<Transform>();

        //Can also pass this in if this becomes a problem
        _initialPosition = GameObject.FindGameObjectWithTag(Constants.Tags.OrbStartPos).transform.position;
    }

    private void Update()
    {
        if(!HasBeenFired)
        {
            //Keep the rotation pointed in the same direction of the player
            transform.rotation = _playerTransform.rotation;
            return;
        }

        //Need to check if our parent transform is gone before checking distance otherwise we pull the local position
        if(transform.parent == null &&
            //transform.position != transform.localPosition &&
            Vector3.Distance(_initialPosition, transform.position) > AttackRange)
        {
            //BUG: This is sometimes pulling local position on transform.positin instead of world position
            LogDebug($"Projectile passed AttackRange - Destroying - " +
                $"InitialPostion: {_initialPosition} - Transform Position: {transform.position} - TransformLocalPosition: {transform.localPosition}");
            
            Destroy(gameObject);
        }

        //Update Position
        transform.Translate(Vector3.forward * Time.deltaTime * ProjectileSpeed);
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
