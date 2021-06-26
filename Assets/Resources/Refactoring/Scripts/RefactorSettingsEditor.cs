#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Biosearcher.Refactoring
{
    [CustomEditor(typeof(RefactorSettings))]
    internal sealed class RefactorSettingsEditor : Editor
    {
        private SerializedProperty _refactorCheckEnabled;
        private SerializedProperty _showInConsole;

        private GUIStyle _headerStyle;
        private GUIStyle _labelStyle;
        private GUIStyle _logStyle;

        private string _logNamespace;
        private float _horizontalWidth;

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

            _refactorCheckEnabled = serializedObject.FindProperty("_refactorCheckEnabled");
            _showInConsole = serializedObject.FindProperty("_showInConsole");
        }

        [NeedsRefactor(Needs.Reformat)]
        public override void OnInspectorGUI()
        {
            HandleRefactorProperties();

            ShowRefactorHeader();

            ShowNamespcaeFilter();

            ShowLogs();
        }

        private void HandleRefactorProperties()
        {
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.PropertyField(_refactorCheckEnabled);
            EditorGUILayout.PropertyField(_showInConsole);

            serializedObject.ApplyModifiedProperties();
        }

        private void ShowRefactorHeader()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("<color=#c4c4c4>Need Refactor:</color>", _headerStyle);
        }

        private void ShowNamespcaeFilter()
        {
            float newHorizontalWidth = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)).width;

            _horizontalWidth = newHorizontalWidth == 0 ? _horizontalWidth : newHorizontalWidth;

            float labelWidth = 120;
            EditorGUILayout.LabelField("<color=#c4c4c4>Namespace filter: </color>", _labelStyle, GUILayout.MaxWidth(labelWidth));

            float namespaceTextAreaWidth = Mathf.Max(_horizontalWidth - labelWidth, labelWidth);
            _logNamespace = EditorGUILayout.TextArea(_logNamespace, GUILayout.MinWidth(labelWidth),
                GUILayout.Width(namespaceTextAreaWidth), GUILayout.ExpandHeight(true)) ?? "";

            EditorGUILayout.EndHorizontal();
        }

        private void ShowLogs()
        {
            Log[] logs = RefactorManager.Logs.Where(log => log.Namespace.ToLower().Contains(_logNamespace.ToLower())).ToArray();

            int i;
            for (i = 0; i < logs.Length; i++)
            {
                Rect verticalRect = EditorGUILayout.BeginVertical("Button");

                if (GUI.Button(verticalRect, GUIContent.none))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(logs[i].FilePath, logs[i].LineNumber, logs[i].ColumnNumber);
                }
                EditorGUILayout.LabelField($"<color=#ffffff>{logs[i].Text}</color>", _logStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                EditorGUILayout.EndVertical();
            }
            if (i == 0 && _refactorCheckEnabled.boolValue)
            {
                EditorGUILayout.HelpBox($"Bro, nice job! There is nothing to refactor{(_logNamespace == "s" ? "" : " in this namespace")}. Come back later! See you soooooooon. \n\n You still here?  ⊙﹏⊙",
                    MessageType.None, true);
            }
        }
    }
}

#endif
