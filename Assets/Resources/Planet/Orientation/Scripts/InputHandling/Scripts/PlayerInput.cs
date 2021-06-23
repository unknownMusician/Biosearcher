using Biosearcher.Player;
using Biosearcher.Refactoring;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Biosearcher.InputHandling
{
    public class PlayerInput : System.IDisposable
    {
        protected Walker.Presenter _playerPresenter;

        public PlayerInput(Walker.Presenter playerPresenter)
        {
            _playerPresenter = playerPresenter;
            SetInput(CustomInput.controls);
        }
        public void Dispose() => UnsetInput(CustomInput.controls);

        public void OnEnable() => CustomInput.controls.Player.Enable();
        public void OnDisable() => CustomInput.controls.Player.Disable();

        protected void SetInput(Controls controls)
        {
            controls.Player.TangentAccelerationStart.performed += HandleTangentAccelerationStart;
            controls.Player.TangentAccelerationStop.performed += HandleTangentAccelerationStop;

            controls.Player.NormalAccelerationStart.performed += HandleNormalAccelerationStart;
            controls.Player.NormalAccelerationStop.performed += HandleNormalAccelerationStop;
        }
        protected void UnsetInput(Controls controls)
        {
            controls.Player.TangentAccelerationStart.performed -= HandleTangentAccelerationStart;
            controls.Player.TangentAccelerationStop.performed -= HandleTangentAccelerationStop;

            controls.Player.NormalAccelerationStart.performed -= HandleNormalAccelerationStart;
            controls.Player.NormalAccelerationStop.performed -= HandleNormalAccelerationStop;
        }

        [NeedsRefactor("small gamepad trigger press would not matter")]
        protected void HandleTangentAccelerationStart(InputAction.CallbackContext ctx)
        {
            _playerPresenter.TangentAcceleration = Mathf.Sign(ctx.ReadValue<float>());
        }

        protected void HandleTangentAccelerationStop(InputAction.CallbackContext ctx)
        {
            _playerPresenter.TangentAcceleration = 0;
        }

        [NeedsRefactor("small gamepad trigger press would not matter")]
        protected void HandleNormalAccelerationStart(InputAction.CallbackContext ctx)
        {
            _playerPresenter.NormalAcceleration = Mathf.Sign(ctx.ReadValue<float>());
        }

        protected void HandleNormalAccelerationStop(InputAction.CallbackContext ctx)
        {
            _playerPresenter.NormalAcceleration = 0;
        }
    }
}
