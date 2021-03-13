using Biosearcher.PlayerBehaviour;
using System.Collections;
using UnityEngine;

namespace Biosearcher.InputHandling
{
    public class PlayerInput
    {
        protected Player.Presenter playerPresenter;
        protected UnityEngine.InputSystem.InputAction.CallbackContext callbackContext;

        protected bool _isMoving = false;
        protected bool IsMoving
        {
            get => _isMoving;
            set
            {
                if (_isMoving == value)
                {
                    return;
                }
                _isMoving = value;
                if (_isMoving)
                {
                    playerPresenter.Player.StartCoroutine(Moving());
                }
            }
        }

        public PlayerInput(Player.Presenter playerPresenter)
        {
            this.playerPresenter = playerPresenter;
            SetInput(CustomInput.controls);
        }
        public void OnDestroy() => UnsetInput(CustomInput.controls);

        public void OnEnable() => CustomInput.controls.Player.Enable();
        public void OnDisable() => CustomInput.controls.Player.Disable();

        protected void SetInput(Controls controls)
        {
            controls.Player.MoveStart.performed += HandlePlayerMoveStart;
            controls.Player.MoveStop.performed += HandlePlayerMoveStop;
        }
        protected void UnsetInput(Controls controls)
        {
            controls.Player.MoveStart.performed -= HandlePlayerMoveStart;
            controls.Player.MoveStop.performed -= HandlePlayerMoveStop;
        }

        protected void HandlePlayerMoveStart(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            callbackContext = ctx;
            IsMoving = true;
        }

        protected void HandlePlayerMoveStop(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            callbackContext = ctx;
            IsMoving = false;
        }

        protected IEnumerator Moving()
        {
            yield return new WaitForFixedUpdate();
            while (IsMoving)
            {
                playerPresenter.Move(callbackContext.ReadValue<Vector2>());
                yield return new WaitForFixedUpdate();
            }
            playerPresenter.Move(Vector2.zero);
        }
    }
}
