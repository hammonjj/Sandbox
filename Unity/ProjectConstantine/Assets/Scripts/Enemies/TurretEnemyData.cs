using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "TurretEnemyData", menuName = "Enemy/TurretEnemyData")]
public class TurrentEnemyData : BaseEnemyData
{
    [Header("Turret")]
    public float AttackDamage = 10f;
    public EnemyProjectileBaseData ProjectileAttackData;

    private Transform _firingPosition;
    private GameObject _parentGameObject;
    private GameObject _playerBodyAttackTarget;

    //m_Script: {fileID: 11500000, guid: ade4ed07debeb41ccb2e670b4b50bf36, type: 3}

    //Virtual Methods
    public override void Setup(GameObject parentGameObject)
    {
        _parentGameObject = parentGameObject;
        _firingPosition = _parentGameObject.transform.Find("FiringPosition");
        _parentGameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        _playerBodyAttackTarget = GameObject.FindGameObjectWithTag(Constants.Tags.PlayerBodyAttackTarget);
    }

    public override void Idle() { }
    public override void PlayerFound() { }

    public override void Attack()
    {
        var enemyProjectile = Instantiate(
            ProjectileAttackData.ProjectilePrefab,
            _firingPosition.position,
            _firingPosition.rotation);

        enemyProjectile.name = "TurretProjectileAttack";
    }

    public override void Move()
    {
        _parentGameObject.transform.LookAt(_playerBodyAttackTarget.transform);
    }

    public override void DebugLines() { }
}