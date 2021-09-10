using Biosearcher.Common;
using Biosearcher.InputHandling;
using Biosearcher.Level;
using Biosearcher.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Biosearcher.Player
{
    [RequireComponent(typeof(PlayerCamera))]
    public sealed class PlayerCameraInput : MonoBehaviour
    {
        [SerializeField] private float _mouseSpeed = 0.1f;
        [SerializeField] private float _gamepadSpeed = 1.5f;
        [SerializeField] private Day _day;

        private PlayerCamera _camera;
        private bool _isMoving;

        private bool _isInteracting;

        private void Awake()
        {
            this.SetComponents(out _camera);
            SetInput(CustomInput.controls);

            _day.OnEnd += OnDisable;
        }
        private void OnDestroy()
        {
            UnsetInput(CustomInput.controls);
            _day.OnEnd -= OnDisable;
        }

        private void OnEnable() => CustomInput.controls.Camera.Enable();
        private void OnDisable() => CustomInput.controls.Camera.Disable();

        private void SetInput(Controls controls)
        {
            controls.Camera.Move.performed += HandleCameraMove;
            controls.Camera.MoveStart.performed += HandleCameraMoveStart;
            controls.Camera.MoveStop.performed += HandleCameraMoveStop;
            controls.Camera.InteractionStateChange.performed += HandleInteractionStateChange;
        }
        private void UnsetInput(Controls controls)
        {
            controls.Camera.Move.performed -= HandleCameraMove;
            controls.Camera.MoveStart.performed -= HandleCameraMoveStart;
            controls.Camera.MoveStop.performed -= HandleCameraMoveStop;
            controls.Camera.InteractionStateChange.performed -= HandleInteractionStateChange;
        }

        private void HandleCameraMove(InputAction.CallbackContext ctx)
        {
            if (!_isInteracting)
            {
                _camera.Rotate(ctx.ReadValue<Vector2>() * _mouseSpeed);
            }
        }

        private void HandleCameraMoveStart(InputAction.CallbackContext ctx)
        {
            if (!_isInteracting)
            {
                _isMoving = true;
                StartCoroutine(Move(ctx));
            }
        }

        private void HandleCameraMoveStop(InputAction.CallbackContext ctx)
        {
            _isMoving = false;
        }

        private void HandleInteractionStateChange(InputAction.CallbackContext ctx)
        {
            _isInteracting = !_isInteracting;
            _isMoving = false;
        }

        private IEnumerator Move(InputAction.CallbackContext ctx)
        {
            while (_isMoving)
            {
                _camera.Rotate(ctx.ReadValue<Vector2>() * _gamepadSpeed);
                yield return null;
            }
        }
    }
}