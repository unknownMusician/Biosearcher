using Biosearcher.Tools;
using System.Collections;
using UnityEngine;

namespace Biosearcher.InputHandling
{
    public class TerraformerInput : System.IDisposable
    {
        protected Terraformer.Presenter terraformerPresenter;
        protected UnityEngine.InputSystem.InputAction.CallbackContext callbackContext;

        protected bool _isAdding = false;
        protected bool IsAdding
        {
            get => _isAdding;
            set
            {
                if (_isAdding == value)
                {
                    return;
                }
                _isAdding = value;
                if (_isAdding)
                {
                    terraformerPresenter.Terraformer.StartCoroutine(Adding());
                }
            }
        }

        public TerraformerInput(Terraformer.Presenter playerPresenter)
        {
            this.terraformerPresenter = playerPresenter;
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

        protected void HandleTerraformerAddStart(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            IsAdding = true;
        }

        protected void HandleTerraformerAddStop(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            IsAdding = false;
        }

        protected IEnumerator Adding()
        {
            yield return new WaitForFixedUpdate();
            while (IsAdding)
            {
                terraformerPresenter.Add();
                yield return new WaitForFixedUpdate();
            }
        }
    }
}