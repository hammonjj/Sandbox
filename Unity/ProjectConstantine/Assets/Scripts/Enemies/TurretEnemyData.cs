﻿using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "TurretEnemyData", menuName = "Enemy/TurretEnemyData")]
public class TurretEnemyData : BaseEnemyData
{
    [Header("Turret")]
    public EnemyProjectileBaseData ProjectileAttackData;

    private Transform _firingPosition;
    private GameObject _parentGameObject;
    private GameObject _playerBodyAttackTarget;

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

    public override void Death() { }
    public override void DebugLines() { }
}