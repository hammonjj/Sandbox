using System;
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
		public bool Dash;

		//Player Action Events
        public Action onPlayerPrimaryAttack;
        public Action onPlayerSecondaryAttack;

        private GameStateManager _gameStateManager;

        void Awake()
        {
            _gameStateManager = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
        }

        public void OnMove(InputValue value)
		{
			Move = value.Get<Vector2>();
		}

		public void OnPrimaryAttack(InputValue value)
		{
            onPlayerPrimaryAttack?.Invoke();
        }

        public void OnSecondaryAttack(InputValue value)
        {
			onPlayerSecondaryAttack?.Invoke();
        }

        public void OnDash(InputValue value)
		{
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