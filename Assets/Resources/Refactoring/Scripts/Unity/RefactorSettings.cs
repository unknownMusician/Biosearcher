#if UNITY_EDITOR

using UnityEngine;

namespace Biosearcher.Refactoring
{
    [CreateAssetMenu(fileName = "Refactor Settings", menuName = "Refactor/Settings", order = 1)]
    internal sealed class RefactorSettings : ScriptableObject
    {
        [SerializeField] private bool _refactorCheckEnabled;
        [SerializeField] private bool _showInConsole;

        private bool _lastRefactorCheckEnabledValue;

        internal Parameters Params
        {
            get => new Parameters
            {
                enabled = _refactorCheckEnabled,
                showInConsole = _showInConsole
            };
            set
            {
                _refactorCheckEnabled = value.enabled;
                _showInConsole = value.showInConsole;
            }
        }

        private void OnValidate()
        {
            if (_lastRefactorCheckEnabledValue != _refactorCheckEnabled)
            {
                _lastRefactorCheckEnabledValue = _refactorCheckEnabled;
                RefactorManager.HandleSettingsChanged();
            }
        }

        internal struct Parameters
        {
            public bool enabled;
            public bool showInConsole;
        }
    }
}

#endif
