using Biosearcher.Common;
using Biosearcher.InputHandling;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Biosearcher.Player.Interactions.Hand
{
    [RequireComponent(typeof(Hand))]
    public sealed class HandInput : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private Hand _hand;

        private Vector2 MousePosition => Mouse.current.position.ReadValue();

        private void Awake()
        {
            this.SetComponents(out _hand);

            SetInput(CustomInput.controls);
        }

        private void Update() => _hand._lookRay = _camera.ScreenPointToRay(MousePosition);

        private void OnDestroy() => UnsetInput(CustomInput.controls);

        private void OnEnable() => CustomInput.controls.Hand.Enable();
        private void OnDisable() => CustomInput.controls.Hand.Disable();

        private void SetInput(Controls controls)
        {
            controls.Hand.Grab.performed += HandleGrab;
            controls.Hand.Drop.performed += HandleDrop;
            controls.Hand.Insert.performed += HandleInsert;
        }
        private void UnsetInput(Controls controls)
        {
            controls.Hand.Grab.performed -= HandleGrab;
            controls.Hand.Drop.performed -= HandleDrop;
            controls.Hand.Insert.performed -= HandleInsert;
        }

        private void HandleGrab(InputAction.CallbackContext ctx) => _hand.TryGrab();
        private void HandleDrop(InputAction.CallbackContext ctx) => _hand.TryDrop();
        private void HandleInsert(InputAction.CallbackContext ctx) => _hand.TryInsert();
    }
}