using Biosearcher.Player;
using Biosearcher.Refactoring;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Biosearcher.InputHandling
{
    public class PlayerCameraInput : System.IDisposable
    {
        protected PlayerCamera.Presenter _cameraPresenter;
        protected bool _isMoving;

        // todo
        [NeedsRefactor]
        protected const float MouseSpeed = 0.1f;
        protected const float GamepadSpeed = 1.5f;

        protected bool isInteracting;

        public PlayerCameraInput(PlayerCamera.Presenter cameraPresenter)
        {
            _cameraPresenter = cameraPresenter;
            SetInput(CustomInput.controls);
        }
        public void Dispose() => UnsetInput(CustomInput.controls);

        public void OnEnable() => CustomInput.controls.Camera.Enable();
        public void OnDisable() => CustomInput.controls.Camera.Disable();

        protected void SetInput(Controls controls)
        {
            controls.Camera.Move.performed += HandleCameraMove;
            controls.Camera.MoveStart.performed += HandleCameraMoveStart;
            controls.Camera.MoveStop.performed += HandleCameraMoveStop;
            controls.Camera.InteractionStateChange.performed += HandleInteractionStateChange;
        }
        protected void UnsetInput(Controls controls)
        {
            controls.Camera.Move.performed -= HandleCameraMove;
            controls.Camera.MoveStart.performed -= HandleCameraMoveStart;
            controls.Camera.MoveStop.performed -= HandleCameraMoveStop;
            controls.Camera.InteractionStateChange.performed -= HandleInteractionStateChange;
        }

        protected void HandleCameraMove(InputAction.CallbackContext ctx)
        {
            if (!isInteracting)
            {
                _cameraPresenter.Rotate(ctx.ReadValue<Vector2>() * MouseSpeed);
            }
        }

        protected void HandleCameraMoveStart(InputAction.CallbackContext ctx)
        {
            if (!isInteracting)
            {
                _isMoving = true;
                _cameraPresenter.Camera.StartCoroutine(Move(ctx));
            }
        }

        protected void HandleCameraMoveStop(InputAction.CallbackContext ctx)
        {
            _isMoving = false;
        }

        protected void HandleInteractionStateChange(InputAction.CallbackContext ctx)
        {
            isInteracting = !isInteracting;
            _isMoving = false;
        }

        protected IEnumerator Move(InputAction.CallbackContext ctx)
        {
            yield return new WaitForFixedUpdate();
            while (_isMoving)
            {
                _cameraPresenter.Rotate(ctx.ReadValue<Vector2>() * GamepadSpeed);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}