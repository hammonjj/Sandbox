using UnityEngine;

[CreateAssetMenu(fileName = "TurretEnemyData", menuName = "Enemy/TurretEnemyData")]
public class TurretEnemyData : BaseEnemyData
{
    [Header("Turret")]
    public EnemyProjectileBaseData ProjectileAttackData;

    public Transform _firingPosition;
    private GameObject _playerBodyAttackTarget;

    public override void Setup(GameObject parentGameObject)
    {
        parentGameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        _playerBodyAttackTarget = GameObject.FindGameObjectWithTag(Constants.Tags.PlayerBodyAttackTarget);
    }

    public override void Idle(GameObject parentGameObject)
    {
        //Turrets don't move
    }

    public override void PlayerFound() { }

    public override void Attack(GameObject parentGameObject, Transform firingPosition)
    {
        Instantiate(
            ProjectileAttackData.ProjectilePrefab,
            firingPosition.position,
            firingPosition.rotation);

        onAttackEnded?.Invoke();
    }

    public override void Move(GameObject parentGameObject)
    {
        var playerTransform = _playerBodyAttackTarget.transform;

        playerTransform.position = new Vector3(
            _playerBodyAttackTarget.transform.position.x,
            parentGameObject.transform.position.y,
            _playerBodyAttackTarget.transform.position.z);

        parentGameObject.transform.LookAt(playerTransform);
    }

    public override void Death() { }

    public override void DebugLines(Quaternion rotation, GameObject parentGameObject)
    {
        Debug.DrawCircle(
            parentGameObject.transform.position,
            rotation,
            ProjectileAttackData.ProjectileRange,
            Color.magenta);
    }
}