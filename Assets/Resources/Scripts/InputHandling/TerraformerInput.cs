using Biosearcher.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Biosearcher.InputHandling
{
    public class TerraformerInput : System.IDisposable
    {
        protected Terraformer.Presenter _terraformerPresenter;
        protected InputAction.CallbackContext _callbackContext;

        protected bool _isAdding = false;

        public TerraformerInput(Terraformer.Presenter playerPresenter)
        {
            _terraformerPresenter = playerPresenter;
            SetInput(CustomInput.controls);
        }
        public void Dispose() => UnsetInput(CustomInput.controls);

        public void OnEnable() => CustomInput.controls.Terraformer.Enable();
        public void OnDisable() => CustomInput.controls.Terraformer.Disable();

        protected void SetInput(Controls controls)
        {
            controls.Terraformer.AddStart.performed += HandleTerraformerAddStart;
            controls.Terraformer.AddStop.performed += HandleTerraformerAddStop;
        }
        protected void UnsetInput(Controls controls)
        {
            controls.Terraformer.AddStart.performed -= HandleTerraformerAddStart;
            controls.Terraformer.AddStop.performed -= HandleTerraformerAddStop;
        }

        protected void HandleTerraformerAddStart(InputAction.CallbackContext ctx)
        {
            _isAdding = true;
            _terraformerPresenter.Terraformer.StartCoroutine(Adding());
        }
        protected void HandleTerraformerAddStop(InputAction.CallbackContext ctx)
        {
            _isAdding = false;
        }

        protected IEnumerator Adding()
        {
            yield return new WaitForFixedUpdate();
            while (_isAdding)
            {
                _terraformerPresenter.Add();
                yield return new WaitForFixedUpdate();
            }
        }
    }
}