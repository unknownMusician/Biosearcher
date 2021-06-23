using Biosearcher.Player;
using UnityEngine.InputSystem;

namespace Biosearcher.InputHandling
{
    public class InserterInput : System.IDisposable
    {
        protected readonly Inserter.Presenter _inserterPresenter;

        public InserterInput(Inserter.Presenter inserterPresenter)
        {
            _inserterPresenter = inserterPresenter;
            SetInput(CustomInput.controls);
        }

        public void Dispose() => UnsetInput(CustomInput.controls);

        public void OnEnable() => CustomInput.controls.Inserter.Enable();
        public void OnDisable() => CustomInput.controls.Inserter.Disable();

        protected void SetInput(Controls controls)
        {
            controls.Inserter.Insert.performed += HandleInsert;
        }
        protected void UnsetInput(Controls controls)
        {
            controls.Grabber.Grab.performed -= HandleInsert;
        }

        protected void HandleInsert(InputAction.CallbackContext ctx) => _inserterPresenter.Insert();
    }
}
