using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviourBase
{
    public Transform OrbSpawn;
    public Vector3 RotationAxis;
    public float AngularVelocity = 20f;
    public float OrbRespawnRate;
    public GameObject OrbPrefab;
    public float AttackCooldown = 0.50f;
    public int MaxOrbs = 1;

    private bool _canAttack;
    private bool _canSpawnOrb;
    private float _orbCooldownCurrent;
    private float _attackCooldownCurrent;
    private List<GameObject> _orbs = new();

    private void Awake()
    {
        //Need to spread orbs out at even intervals around player
        EventManager.GetInstance().onPlayerPrimaryAttack += OnAttack;
    }

    private void Start()
    {
        var orb = Instantiate(OrbPrefab, OrbSpawn);
        _orbs.Add(orb);
    }

    private void Update()
    {
        UpdateAttackCooldown();

        if(_orbs.Count < MaxOrbs)
        {
            UpdateOrbCooldown();
        }

        if(_canSpawnOrb)
        {
            LogDebug("Spawning Orb");
            _canSpawnOrb = false;
            _orbCooldownCurrent = OrbRespawnRate;
            var orb = Instantiate(OrbPrefab, OrbSpawn);
            _orbs.Add(orb);
        }

        //Equal orb orbit
        //Get orb count
        //Determine orbital pattern based off of number of current number of orbs
        //  - Two orbs -> Opposite Sides
        //  - Three -> Triangle
        //  - Four ->
        //  - Five ->
        //Iterate through orbs and slow them down (except for the first) until they reach the right point
        foreach(var orb in _orbs)
        {
            //Check if orb is equal distance from the other orbs and slow them down
            orb.transform.RotateAround(
                gameObject.transform.position, RotationAxis, AngularVelocity * Time.deltaTime);
        }
    }

    private void OnAttack()
    {
        //Select orb and fire it at enemy
        if(_orbs.Count == 0 || !_canAttack)
        {
            LogDebug("No Orbs or can't attack");
            return;
        }

        LogDebug("Firing orb");
        var firstOrb = _orbs[0];
        _orbs.RemoveAt(0);

        var (enemyPos, projectileRotation) = FindEnemiesToAttack(firstOrb);
        _canAttack = false;
        _attackCooldownCurrent = AttackCooldown;

        firstOrb.transform.rotation = projectileRotation;
        var orb = firstOrb.GetComponent<Orb>();
        orb.HasBeenFired = true;
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
            return (new Vector3(0, 0, 0), Quaternion.identity);
        }

        //Watch for performance issues - might need to put enemies on their own layer
        var collidersHit = Physics.OverlapSphere(gameObject.transform.position, 10f);
        if(collidersHit.Length > 0)
        {
            foreach(var colliderHit in collidersHit)
            {
                if(colliderHit.gameObject.tag != "Enemy")
                {
                    continue;
                }

                var normalizedColliderVector = colliderHit.transform.position - orb.transform.position;
                normalizedColliderVector.Normalize();

                //0 = 180 degree arc - 0.5 = 90 degree arc
                if(Vector3.Dot(normalizedColliderVector, orb.transform.forward) > 0.5f)
                {
                    LogDebug("Enemy Detected in Range");

                    //var attackTarget = colliderHit.gameObject.GetComponent<EnemyBase>().AttackTarget;
                    retVector = colliderHit.gameObject.transform.position;
                    projectileRotation = Quaternion.LookRotation(normalizedColliderVector, Vector3.up);
                    break;
                }
            }
        }

        return (retVector, projectileRotation);
    }
}
