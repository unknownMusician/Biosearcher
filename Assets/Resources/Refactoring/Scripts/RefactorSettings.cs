using UnityEditor;
using UnityEngine;

namespace Biosearcher.Refactoring
{
    [CreateAssetMenu(fileName = "Refactor Settings", menuName = "Refactor/Settings", order = 1)]
    public sealed class RefactorSettings : ScriptableObject
    {
        [SerializeField] private bool _refactorCheckEnabled;
        [SerializeField] private bool _showInConsole;

        private bool _lastRefactorCheckEnabledValue;

        public Parameters Params => new Parameters
        {
            enabled = _refactorCheckEnabled,
            showInConsole = _showInConsole
        };

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_lastRefactorCheckEnabledValue != _refactorCheckEnabled)
            {
                _lastRefactorCheckEnabledValue = _refactorCheckEnabled;
                NeedsRefactorAttribute.CheckAssembly();
            }
        }
#endif

        public struct Parameters
        {
            public bool enabled;
            public bool showInConsole;
        }
    }

    public static class RefactorSettingsExtensions
    {
        public static RefactorSettings.Parameters GetParamsSafe(this RefactorSettings settings)
        {
            if (settings == null)
            {
                return new RefactorSettings.Parameters
                {
                    enabled = true,
                    showInConsole = true
                };
            }
            return settings.Params;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(RefactorSettings))]
    public sealed class RefactorSettingsEditor : Editor
    {
        private SerializedProperty _refactorCheckEnabled;
        private SerializedProperty _showInConsole;

        private GUIStyle _headerStyle;
        private GUIStyle _logStyle;

        void OnEnable()
        {
            _headerStyle = new GUIStyle
            {
                richText = true,
                wordWrap = true,
                fontStyle = FontStyle.Bold
            };
            _logStyle = new GUIStyle
            {
                richText = true,
                wordWrap = true
            };

            _refactorCheckEnabled = serializedObject.FindProperty("_refactorCheckEnabled");
            _showInConsole = serializedObject.FindProperty("_showInConsole");
        }

        [NeedsRefactor(Need.Reformat)]
        public override void OnInspectorGUI()
        {
            Log[] logs = NeedsRefactorAttribute.Logs;

            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(_refactorCheckEnabled);
            EditorGUILayout.PropertyField(_showInConsole);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("<color=#c4c4c4>Need Refactor:</color>", _headerStyle);
            int i;
            for (i = 0; i < logs.Length; i++)
            {
                Rect r = EditorGUILayout.BeginVertical("Button");

                if (GUI.Button(r, GUIContent.none))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(logs[i].filePath, 0, 0);
                }
                EditorGUILayout.LabelField($"<color=#ffffff>{logs[i].text}</color>", _logStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                EditorGUILayout.EndVertical();
            }
            if (i == 0 && _refactorCheckEnabled.boolValue)
            {
                EditorGUILayout.HelpBox("Bro, nice job! There is nothing to refactor. Come back later! See you soooooooon. \n\n You still here?  ⊙﹏⊙",
                    MessageType.None, true);
            }
        }
    }
#endif
}