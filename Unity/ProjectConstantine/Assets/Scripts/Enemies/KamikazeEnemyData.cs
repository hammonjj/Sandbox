using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "KamikazeEnemyData", menuName = "Enemy/KamikazeEnemyData")]
public class KamikazeEnemyData : BaseEnemyData
{
    [Header("Kamikaze")]
    public int ExplosionDamage = 30;
    public float WindupTimeBeforeSprint = 1.5f;
    public float TimeBeforeDetonation = 1.5f;
    public float SprintSpeed = 30f;
    public float ExplosionRadius = 5f;
    public float ExplosionProximity = 3f;

    private bool _isSprinting;
    private bool _preparingAttack;
    private float _currentExplosionTimer = 0f;
    private float _currentWindupTime;
    private GameObject _parentGameObject;
    private GameObject _playerBodyAttackTarget;
    private NavMeshAgent _navMeshAgent;

    private Transform _currentAttackTransform;

    public override void Setup(GameObject parentGameObject) 
    {
        _parentGameObject = parentGameObject;
        _navMeshAgent = _parentGameObject.GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = MovementSpeed;

        _playerBodyAttackTarget = GameObject.FindGameObjectWithTag(Constants.Tags.PlayerBodyAttackTarget);
    }

    public override void Idle() { }
    public override void PlayerFound() { }
    public override void Attack() 
    {
        if(ExplosionTimerStarted())
        {
            Helper.LogDebug("Explosion timer started - do nothing");
            return;
        }

        Helper.LogDebug("Initializing Attack");
        _preparingAttack = true;
        _currentWindupTime = 0f;
        _navMeshAgent.isStopped = true;
        _currentAttackTransform = _playerBodyAttackTarget.transform;
    }
    
    public override void Move() 
    {
        if(ExplosionTimerStarted())
        {
            if(_currentExplosionTimer <= 0)
            {
                Explode();
                return;
            }

            Helper.LogDebug("Explosion timer started - Update timer - Stop all actions");
            _currentExplosionTimer -= Time.deltaTime;
            return;
        }

        if(_preparingAttack)
        {
            _currentWindupTime += Time.deltaTime;
        }

        if(_preparingAttack && _currentWindupTime >= WindupTimeBeforeSprint)
        {
            Helper.LogDebug("Beginnning Sprint");
            _isSprinting = true;
            _preparingAttack = false;
            _navMeshAgent.speed = SprintSpeed;
            _navMeshAgent.SetDestination(_currentAttackTransform.position);
        }

        if(_isSprinting && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            Helper.LogDebug("Finished Sprint");
            _isSprinting = false;
            _navMeshAgent.isStopped = false; //Reset agent so we move again if we don't explode
            _navMeshAgent.speed = MovementSpeed; //Reset speed so we don't sprint again on move

            if(Vector3.Distance(
                _parentGameObject.transform.position, _playerBodyAttackTarget.transform.position) < ExplosionProximity)
            {
                Helper.LogDebug("Player in explosion proximity");
                Explode();
            }

            return;
        }

        //If the player gets too close (while not attaking), explode after time
        if(!_isSprinting &&
            !_preparingAttack &&
            Vector3.Distance(
                _parentGameObject.transform.position, _playerBodyAttackTarget.transform.position) < ExplosionProximity)
        {
            Helper.LogDebug("Player in explosion proximity");
            _navMeshAgent.isStopped = true;
            _currentExplosionTimer = TimeBeforeDetonation;
            return;
        }

        //Move towards player
        if(!_isSprinting &&
            !_preparingAttack && 
            Vector3.Distance(
                _parentGameObject.transform.position, _playerBodyAttackTarget.transform.position) > AttackRange)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_playerBodyAttackTarget.transform.position);
        }
    }

    public override void Death() 
    {
        Explode();
    }

    public override void DebugLines(Quaternion rotation) 
    {
        Debug.DrawCircle(
            _parentGameObject.transform.position,
            rotation,
            ExplosionRadius,
            Color.magenta);

        Debug.DrawCircle(
            _parentGameObject.transform.position,
            rotation,
            ExplosionProximity,
            Color.magenta);
    }

    private bool ExplosionTimerStarted()
    {
        return _currentExplosionTimer > 0;
    }

    private void Explode()
    {
        Helper.LogDebug("Exploding");
        //Explode
        // - Create Overlap Sphere
        // - Detect if player is in sphere
        // - Do damage
        // - Destroy myself
        //Destroy(_parentGameObject);
    }
}
