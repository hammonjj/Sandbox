using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Assets.Scripts.PlayerSystems
{
    public class DualPistolAttack : PlayerAttackBase
    {
        public GameObject LeftArmAimMover;
        public GameObject LeftArmAimTarget;

        public GameObject RightArmAimMover;
        public GameObject RightArmAimTarget;

        private IRigConstraint _leftArmMoverConstraint;
        private IRigConstraint _rightArmMoverConstraint;

        private Vector3 _baseLeftFiringPosition;
        private Vector3 _baseRightFiringPosition;

        private bool _returnFiringPositionsRunning = false;

        private void Awake()
        {
            _attackCooldownCurrent = AttackCooldown;

            //EventManager.GetInstance().onPlayerPrimaryAttack += OnAttack;
            
            _baseLeftFiringPosition = new Vector3(
                LeftArmAimTarget.transform.localPosition.x,
                LeftArmAimTarget.transform.localPosition.y,
                LeftArmAimTarget.transform.localPosition.z);
            
            _baseRightFiringPosition = new Vector3(
                RightArmAimTarget.transform.localPosition.x,
                RightArmAimTarget.transform.localPosition.y,
                RightArmAimTarget.transform.localPosition.z);

            _leftArmMoverConstraint = LeftArmAimMover.GetComponent<IRigConstraint>();
            _rightArmMoverConstraint = RightArmAimMover.GetComponent<IRigConstraint>();
        }

        private void Update()
        {
            UpdateAttackCooldown();
        }

        protected override void OnAttack()
        {
            if(!_canAttack)
            {
                return;
            }

            LogDebug("OnAttack Called");
            if(_returnFiringPositionsRunning)
            {
                if(this == null)
                {
                    //StopAllCoroutines();
                }
            }

            _canAttack = false;
            _attackCooldownCurrent = AttackCooldown;

            _leftArmMoverConstraint.weight = ShootAnimationConstraintWeight;
            _rightArmMoverConstraint.weight = ShootAnimationConstraintWeight;

            var (enemyPos, projectileRotation) = FindEnemiesToAttack();
            if(enemyPos == Vector3.zero)
            {
                //Fire straight if no enemy found
                LeftArmAimTarget.transform.localPosition = new Vector3(
                    _baseLeftFiringPosition.x, _baseLeftFiringPosition.y, _baseLeftFiringPosition.z);
                
                RightArmAimTarget.transform.localPosition = new Vector3(
                    _baseRightFiringPosition.x, _baseRightFiringPosition.y, _baseRightFiringPosition.z);
            }
            else
            {
                RightArmAimTarget.transform.position = enemyPos;
                LeftArmAimTarget.transform.position = enemyPos;
            }

            Instantiate(AttackProjectile, AttackSpawnPoints[0].position, projectileRotation);

            if(this != null)
            {
                StartCoroutine(ReturnFiringPositions(enemyPos));
            }
        }

        private IEnumerator ReturnFiringPositions(Vector3 startPosition)
        {
            _returnFiringPositionsRunning = true;

            yield return CoroutineWaiter();

            var returnTime = 0f;
            while(returnTime < AimAnimationReturnTime)
            {
                _leftArmMoverConstraint.weight = RunAnimationConstraintWeight;
                _rightArmMoverConstraint.weight = RunAnimationConstraintWeight;
                //Left Arm
                //_leftArmMoverConstraint.weight = Mathf.Lerp(
                //    ShootAnimationConstraintWeight, RunAnimationConstraintWeight, returnTime / AimAnimationReturnTime);
                LeftArmAimTarget.transform.localPosition = Vector3.Lerp(
                    startPosition, _baseLeftFiringPosition, returnTime / AimAnimationReturnTime);

                //Right Arm
                //_rightArmMoverConstraint.weight = Mathf.Lerp(
                //    ShootAnimationConstraintWeight, RunAnimationConstraintWeight, returnTime / AimAnimationReturnTime);
                RightArmAimTarget.transform.localPosition = Vector3.Lerp(
                    startPosition, _baseRightFiringPosition, returnTime / AimAnimationReturnTime);

                returnTime += Time.deltaTime;
                yield return null;
            }

            LeftArmAimTarget.transform.localPosition = _baseLeftFiringPosition;
            RightArmAimTarget.transform.localPosition = _baseRightFiringPosition;
            
            _returnFiringPositionsRunning = false;
        }

        private void ValidatePrerequisites()
        {
            if(LeftArmAimTarget == null)
            {

            }

            if(RightArmAimTarget == null)
            {

            }
        }
    }
}
