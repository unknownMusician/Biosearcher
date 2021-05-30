using Biosearcher.PlayerBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.InputHandling
{
    public class PlayerRobotInput : System.IDisposable
    {
        protected PlayerRobot.Presenter playerPresenter;

        public PlayerRobotInput(PlayerRobot.Presenter playerPresenter)
        {
            this.playerPresenter = playerPresenter;
            SetInput(CustomInput.controls);
        }
        public void Dispose() => UnsetInput(CustomInput.controls);

        public void OnEnable() => CustomInput.controls.Player.Enable();
        public void OnDisable() => CustomInput.controls.Player.Disable();

        protected void SetInput(Controls controls)
        {
            controls.Player.TorqueStart.performed += HandlePlayerTorqueStart;
            controls.Player.TorqueStop.performed += HandlePlayerTorqueStop;
        }
        protected void UnsetInput(Controls controls)
        {
            controls.Player.TorqueStart.performed -= HandlePlayerTorqueStart;
            controls.Player.TorqueStop.performed -= HandlePlayerTorqueStop;
        }

        protected void HandlePlayerTorqueStart(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            playerPresenter.WheelVelocity = Mathf.Sign(ctx.ReadValue<float>()); // todo: small gamepad trigger press would not matter
        }

        protected void HandlePlayerTorqueStop(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            playerPresenter.WheelVelocity = 0;
        }
    }
}
