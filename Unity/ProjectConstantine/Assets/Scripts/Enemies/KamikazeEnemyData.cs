using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "KamikazeEnemyData", menuName = "Enemy/KamikazeEnemyData")]
public class KamikazeEnemyData : BaseEnemyData
{
    [Header("Kamikaze")]
    public int ExplosionDamage = 30;
    public float WindupTimeBeforeSprint = 1.5f;
    public float SprintSpeed = 20f;
    public float ExplosionRadius = 5f;
    public float ExplosionProximity = 3f;

    private bool _isExploding = false;
    private bool _isSprinting;
    private bool _preparingAttack;
    private float _currentWindupTime;
    private NavMeshAgent _navMeshAgent;
    private GameObject _parentGameObject;
    private GameObject _playerBodyAttackTarget;
    private Vector3 _currentAttackPosition;

    public override void Setup(GameObject parentGameObject) 
    {
        _isSprinting = false;
        _preparingAttack = false;
        _parentGameObject = parentGameObject;
        _navMeshAgent = _parentGameObject.GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = MovementSpeed;

        _playerBodyAttackTarget = GameObject.FindGameObjectWithTag(Constants.Tags.PlayerBodyAttackTarget);
    }

    public override void Idle() { }
    public override void PlayerFound() { }

    public override void Update()
    {
        if(_isExploding)
        {
            //_parentGameObject.transform.localScale *= 1.1f;
        }

        if(_preparingAttack)
        {
            _currentWindupTime += Time.deltaTime;
        }
    }

    public override void Attack() 
    {
        if(_preparingAttack || _isSprinting || _isExploding)
        {
            return;
        }

        Helper.LogDebug("Initializing Attack");
        _preparingAttack = true;
        _currentWindupTime = 0f;
        _navMeshAgent.ResetPath();
        _currentAttackPosition = _playerBodyAttackTarget.transform.position;
        Helper.LogDebug($"1. CurrentAttackPosition: {_currentAttackPosition}");
    }
    
    public override void Move() 
    {
        if(_isExploding)
        {
            _navMeshAgent.isStopped = true;
            _isSprinting = false;
            _preparingAttack = false;
            return;
        }

        if(_preparingAttack && _currentWindupTime >= WindupTimeBeforeSprint)
        {
            Helper.LogDebug("Beginning Sprint");
            _isSprinting = true;
            _preparingAttack = false;
            _navMeshAgent.speed = SprintSpeed;
            _navMeshAgent.acceleration = 100f;
            _navMeshAgent.SetDestination(_currentAttackPosition);
        }

        if(_isSprinting && 
            Helper.HorizontalDistance(
                _parentGameObject.transform.position, _currentAttackPosition) <= ExplosionProximity)
        {
            Helper.LogDebug("Finished Sprint");

            _isSprinting = false;
            _navMeshAgent.acceleration = 8f;
            _navMeshAgent.speed = MovementSpeed;
            _navMeshAgent.velocity = Vector3.zero;

            if(Helper.HorizontalDistance(
                _parentGameObject.transform.position, _playerBodyAttackTarget.transform.position) < ExplosionProximity)
            {
                Helper.LogDebug("Player in explosion proximity");
                _navMeshAgent.isStopped = true;
                Explode();
                return;
            }

            onAttackEnded?.Invoke();
            _currentWindupTime = 0f;
            _navMeshAgent.ResetPath();
        }
        
        //Move towards player
        if(!_isSprinting &&
            !_preparingAttack && 
            Vector3.Distance(
                _parentGameObject.transform.position, _playerBodyAttackTarget.transform.position) > AttackRange)
        {
            _navMeshAgent.SetDestination(_playerBodyAttackTarget.transform.position);
        }
    }

    public override void Death() { }

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

    private void Explode()
    {
        Helper.LogDebug("Exploding");
        var collidersHit = Physics.OverlapSphere(_parentGameObject.transform.position, ExplosionRadius);
        foreach(var collider in collidersHit)
        {
            if(collider.gameObject.tag != Constants.Tags.Player)
            {
                continue;
            }

            var playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(ExplosionDamage);

            Helper.LogDebug($"Did {ExplosionDamage} to player");
        }

        //Add explosion visual effect
        _isExploding = true;
        Destroy(_parentGameObject, 0.2f);
    }
}
