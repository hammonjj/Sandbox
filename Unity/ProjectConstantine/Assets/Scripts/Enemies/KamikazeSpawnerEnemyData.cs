using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "KamikazeSpawnerEnemyData", menuName = "Enemy/KamikazeSpawnerEnemyData")]
public class KamikazeSpawnerEnemyData : BaseEnemyData
{
    [Header("Kamikaze Spawner")]
    public float EnemySpawnTimer = 5f;
    public float DistanceToMaintain = 2.0f;
    public GameObject SpawnedEnemyPrefab;

    private float _currentEnemySpawnTimer;
    private GameObject _playerBodyAttackTarget;

    public override void Setup(GameObject parentGameObject) 
    {
        _currentEnemySpawnTimer = EnemySpawnTimer;
        
        var navMeshAgent = parentGameObject.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = MovementSpeed;
        _playerBodyAttackTarget = GameObject.FindGameObjectWithTag(Constants.Tags.PlayerBodyAttackTarget);
    }

    public override void UpdateDataObj(GameObject parentGameObject) 
    {
        _currentEnemySpawnTimer -= Time.deltaTime;
        if(_currentEnemySpawnTimer <= 0f)
        {
            _currentEnemySpawnTimer = EnemySpawnTimer;
            var enemy = Instantiate(SpawnedEnemyPrefab, parentGameObject.transform);
            enemy.transform.parent = null;
            enemy.GetComponent<BaseEnemy>().EmitDeathEvent = false;
        }
    }

    public override void Move(GameObject parentGameObject) 
    {
        var navMeshAgent = parentGameObject.GetComponent<NavMeshAgent>();

        //Run away from Player
        if(Vector3.Distance(
            parentGameObject.transform.position, _playerBodyAttackTarget.transform.position) < DistanceToMaintain)
        {
            navMeshAgent.isStopped = false;
            var dirToPlayer = parentGameObject.transform.position - _playerBodyAttackTarget.transform.position;
            navMeshAgent.SetDestination(parentGameObject.transform.position + dirToPlayer);
        }
        else
        {
            //Wander around while pooping enemies
            RandomPatrol(parentGameObject);
        }
    }
}
