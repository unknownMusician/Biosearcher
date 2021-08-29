#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Biosearcher.Refactoring
{
    internal sealed class Window : EditorWindow
    {
        private static RefactorSettings settings;

        private GUIStyle _headerStyle;
        private GUIStyle _labelStyle;
        private GUIStyle _logStyle;

        private string _logNamespace = "";
        private float _horizontalWidth;
        private Vector2 _scrollPosition;
        [SerializeField] private bool _showParameters;

        [MenuItem("Window/Biosearcher/Refactor")]
        private static void Init()
        {
            Window window = GetWindow<Window>(false, "Refactor", true);

            window.Show();
        }

        private void OnEnable()
        {
            _headerStyle = new GUIStyle
            {
                richText = true,
                fontStyle = FontStyle.Bold
            };
            _labelStyle = new GUIStyle
            {
                richText = true
            };
            _logStyle = new GUIStyle
            {
                richText = true,
                wordWrap = true
            };

            settings = Resources.Load<RefactorSettings>("Settings/Refactor Settings");
        }

        private void OnGUI()
        {
            RefactorSettings.Parameters parameters = settings.GetParamsSafe();

            EditorCommon.ShowRefactorSettings(ref _showParameters,() => ShowRefactorParameters(parameters), _headerStyle);

            Log[] logs = RefactorManager.Logs;

            EditorCommon.ShowNamespaceFilter(logs, _labelStyle, _logNamespace, ref _horizontalWidth, logNamespace => _logNamespace = logNamespace);
            EditorCommon.ShowLogs(parameters, logs, _logNamespace, _logStyle, ref _scrollPosition);
        }

        private void ShowRefactorParameters(RefactorSettings.Parameters parameters)
        {
            settings = (RefactorSettings)EditorGUILayout.ObjectField(settings, typeof(RefactorSettings), false);
            if (settings != null)
            {
                EditorGUILayout.Space();
                GUI.enabled = false;
                EditorGUILayout.Toggle("Refactor Check Enabled", parameters.enabled);
                EditorGUILayout.Toggle("Show In Console", parameters.showInConsole);
                GUI.enabled = true;
            }
        }
    }
}

#endif
