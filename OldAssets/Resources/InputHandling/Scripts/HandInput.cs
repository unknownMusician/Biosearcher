using Biosearcher.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Biosearcher.InputHandling
{
    public class HandInput : System.IDisposable
    {
        protected readonly Hand.Presenter _handPresenter;

        public Vector2 MousePosition => Mouse.current.position.ReadValue();

        public HandInput(Hand.Presenter handPresenter)
        {
            _handPresenter = handPresenter;
            SetInput(CustomInput.controls);
        }

        public void Dispose() => UnsetInput(CustomInput.controls);

        public void OnEnable() => CustomInput.controls.Hand.Enable();
        public void OnDisable() => CustomInput.controls.Hand.Disable();

        protected void SetInput(Controls controls)
        {
            controls.Hand.Grab.performed += HandleGrab;
            controls.Hand.Drop.performed += HandleDrop;
            controls.Hand.Insert.performed += HandleInsert;
        }
        protected void UnsetInput(Controls controls)
        {
            controls.Hand.Grab.performed -= HandleGrab;
            controls.Hand.Drop.performed -= HandleDrop;
            controls.Hand.Insert.performed -= HandleInsert;
        }

        protected void HandleGrab(InputAction.CallbackContext ctx) => _handPresenter.Grab();
        protected void HandleDrop(InputAction.CallbackContext ctx) => _handPresenter.Drop();
        protected void HandleInsert(InputAction.CallbackContext ctx) => _handPresenter.Insert();
    }
}