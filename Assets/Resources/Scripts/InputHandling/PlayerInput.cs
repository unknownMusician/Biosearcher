using Biosearcher.PlayerBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.InputHandling
{
    public class PlayerInput : System.IDisposable
    {
        protected Player.Presenter playerPresenter;

        public PlayerInput(Player.Presenter playerPresenter)
        {
            this.playerPresenter = playerPresenter;
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

        protected void HandleTangentAccelerationStart(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            playerPresenter.TangentAcceleration = Mathf.Sign(ctx.ReadValue<float>()); // todo: small gamepad trigger press would not matter
        }

        protected void HandleTangentAccelerationStop(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            playerPresenter.TangentAcceleration = 0;
        }

        protected void HandleNormalAccelerationStart(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            playerPresenter.NormalAcceleration = Mathf.Sign(ctx.ReadValue<float>()); // todo: small gamepad trigger press would not matter
        }

        protected void HandleNormalAccelerationStop(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            playerPresenter.NormalAcceleration = 0;
        }
    }
}
