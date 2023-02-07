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

		private bool _pausePlayerController = false;
		private EventManager _eventManager;

        private void Awake()
        {
			_eventManager = EventManager.GetInstance();
			_eventManager.onPausePlayerController += PausePlayerController;
		}

		public void PausePlayerController(bool value)
		{
			_pausePlayerController = value;
		}

        public void OnMove(InputValue value)
		{
            if(_pausePlayerController)
			{
                Move = Vector2.zero;
                return;
			}

            Move = value.Get<Vector2>();
		}

		public void OnPrimaryAttack(InputValue value)
		{
            if(_pausePlayerController)
            {
                return;
            }

            _eventManager.OnPlayerPrimaryAttack();
		}

        public void OnSecondaryAttack(InputValue value)
        {
            if(_pausePlayerController)
            {
                return;
            }

            _eventManager.OnPlayerSecondaryAttack();
		}

		public void OnUseItem(InputValue value)
        {
            if(_pausePlayerController)
            {
                return;
            }

            _eventManager.OnSupportAbility();
		}

        public void OnDash(InputValue value)
		{
            if(_pausePlayerController)
            {
                return;
            }

            _eventManager.OnPlayerDash();
		}

		public void OnAdvanceScene(InputValue value)
        {
            _eventManager.OnAdvanceScenePressed(value.isPressed);
		}

        public void OnPause(InputValue value)
        {
			_eventManager.OnPause();
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