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
    public float DistAboveGround = 1.5f;

    private bool _isSprinting;
    private bool _preparingAttack;
    private float _currentWindupTime;
    private GameObject _playerBodyAttackTarget;
    private Vector3 _currentAttackPosition;

    public override void Setup(GameObject parentGameObject) 
    {
        _isSprinting = false;
        _preparingAttack = false;
        parentGameObject.GetComponent<NavMeshAgent>().speed = MovementSpeed;

        _playerBodyAttackTarget = GameObject.FindGameObjectWithTag(Constants.Tags.PlayerBodyAttackTarget);
    }

    public override void PlayerFound() { }

    public override void UpdateDataObj(GameObject parentGameObject)
    {
        if(_preparingAttack)
        {
            _currentWindupTime += Time.deltaTime;
        }
    }

    public override void Attack(GameObject parentGameObject, Transform firingPosition) 
    {
        if(_preparingAttack || _isSprinting)
        {
            return;
        }

        LogDebug("Initializing Attack");
        _preparingAttack = true;
        _currentWindupTime = 0f;
        parentGameObject.GetComponent<NavMeshAgent>().ResetPath();
        _currentAttackPosition = _playerBodyAttackTarget.transform.position;
    }
    
    public override void Move(GameObject parentGameObject) 
    {
        var navMeshAgent = parentGameObject.GetComponent<NavMeshAgent>();
        if(_preparingAttack && _currentWindupTime >= WindupTimeBeforeSprint)
        {
            Helper.LogDebug("Beginning Sprint");
            _isSprinting = true;
            _preparingAttack = false;
            navMeshAgent.speed = SprintSpeed;
            navMeshAgent.acceleration = 100f;
            navMeshAgent.SetDestination(_currentAttackPosition);
        }

        if(_isSprinting &&
            Helper.HorizontalDistance(
                parentGameObject.transform.position, _currentAttackPosition) <= ExplosionProximity)
        {
            Helper.LogDebug("Finished Sprint");

            _isSprinting = false;
            navMeshAgent.acceleration = 8f;
            navMeshAgent.speed = MovementSpeed;
            navMeshAgent.velocity = Vector3.zero;

            if(parentGameObject != null &&
                Helper.HorizontalDistance(
                    parentGameObject.transform.position, _playerBodyAttackTarget.transform.position) < ExplosionProximity)
            {
                Helper.LogDebug("Player in explosion proximity");
                navMeshAgent.isStopped = true;
                Explode(parentGameObject);
                return;
            }

            onAttackEnded?.Invoke();
            _currentWindupTime = 0f;
            navMeshAgent.ResetPath();
        }
        
        //Move towards player
        if(!_isSprinting &&
            !_preparingAttack &&
            Vector3.Distance(
                parentGameObject.transform.position, _playerBodyAttackTarget.transform.position) > AttackRange)
        {
            navMeshAgent.SetDestination(_playerBodyAttackTarget.transform.position);
        }
    }

    public override void Death() { }

    public override void DebugLines(Quaternion rotation, GameObject parentGameObject) 
    {
        Debug.DrawCircle(
            parentGameObject.transform.position,
            rotation,
            ExplosionRadius,
            Color.magenta);

        Debug.DrawCircle(
            parentGameObject.transform.position,
            rotation,
            ExplosionProximity,
            Color.magenta);
    }

    private void Explode(GameObject parentGameObject)
    {
        LogDebug("Exploding");
        var collidersHit = Physics.OverlapSphere(parentGameObject.transform.position, ExplosionRadius);
        foreach(var collider in collidersHit)
        {
            if(collider.gameObject.tag != Constants.Tags.Player)
            {
                continue;
            }

            var playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(ExplosionDamage);

            LogDebug($"Did {ExplosionDamage} to player");
        }

        //Add explosion visual effect
        onDeath?.Invoke();
    }
}
