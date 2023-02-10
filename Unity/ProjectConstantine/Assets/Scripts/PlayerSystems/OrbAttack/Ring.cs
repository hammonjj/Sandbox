using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviourBase
{
    public bool DrawDebugLines = true;
    public float SoftAimLock = 0.5f;
    public float AngularVelocity = 20f;
    public float AttackCooldown = 0.50f;
    public float OrbDistanceFromPlayerCenter = 0.75f;
    public Vector3 RotationAxis;
    public GameObject OrbStartPosition;
    public GameObject OrbSpawnPrefab;

    [SerializeField] private int _maxOrbs = 3;
    private bool _canAttack;
    private float _attackCooldownCurrent;
    private List<GameObject> _orbSpawns = new();

    //Pulled from Orb prefab for convenience
    private float _attackRange;

    private void Start()
    {
        GetUpgrades();
        CalculateOrbSpawns();
        EventManager.GetInstance().onPlayerPrimaryAttack += OnAttack;
    }
    
    private void CalculateOrbSpawns()
    {
        //Calculate Orb Spawns
        //  - Angle in degrees = 360° / number of parts
        //
        //Axis:
        // (0, 1, 0) -> Horizontal
        // (0, 0, 1) -> Vertical
        // (-0.5, 0.5, 0) -> Right High, Left Low
        // (0.5, 0.5, 0) -> Left High, Right Low
        
        var degreesBetweenSections = (float)360 / _maxOrbs;
        var orbPrefab = VerifyComponent<DataWarehouse>(Constants.Tags.GameStateManager).PrimaryOrbPrefab;
        _attackRange = orbPrefab.GetComponent<BaseOrb>().BaseOrbData.AttackRange;

        //Works for flat ring only
        for(int i = 0; i < _maxOrbs; i++)
        {
            var directionOfRay = Quaternion.AngleAxis(i * degreesBetweenSections, RotationAxis) * Vector3.forward;

            Ray r = new Ray(transform.position, directionOfRay);
            var point = r.GetPoint(OrbDistanceFromPlayerCenter);

            var orbSpawn = Instantiate(OrbSpawnPrefab, point, transform.rotation);
            orbSpawn.transform.SetParent(transform);
            orbSpawn.name = $"OrbSpawn-{i}";
            orbSpawn.GetComponent<OrbSpawn>().Initialize(orbPrefab);

            _orbSpawns.Add(orbSpawn);
        }
    }
    
    private void Update()
    {
        if(DrawDebugLines)
        {
            //Orb orbit
            foreach(var spawn in _orbSpawns)
            {
                Debug.DrawLine(gameObject.transform.position, spawn.transform.position, Color.blue);
            }

            //Attack Range
            var rotation = gameObject.transform.rotation;
            rotation *= Quaternion.Euler(90, 0, 0);
            Debug.DrawCircle(gameObject.transform.position, rotation, _attackRange, Color.blue);
        }

        UpdateAttackCooldown();
        
        //Rotate orbs spawns
        foreach(var spawn in _orbSpawns)
        {
            spawn.transform.RotateAround(
                gameObject.transform.position, new Vector3(0,1,0), AngularVelocity * Time.deltaTime);
        }
    }
    
    private OrbSpawn GetLoadedOrbSpawn()
    {
        foreach(var spawn in _orbSpawns)
        {
            if(spawn.GetComponent<OrbSpawn>().HasOrb())
            {
                return spawn.GetComponent<OrbSpawn>();
            }
        }

        return null;
    }
    
    private int GetCurrentOrbCount()
    {
        var count = 0;
        foreach(var spawn in _orbSpawns)
        {
            //Might need to cache the orb spawn to avoid GetComponent call
            if(spawn.GetComponent<OrbSpawn>().HasOrb())
            {
                count++;
            }
        }

        return count;
    }

    private void OnAttack()
    {
        LogDebug("Ring.OnAttackCalled");
        //Select orb and fire it at enemy
        if(!_canAttack || GetCurrentOrbCount() == 0)
        {
            LogDebug("No Orbs or can't attack");
            return;
        }

        LogDebug("Firing orb");
        _canAttack = false;
        _attackCooldownCurrent = AttackCooldown;

        var orbSpawn = GetLoadedOrbSpawn();
        var (enemyPos, projectileRotation) = FindEnemiesToAttack(OrbStartPosition);

        orbSpawn.Fire(projectileRotation, enemyPos);
    }

    private void UpdateAttackCooldown()
    {
        _attackCooldownCurrent -= Time.deltaTime;
        _canAttack = _attackCooldownCurrent <= 0.0f;
    }

    private void GetUpgrades()
    {
        var abilityTracker = VerifyComponent<PlayerTracker>(Constants.Tags.GameStateManager);
        var upgrades = abilityTracker.GetCurrentUpgrades(Constants.Enums.UpgradeType.PrimaryRing);
        foreach(var upgrade in upgrades)
        {
            var upgradeCast = (PrimaryRingUpgradeData)upgrade;
            switch(upgradeCast.UpgradeName)
            {
                case PrimaryRingUpgradeData.Upgrade.IncreaseOrbs:
                    _maxOrbs++;
                    break;
            }
        }
    }

    protected (Vector3, Quaternion) FindEnemiesToAttack(GameObject orbStartPos)
    {
        var projectileRotation = gameObject.transform.rotation;
        Vector3 retVector = Vector3.zero;

        if(this == null)
        {
            return (retVector, projectileRotation); 
        }

        //Watch for performance issues - might need to put enemies on their own layer
        //and use the non-alloc version of this method
        var collidersHit = Physics.OverlapSphere(gameObject.transform.position, _attackRange);
        if(collidersHit.Length == 0)
        {
            return (retVector, projectileRotation);
        }

        foreach(var colliderHit in collidersHit)
        {
            if(colliderHit.gameObject.tag != Constants.Tags.Enemy)
            {
                continue;
            }

            var normalizedColliderVector = colliderHit.transform.position - orbStartPos.transform.position;
            normalizedColliderVector.Normalize();

            //0 = 180 degree arc - 0.5 = 90 degree arc
            if(Vector3.Dot(normalizedColliderVector, orbStartPos.transform.forward) < SoftAimLock)
            {
                continue;
            }

            //Check if this enemy is closer than the previous one
            LogDebug("Enemy Detected in Range");

            //Check to see if we have found an enemy first
            if(retVector == Vector3.zero)
            {
                retVector = colliderHit.gameObject.transform.position;
                projectileRotation = Quaternion.LookRotation(normalizedColliderVector, Vector3.up);
            }
            else if(Vector3.Distance(gameObject.transform.position, colliderHit.gameObject.transform.position) <
                Vector3.Distance(gameObject.transform.position, retVector))
            {
                retVector = colliderHit.gameObject.transform.position;
                projectileRotation = Quaternion.LookRotation(normalizedColliderVector, Vector3.up);
            }
        }  

        return (retVector, projectileRotation);
    }
}
