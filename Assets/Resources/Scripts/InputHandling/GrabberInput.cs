using Biosearcher.PlayerBehaviour;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Biosearcher.InputHandling
{
    public class GrabberInput : System.IDisposable
    {
        protected Grabber.Presenter _grabberPresenter;

        public Vector2 MousePosition => Mouse.current.position.ReadValue();

        public GrabberInput(Grabber.Presenter grabberPresenter)
        {
            this._grabberPresenter = grabberPresenter;
            SetInput(CustomInput.controls);
        }

        public void Dispose() => UnsetInput(CustomInput.controls);

        public void OnEnable() => CustomInput.controls.Grabber.Enable();
        public void OnDisable() => CustomInput.controls.Grabber.Disable();

        protected void SetInput(Controls controls)
        {
            controls.Grabber.Grab.performed += HandleGrab;
            controls.Grabber.Drop.performed += HandleDrop;
        }
        protected void UnsetInput(Controls controls)
        {
            controls.Grabber.Grab.performed -= HandleGrab;
            controls.Grabber.Drop.performed -= HandleDrop;
        }

        protected void HandleGrab(InputAction.CallbackContext ctx) => _grabberPresenter.Grab(MousePosition);
        protected void HandleDrop(InputAction.CallbackContext ctx) => _grabberPresenter.Drop();

    }
}