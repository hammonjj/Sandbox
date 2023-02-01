using System;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviourBase
{
    public bool DrawDebugLines = true;
    public float OrbDistanceFromPlayerCenter = 0.75f;
    public Vector3 RotationAxis;
    public float AngularVelocity = 20f;
    public float OrbRespawnRate;
    public GameObject OrbPrefab;
    public float AttackCooldown = 0.50f;
    public int MaxOrbs = 1;

    public Constants.Enums.AttackType AttackType;

    private bool _initialized = false;
    private bool _canAttack;
    private bool _canSpawnOrb;
    private float _orbCooldownCurrent;
    private float _attackCooldownCurrent;
    private List<GameObject> _orbSpawns = new();

    //Pulled from Orb prefab
    private float _attackRange;

    public void Initialize()
    {
        _initialized = true;
        if(!enabled)
        {
            return;
        }

        CalculateOrbSpawns();
        SpawnInitialOrbs();
        _orbCooldownCurrent = OrbRespawnRate;

        _attackRange = OrbPrefab.GetComponent<BaseOrb>().BaseOrbData.AttackRange;
        if(AttackType == Constants.Enums.AttackType.Primary)
        {
            EventManager.GetInstance().onPlayerPrimaryAttack += OnAttack;
        }
        else if(AttackType == Constants.Enums.AttackType.Secondary)
        {
            EventManager.GetInstance().onPlayerSecondaryAttack += OnAttack;
        }
    }

    private void Start()
    {
        
    }

    private void CalculateOrbSpawns()
    {
        //Calculate Orb Spawns
        //  - Angle in degrees = 360° / number of parts
        //  - Angle in radian = 2π / number of parts
        //  - Angle in multiples of pi = 2 / number of parts
        //  - https://rechneronline.de/winkel/divide-circle.php
        //Draw a ray the center through the line given by the calculation and place a spawn point there
        //Need to figure out how to calculate when the ring is tilted
        //Rotate that spawn point around the origin (player)
        //Spawn an orb when the spawn is empty (has no children)
        //  - The spawn is the orbs parent

        var degreesBetweenSections = (float)360 / MaxOrbs;

        var tmpGameObject = new GameObject();
        tmpGameObject.name = "tmpGOB";

        /*
         * Axis:
         *  (0, 1, 0) -> Horizontal
         *  (0, 0, 1) -> Vertical
         *  (-0.5, 0.5, 0) -> Right High, Left Low
         *  (0.5, 0.5, 0) -> Left High, Right Low
         */

        //Works for flat ring only
        for(int i = 0; i < MaxOrbs; i++)
        {
            var directionOfRay = Quaternion.AngleAxis(i * degreesBetweenSections, RotationAxis) * Vector3.forward;//Vector3.left;

            Ray r = new Ray(transform.position, directionOfRay);
            var point = r.GetPoint(OrbDistanceFromPlayerCenter);

            var gObjI = Instantiate(tmpGameObject, point, transform.rotation);
            gObjI.transform.SetParent(transform);
            gObjI.name = $"OrbSpawn-{i}";
            _orbSpawns.Add(gObjI);
        }

        Destroy(tmpGameObject);
    }

    private void SpawnInitialOrbs()
    {
        foreach(var spawn in _orbSpawns)
        {
            var orb = Instantiate(OrbPrefab, spawn.transform.position, Quaternion.identity);
            orb.name = $"Orb-{Guid.NewGuid()}";
            orb.transform.SetParent(spawn.transform);
        }
    }

    private void Update()
    {
        if(!_initialized || !enabled)
        {
            return;
        }

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
        
        if(GetCurrentOrbCount() < MaxOrbs)
        {
            UpdateOrbCooldown();
        }

        if(_canSpawnOrb)
        {
            _canSpawnOrb = false;
            _orbCooldownCurrent = OrbRespawnRate;
            SpawnOrb();
        }

        //Rotate orbs
        foreach(var spawn in _orbSpawns)
        {
            spawn.transform.RotateAround(
                gameObject.transform.position, new Vector3(0,1,0)/*RotationAxis*/, AngularVelocity * Time.deltaTime);
        }
    }

    private void SpawnOrb()
    {
        LogDebug("Spawning Orb");
        var spawn = GetFirstVacantSpawn();
        var orb = Instantiate(OrbPrefab, spawn.transform.position, Quaternion.identity);
        orb.name = $"Orb-{Guid.NewGuid()}";
        orb.transform.SetParent(spawn.transform);
    }

    private GameObject GetFirstOrb()
    {
        foreach(var spawn in _orbSpawns)
        {
            if(spawn.transform.childCount != 0)
            {
                return spawn.transform.GetChild(0).gameObject;
            }
        }

        return null;
    }

    private GameObject GetFirstVacantSpawn()
    {
        foreach(var spawn in _orbSpawns)
        {
            if(spawn.transform.childCount == 0)
            {
                return spawn;
            }
        }

        return null;
    }

    private int GetCurrentOrbCount()
    {
        var count = 0;
        foreach(var spawn in _orbSpawns)
        {
            if(spawn.transform.childCount != 0)
            {
                count++;
            }
        }

        return count;
    }

    private void OnAttack()
    {
        //Select orb and fire it at enemy
        if(!_canAttack || GetCurrentOrbCount() == 0)
        {
            LogDebug("No Orbs or can't attack");
            return;
        }

        LogDebug("Firing orb");
        _canAttack = false;
        _attackCooldownCurrent = AttackCooldown;

        var firstOrb = GetFirstOrb();
        if(firstOrb == null)
        {
            LogError("First orb is null");
            return;
        }

        var (enemyPos, projectileRotation) = FindEnemiesToAttack(firstOrb);

        firstOrb.transform.parent = null;
        firstOrb.transform.rotation = projectileRotation;
        var orb = firstOrb.GetComponent<BaseOrb>();
        orb.Fire();
    }

    private void UpdateAttackCooldown()
    {
        _attackCooldownCurrent -= Time.deltaTime;
        _canAttack = _attackCooldownCurrent <= 0.0f;
    }

    private void UpdateOrbCooldown()
    {
        _orbCooldownCurrent -= Time.deltaTime;
        _canSpawnOrb = _orbCooldownCurrent <= 0.0f;
    }

    protected (Vector3, Quaternion) FindEnemiesToAttack(GameObject orb)
    {
        var projectileRotation = gameObject.transform.rotation;
        Vector3 retVector = Vector3.zero;

        if(this == null)
        {
            return (retVector, projectileRotation); 
            //return (new Vector3(0, 0, 0), Quaternion.identity);
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

            var normalizedColliderVector = colliderHit.transform.position - orb.transform.position;
            normalizedColliderVector.Normalize();

            //0 = 180 degree arc - 0.5 = 90 degree arc
            if(Vector3.Dot(normalizedColliderVector, orb.transform.forward) < 0.5f)
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
