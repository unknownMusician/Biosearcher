using Biosearcher.PlayerBehaviour;
using System.Collections;
using UnityEngine;

namespace Biosearcher.InputHandling
{
    public class PlayerCameraInput : System.IDisposable
    {
        protected PlayerCamera.Presenter cameraPresenter;
        protected bool isMoving;

        protected const float mouseSpeed = 0.1f;
        protected const float gamepadSpeed = 1.5f;

        public PlayerCameraInput(PlayerCamera.Presenter cameraPresenter)
        {
            this.cameraPresenter = cameraPresenter;
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
        }
        protected void UnsetInput(Controls controls)
        {
            controls.Camera.Move.performed -= HandleCameraMove;
            controls.Camera.MoveStart.performed -= HandleCameraMoveStart;
            controls.Camera.MoveStop.performed -= HandleCameraMoveStop;
        }

        protected void HandleCameraMove(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            cameraPresenter.Rotate(ctx.ReadValue<Vector2>() * mouseSpeed);
        }

        protected void HandleCameraMoveStart(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            isMoving = true;
            cameraPresenter.Camera.StartCoroutine(Move(ctx));
        }

        protected void HandleCameraMoveStop(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            isMoving = false;
        }

        protected IEnumerator Move(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            yield return new WaitForFixedUpdate();
            while (isMoving)
            {
                cameraPresenter.Rotate(ctx.ReadValue<Vector2>() * gamepadSpeed);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}