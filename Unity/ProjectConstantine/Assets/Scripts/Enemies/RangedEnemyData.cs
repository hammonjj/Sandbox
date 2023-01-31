using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "RangedEnemyData", menuName = "Enemy/RangedEnemyData")]
public class RangedEnemyData : BaseEnemyData
{
    [Header("Ranged")]
    public EnemyProjectileBaseData ProjectileAttackData;
    public float DistanceToRun = 3.0f;
    public float DistanceToMaintain = 2.0f;
    public float RotationSpeed = 120f;

    private bool _preparingAttack;
    private Transform _firingPosition;
    private GameObject _parentGameObject;
    private GameObject _playerBodyAttackTarget;
    private NavMeshAgent _navMeshAgent;

    private Quaternion _attackEnemyRotation;
    private Transform _currentShotTarget;

    public override void Setup(GameObject parentGameObject)
    {
        _parentGameObject = parentGameObject;
        
        _firingPosition = GameObjectExtensions.RecursiveFindChild(
            _parentGameObject.transform, Constants.ObjectNames.FiringPosition);
        _playerBodyAttackTarget = GameObject.FindGameObjectWithTag(Constants.Tags.PlayerBodyAttackTarget);

        _navMeshAgent = _parentGameObject.GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = MovementSpeed;
    }

    public override void Idle() { }
    public override void PlayerFound() { }

    public override void Attack()
    {
        _preparingAttack = true;
        _navMeshAgent.isStopped = true;
        _currentShotTarget = _playerBodyAttackTarget.transform;
        _parentGameObject.transform.LookAt(_currentShotTarget);

        Vector3 relativePos = _currentShotTarget.position - _parentGameObject.transform.position;
        _attackEnemyRotation = Quaternion.LookRotation(relativePos);
    }

    public override void Move()
    {
        if(_preparingAttack)
        {
            if(IsEnemyFacingPlayerPos())
            {
                _preparingAttack = false;
                ShootProjectile();
                onAttackEnded?.Invoke();
            }
            else
            {
                Quaternion current = _parentGameObject.transform.localRotation;
                _parentGameObject.transform.localRotation =
                    Quaternion.Slerp(current, _attackEnemyRotation, Time.deltaTime * RotationSpeed);
            }

            return;
        }

        //Run away from Player
        if(_parentGameObject != null &&
            Vector3.Distance(
                _parentGameObject.transform.position, _playerBodyAttackTarget.transform.position) < DistanceToMaintain)
        {
            _navMeshAgent.isStopped = false;
            var dirToPlayer = _parentGameObject.transform.position - _playerBodyAttackTarget.transform.position;
            _navMeshAgent.SetDestination(_parentGameObject.transform.position + dirToPlayer);
        }
        //Run towards player
        else if(_parentGameObject != null &&
            Vector3.Distance(
                _parentGameObject.transform.position, _playerBodyAttackTarget.transform.position) > AttackRange)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_playerBodyAttackTarget.transform.position);
        }
    }

    public override void Death() { }

    public override void DebugLines(Quaternion rotation)
    {
        if(_parentGameObject == null)
        {
            return;
        }

        Debug.DrawCircle(
            _parentGameObject.transform.position,
            rotation,
            ProjectileAttackData.ProjectileRange,
            Color.magenta);

        Debug.DrawCircle(
            _parentGameObject.transform.position,
            rotation,
            DistanceToMaintain,
            Color.green);
    }

    private void ShootProjectile()
    {
        var enemyProjectile = Instantiate(
            ProjectileAttackData.ProjectilePrefab,
            _firingPosition.position,
            _firingPosition.rotation);
    }

    private bool IsEnemyFacingPlayerPos()
    {
        if(_parentGameObject == null)
        {
            return false;
        }

        var dirFromAtoB = (_currentShotTarget.position - _parentGameObject.transform.position).normalized;
        var dotProd = Vector3.Dot(dirFromAtoB, _parentGameObject.transform.forward);
        return dotProd > 0.9;
    }
}
