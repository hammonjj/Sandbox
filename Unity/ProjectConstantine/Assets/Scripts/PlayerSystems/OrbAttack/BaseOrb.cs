using UnityEngine;

public class BaseOrb : MonoBehaviourBase
{
    public BaseOrbData BaseOrbData;

    private bool _hasBeenFired = false;
    private Vector3 _initialPosition;
    private Transform _playerTransform;

    private void Start()
    {
        MessageEnding = gameObject.name;
        _playerTransform = GameObject.FindGameObjectWithTag(Constants.Tags.Player)
           .GetComponent<Transform>();

        BaseOrbData.Initialize();
    }

    private void Update()
    {
        if(!_hasBeenFired)
        {
            //Keep the rotation pointed in the same direction of the player
            transform.rotation = _playerTransform.rotation;
            return;
        }

        if(_hasBeenFired &&
            Vector3.Distance(_initialPosition, transform.position) > BaseOrbData.AttackRange)
        {
            LogDebug($"Projectile passed AttackRange - Destroying - " +
                $"InitialPostion: {_initialPosition} - Transform Position: {transform.position}");

            BaseOrbData.OnMaxRangePassed(transform.position);
            Destroy(gameObject);
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * BaseOrbData.ProjectileSpeed);
        }
    }

    public void Fire()
    {
        _hasBeenFired = true;

        //Can also pass this in if this becomes a problem
        _initialPosition = GameObject.FindGameObjectWithTag(Constants.Tags.OrbStartPos).transform.position;
        LogDebug($"Initial Position: {_initialPosition}");
    }

    private void OnTriggerEnter(Collider other)
    {
        var destroyObject = BaseOrbData.OnHit(other, _hasBeenFired);
        if(destroyObject)
        {
            Destroy(gameObject);
        }
    }
}
