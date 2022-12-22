using System.Collections;
using System.Collections.Generic;
using Constantine;
using UnityEngine;

public class PlayerAttack : MonoBehaviourBase
{
    //Add scriptable object for player weapons
    //  - Use regular variables for now

    [Tooltip("Time required to pass before being able to attack again")]
    public float PrimaryAttackTimeout = 0.50f;

    [Tooltip("Time required to pass before being able to attack again")]
    public float SecondaryAttackTimeout = 0.50f;

    [Space(10)]
    [Tooltip("Where the player's attack spawns from")]
    public Transform AttackSpawnPoint;

    public GameObject ProjectileAttack;

    //Attack
    private bool _canPrimaryAttack = true;
    private bool _canSecondaryAttack = true;
    private float _primaryAttackTimeoutCurrent;
    private float _secondaryAttackTimeoutCurrent;

    private float _attackRange = 5f;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        //Setup Input Events
        var playerInputs = GetComponent<PlayerInputs>();
        playerInputs.onPlayerPrimaryAttack += OnPrimaryAttack;
        playerInputs.onPlayerSecondaryAttack += OnSecondaryAttack;

        //Reset Timeouts
        _primaryAttackTimeoutCurrent = PrimaryAttackTimeout;
        _secondaryAttackTimeoutCurrent = SecondaryAttackTimeout;
    }

    private void OnPrimaryAttack()
    {
        if(!_canPrimaryAttack)
        {
            return; 
        }

        LogDebug("OnPrimaryAttack called");

        _canPrimaryAttack = false;
        _primaryAttackTimeoutCurrent = PrimaryAttackTimeout;

        var projectileRotation = AttackSpawnPoint.rotation;

        //Soft Aim Lock:
        //  - https://answers.unity.com/questions/498657/detect-colliders-in-an-arc.html
        //  - Watch for performance issues - might need to put enemies on their own layer
        var collidersHit = Physics.OverlapSphere(gameObject.transform.position, _attackRange);
        if(collidersHit.Length > 0)
        {
            //Might want to rotate character slightly to line up with shot
            foreach(var colliderHit in collidersHit)
            {
                if(colliderHit.gameObject.tag != "Enemy")
                {
                    continue;
                }

                var normalizedColliderVector = colliderHit.transform.position - AttackSpawnPoint.position;
                normalizedColliderVector.Normalize();

                //0 = 180 degree arc - 0.5 = 90 degree arc
                if(Vector3.Dot(normalizedColliderVector, AttackSpawnPoint.forward) > 0.5)
                {
                    LogDebug("Enemy Detected in Range");
                    projectileRotation = Quaternion.LookRotation(normalizedColliderVector, Vector3.up);
                }
            }
        }

        //_animator?.SetBool(_animIDJump, true);
        Instantiate(ProjectileAttack, AttackSpawnPoint.position, projectileRotation);
    }

    private void OnSecondaryAttack()
    {
        LogDebug("OnSecondaryAttack called");
    }

    private void Update()
    {
        _primaryAttackTimeoutCurrent -= Time.deltaTime;
        _canPrimaryAttack = _primaryAttackTimeoutCurrent <= 0.0f;

        _secondaryAttackTimeoutCurrent -= Time.deltaTime;
        _canSecondaryAttack = _secondaryAttackTimeoutCurrent <= 0.0f;
    }
}
