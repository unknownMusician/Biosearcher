#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Biosearcher.Refactoring
{
    [CustomEditor(typeof(RefactorSettings))]
    internal sealed class SettingsEditor : Editor
    {
        private SerializedProperty _refactorCheckEnabled;
        private SerializedProperty _showInConsole;

        private RefactorSettings settings;

        private GUIStyle _headerStyle;
        private GUIStyle _labelStyle;
        private GUIStyle _logStyle;

        private string _logNamespace = "";
        private float _horizontalWidth;
        private Vector2 _scrollPosition;
        [SerializeField] private bool _showParameters;

        void OnEnable()
        {
            _headerStyle = new GUIStyle
            {
                richText = true,
                wordWrap = true,
                fontStyle = FontStyle.Bold
            };
            _labelStyle = new GUIStyle
            {
                richText = true,
                wordWrap = true
            };
            _logStyle = new GUIStyle
            {
                richText = true,
                wordWrap = true
            };

            settings = (RefactorSettings)serializedObject.targetObject;
            _refactorCheckEnabled = serializedObject.FindProperty("_refactorCheckEnabled");
            _showInConsole = serializedObject.FindProperty("_showInConsole");
        }

        public override void OnInspectorGUI()
        {
            EditorCommon.ShowRefactorSettings(ref _showParameters, ShowRefactorParameters, _headerStyle);

            Log[] logs = RefactorManager.Logs;

            EditorCommon.ShowNamespaceFilter(logs, _labelStyle, _logNamespace, ref _horizontalWidth, logNamespace => _logNamespace = logNamespace);
            EditorCommon.ShowLogs(settings.Params, logs, _logNamespace, _logStyle, ref _scrollPosition);
        }

        private void ShowRefactorParameters()
        {
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.PropertyField(_refactorCheckEnabled);
            EditorGUILayout.PropertyField(_showInConsole);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif
