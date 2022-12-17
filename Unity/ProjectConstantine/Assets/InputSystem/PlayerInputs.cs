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
            _gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
        }

        public void OnMove(InputValue value)
		{
			Move = value.Get<Vector2>();
		}

		public void OnAttack(InputValue value)
		{
			LogDebug("Attack Pressed");
			Attack = value.isPressed;
		}

		public void OnDash(InputValue value)
		{
            LogDebug("Dash Pressed");
			Dash = value.isPressed;
		}

        public void OnPause(InputValue value)
        {
            LogDebug("Paused Pressed");
            _gameStateManager.PauseOrUnpauseGame();
        }

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