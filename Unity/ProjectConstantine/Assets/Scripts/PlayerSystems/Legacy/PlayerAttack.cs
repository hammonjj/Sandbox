using Constantine;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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
    public Transform[] AttackSpawnPoints;

    public GameObject HeadAimTarget;
    public GameObject LeftArmAimTarget;
    public GameObject RightArmAimTarget;
    public GameObject RightArmAimMover;

    public GameObject ProjectileAttack;

    [Tooltip("Amount of time it takes for the head to return to it's original position after firing")]
    public float HeadReturnTime = 0.33f;

    //Attack
    private bool _canPrimaryAttack = true;
    private bool _canSecondaryAttack = true;
    private float _primaryAttackTimeoutCurrent;
    private float _secondaryAttackTimeoutCurrent;

    private float _attackRange = 5f;

    private Vector3 _baseHeadPosition;
    private Vector3 _baseRightFiringPosition;

    //Animation Controls
    private bool _returnHeadPositionRunning = false;
    private bool _returnFiringPositionsRunning = false;
    private IEnumerator _returnHeadPositionCoroutine;
    private IEnumerator _returnFiringPositionsCoroutine;

    private IRigConstraint _rightArmMoverConstraint;

    private void Awake()
    {
        //Setup Input Events
        //var playerInputs = GetComponent<PlayerInputs>();
        //playerInputs.onPlayerPrimaryAttack += OnPrimaryAttack;
        //playerInputs.onPlayerSecondaryAttack += OnSecondaryAttack;

        _rightArmMoverConstraint = RightArmAimMover.GetComponent<IRigConstraint>();

        //Reset Timeouts
        _primaryAttackTimeoutCurrent = PrimaryAttackTimeout;
        _secondaryAttackTimeoutCurrent = SecondaryAttackTimeout;

        //Base Target Positions
        _baseHeadPosition = new Vector3(0, HeadAimTarget.transform.position.y, HeadAimTarget.transform.position.z);
        _baseRightFiringPosition = new Vector3(
            RightArmAimTarget.transform.localPosition.x, 
            RightArmAimTarget.transform.localPosition.y, 
            RightArmAimTarget.transform.localPosition.z);
    }

    private void OnPrimaryAttack()
    {
        if(!_canPrimaryAttack)
        {
            return; 
        }

        LogDebug("OnPrimaryAttack called");

        if(_returnHeadPositionRunning)
        {
            StopCoroutine(_returnHeadPositionCoroutine);
        }

        if(_returnFiringPositionsRunning)
        {
            StopCoroutine(_returnFiringPositionsCoroutine);
        }

        _canPrimaryAttack = false;
        _primaryAttackTimeoutCurrent = PrimaryAttackTimeout;
        var projectileRotation = AttackSpawnPoint.rotation;

        //Watch for performance issues - might need to put enemies on their own layer
        var collidersHit = Physics.OverlapSphere(gameObject.transform.position, _attackRange);
        if(collidersHit.Length > 0)
        {
            var enemyFound = false;
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

                    enemyFound = true;
                    projectileRotation = Quaternion.LookRotation(normalizedColliderVector, Vector3.up);
                    
                    //Adjust Aim Target Animations
                    HeadAimTarget.transform.position = colliderHit.transform.position;
                    RightArmAimTarget.transform.position = colliderHit.transform.position;
                    _rightArmMoverConstraint.weight = 1.0f;
                    //LeftArm
                    break;
                }
            }

            if(!enemyFound)
            {
                HeadAimTarget.transform.localPosition = new Vector3(0, _baseHeadPosition.y, _baseHeadPosition.z);
                RightArmAimTarget.transform.localPosition = new Vector3(
                    _baseRightFiringPosition.x, _baseRightFiringPosition.y, _baseRightFiringPosition.z);
                _rightArmMoverConstraint.weight = 1.0f;
            }
        }

        Instantiate(ProjectileAttack, AttackSpawnPoint.position, projectileRotation);

        _returnHeadPositionCoroutine = ReturnHeadPosition(_baseHeadPosition, HeadReturnTime);
        _returnFiringPositionsCoroutine = ReturnFiringPositions(_baseRightFiringPosition, HeadReturnTime);
        StartCoroutine(_returnHeadPositionCoroutine);
        StartCoroutine(_returnFiringPositionsCoroutine);
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

    private IEnumerator ReturnHeadPosition(Vector3 targetPosition, float returnTimeDuration)
    {
        _returnHeadPositionRunning = true;
        yield return CoroutineWaiter();

        var returnTime = 0f;
        var startPosition = HeadAimTarget.transform.localPosition;
        while(returnTime < returnTimeDuration)
        {
            HeadAimTarget.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, returnTime / returnTimeDuration);
            returnTime += Time.deltaTime;
            yield return null;
        }

        HeadAimTarget.transform.localPosition = targetPosition;
        _returnHeadPositionRunning = false;
    }

    private IEnumerator ReturnFiringPositions(Vector3 targetPosition, float returnTimeDuration)
    {
        _returnFiringPositionsRunning = true;

        yield return CoroutineWaiter();

        var returnTime = 0f;
        var startPosition = HeadAimTarget.transform.localPosition;
        while(returnTime < returnTimeDuration)
        {
            _rightArmMoverConstraint.weight = Mathf.Lerp(1.0f, 0.115f, returnTime / returnTimeDuration);
            RightArmAimTarget.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, returnTime / returnTimeDuration);
            returnTime += Time.deltaTime;
            yield return null;
        }

        RightArmAimTarget.transform.localPosition = targetPosition;
        _returnFiringPositionsRunning = false;
    }

    private IEnumerator CoroutineWaiter()
    {
        var totalWaitTime = 1f;
        var waitTimeDuration = 0f;
        while(waitTimeDuration < totalWaitTime)
        {
            waitTimeDuration += Time.deltaTime;
            yield return null;
        }
    }
}
