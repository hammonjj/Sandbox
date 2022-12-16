using EditorExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Constantine
{
	public class PlayerInputs : MonoBehaviourBase
	{
		[Header("Character Input Values")]
		[DisplayWithoutEdit()]
		public Vector2 Move;
		[DisplayWithoutEdit()]
		public bool Attack;
		[DisplayWithoutEdit()]
		public bool Dash;

		private GameStateManager _gameStateManager;

        void Awake()
        {
            _gameStateManager = GameObject.Find("GameStateManager")
				.GetComponent<GameStateManager>();
        }

        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
        }

		public void OnAttack(InputValue value)
		{
			LogDebug("Attack Pressed");
			AttackInput(value.isPressed);
        }

		public void OnDash(InputValue value)
		{
            LogDebug("Dash Pressed");
            DashInput(value.isPressed);
		}

        public void OnPause(InputValue value)
        {
            LogDebug("Paused Pressed");
            _gameStateManager.PauseOrUnpauseGame();
        }

		/* Methods below are triggered by UI - Will need to eventually remove */
        public void MoveInput(Vector2 newMoveDirection)
		{
			Move = newMoveDirection;
		} 

		public void AttackInput(bool newAttackState)
		{
			Attack = newAttackState;
		}

		public void DashInput(bool newDashState)
		{
			Dash = newDashState;
		}
		/**********************************************************************/

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(true);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

        
    }
}