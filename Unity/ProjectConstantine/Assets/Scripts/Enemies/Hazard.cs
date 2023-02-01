using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviourBase
{
    public int Damage = 30;
    public float TimeBeforeDamage = 0.5f;

    private float _currentDamageTimer = -1f;
    private List<GameObject> _gameObjectsInDanger = new();

    private void Update()
    {
        _currentDamageTimer -= Time.deltaTime;
        if(_currentDamageTimer > 0f)
        {
            if(_currentDamageTimer <= 0)
            {
                LogDebug("Timer Expired: Doing Damage");
                DoDamage();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        LogDebug("");
        _currentDamageTimer = TimeBeforeDamage;

        var containsObject = false;
        foreach(var gObj in _gameObjectsInDanger)
        {
            if(GameObject.ReferenceEquals(collision.gameObject, gObj))
            {
                containsObject = true;
                break;
            }
        }

        if(!containsObject)
        {
            _gameObjectsInDanger.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        LogDebug("");
        _gameObjectsInDanger.RemoveAll(x => GameObject.ReferenceEquals(x.gameObject, collision.gameObject));
    }

    private void DoDamage()
    {
        foreach(var gObj in _gameObjectsInDanger)
        {
            if(gObj.tag == Constants.Tags.Player)
            {
                var health = gObj.GetComponent<PlayerHealth>();
                health.TakeDamage(Damage);
            }
            else if(gObj.tag == Constants.Tags.Enemy)
            {
                var health = gObj.GetComponent<EnemyBase>();
                health.TakeDamage(Damage);
            }
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        LogDebug("");
        _currentDamageTimer = TimeBeforeDamage;

        var containsObject = false;
        foreach(var gObj in _gameObjectsInDanger)
        {
            if(GameObject.ReferenceEquals(collision.gameObject, gObj))
            {
                containsObject = true;
                break;
            }
        }

        if(!containsObject)
        {
            _gameObjectsInDanger.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        LogDebug("");
        _gameObjectsInDanger.RemoveAll(x => GameObject.ReferenceEquals(x.gameObject, collision.gameObject));
    }
}
