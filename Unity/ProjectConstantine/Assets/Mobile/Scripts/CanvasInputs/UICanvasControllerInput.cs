using UnityEngine;

namespace Constantine
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public PlayerInputs PlayerInputs;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            PlayerInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualAttackInput(bool virtualJumpState)
        {
            PlayerInputs.AttackInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualDashState)
        {
            PlayerInputs.DashInput(virtualDashState);
        }
    }
}
