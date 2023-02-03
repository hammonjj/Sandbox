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
    public Transform _firingPosition;

    private bool _preparingAttack;
    
    private GameObject _playerBodyAttackTarget;

    private Quaternion _attackEnemyRotation;
    private Vector3 _currentShotTarget;

    public override void Setup(GameObject parentGameObject)
    {
        _playerBodyAttackTarget = GameObject.FindGameObjectWithTag(Constants.Tags.PlayerBodyAttackTarget);

        var navMeshAgent = parentGameObject.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = MovementSpeed;
    }

    public override void PlayerFound() { }

    public override void Attack(GameObject parentGameObject, Transform firingPosition)
    {
        _firingPosition = firingPosition;
        _preparingAttack = true;
        parentGameObject.GetComponent<NavMeshAgent>().isStopped = true;
        _currentShotTarget = _playerBodyAttackTarget.transform.position;
        parentGameObject.transform.LookAt(_playerBodyAttackTarget.transform);

        Vector3 relativePos = _currentShotTarget - parentGameObject.transform.position;
        _attackEnemyRotation = Quaternion.LookRotation(relativePos);
    }

    public override void Move(GameObject parentGameObject)
    {
        if(_preparingAttack)
        {
            if(IsEnemyFacingPlayerPos(parentGameObject))
            {
                _preparingAttack = false;
                ShootProjectile();
                onAttackEnded?.Invoke();
            }
            else
            {
                Quaternion current = parentGameObject.transform.localRotation;
                parentGameObject.transform.localRotation =
                    Quaternion.Slerp(current, _attackEnemyRotation, Time.deltaTime * RotationSpeed);
            }

            return;
        }

        var navMeshAgent = parentGameObject.GetComponent<NavMeshAgent>();

        //Run away from Player
        if(Vector3.Distance(
            parentGameObject.transform.position, _playerBodyAttackTarget.transform.position) < DistanceToMaintain)
        {
            navMeshAgent.isStopped = false;
            var dirToPlayer = parentGameObject.transform.position - _playerBodyAttackTarget.transform.position;
            navMeshAgent.SetDestination(parentGameObject.transform.position + dirToPlayer);
        }
        //Run towards player
        else if(Vector3.Distance(
            parentGameObject.transform.position, _playerBodyAttackTarget.transform.position) > AttackRange)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(_playerBodyAttackTarget.transform.position);
        }
    }

    public override void Death() { }

    public override void DebugLines(Quaternion rotation, GameObject parentGameObject)
    {
        Debug.DrawCircle(
            parentGameObject.transform.position,
            rotation,
            ProjectileAttackData.ProjectileRange,
            Color.magenta);

        Debug.DrawCircle(
            parentGameObject.transform.position,
            rotation,
            DistanceToMaintain,
            Color.green);
    }

    private void ShootProjectile()
    {
        Instantiate(
            ProjectileAttackData.ProjectilePrefab,
            _firingPosition.position,
            _firingPosition.rotation);
    }

    private bool IsEnemyFacingPlayerPos(GameObject parentGameObject)
    {
        var dirFromAtoB = (_currentShotTarget - parentGameObject.transform.position).normalized;
        var dotProd = Vector3.Dot(dirFromAtoB, parentGameObject.transform.forward);
        return dotProd > 0.9;
    }
}
