using Biosearcher.Common;
using Biosearcher.InputHandling;
using Biosearcher.Refactoring;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Biosearcher.Player.Movement
{
    [RequireComponent(typeof(Walker))]
    public sealed class PlayerInput : MonoBehaviour
    {
        private Walker _walker;

        private void Awake()
        {
            this.SetComponents(out _walker);

            SetInput(CustomInput.controls);
        }
        private void OnDestroy() => UnsetInput(CustomInput.controls);

        private void OnEnable() => CustomInput.controls.Player.Enable();
        private void OnDisable() => CustomInput.controls.Player.Disable();

        private void SetInput(Controls controls)
        {
            controls.Player.TangentAccelerationStart.performed += HandleTangentAccelerationStart;
            controls.Player.TangentAccelerationStop.performed += HandleTangentAccelerationStop;

            controls.Player.NormalAccelerationStart.performed += HandleNormalAccelerationStart;
            controls.Player.NormalAccelerationStop.performed += HandleNormalAccelerationStop;
        }
        private void UnsetInput(Controls controls)
        {
            controls.Player.TangentAccelerationStart.performed -= HandleTangentAccelerationStart;
            controls.Player.TangentAccelerationStop.performed -= HandleTangentAccelerationStop;

            controls.Player.NormalAccelerationStart.performed -= HandleNormalAccelerationStart;
            controls.Player.NormalAccelerationStop.performed -= HandleNormalAccelerationStop;
        }

        [NeedsRefactor("small gamepad trigger press would not matter")]
        private void HandleTangentAccelerationStart(InputAction.CallbackContext ctx)
        {
            _walker.TangentAcceleration = Mathf.Sign(ctx.ReadValue<float>());
        }

        private void HandleTangentAccelerationStop(InputAction.CallbackContext ctx)
        {
            _walker.TangentAcceleration = 0;
        }

        [NeedsRefactor("small gamepad trigger press would not matter")]
        private void HandleNormalAccelerationStart(InputAction.CallbackContext ctx)
        {
            _walker.NormalAcceleration = Mathf.Sign(ctx.ReadValue<float>());
        }

        private void HandleNormalAccelerationStop(InputAction.CallbackContext ctx)
        {
            _walker.NormalAcceleration = 0;
        }
    }
}
