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

		private EventManager _eventManager;

        private void Awake()
        {
			_eventManager = EventManager.GetInstance();
		}

        public void OnMove(InputValue value)
		{
			Move = value.Get<Vector2>();
		}

		public void OnPrimaryAttack(InputValue value)
		{
			_eventManager.OnPlayerPrimaryAttack();
		}

        public void OnSecondaryAttack(InputValue value)
        {
			_eventManager.OnPlayerSecondaryAttack();
		}

		public void OnUseItem(InputValue value)
        {
			_eventManager.OnUseItem();
		}

        public void OnDash(InputValue value)
		{
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