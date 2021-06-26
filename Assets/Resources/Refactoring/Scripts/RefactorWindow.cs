#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Biosearcher.Refactoring
{
    internal sealed class RefactorWindow : EditorWindow
    {
        private static RefactorSettings refactorSettings;

        private GUIStyle _headerStyle;
        private GUIStyle _labelStyle;
        private GUIStyle _logStyle;

        private string _logNamespace;
        private float _horizontalWidth;
        private Vector2 _scrollPosition;
        private bool _showParameters;

        [MenuItem("Window/Biosearcher/Refactor")]
        private static void Init() => GetWindow<RefactorWindow>(false, "Refactor", true);

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
            refactorSettings = Resources.Load<RefactorSettings>("Settings/Refactor Settings");
        }

        private void OnGUI()
        {
            RefactorSettings.Parameters parameters = refactorSettings.Params;

            TryShowParameters(refactorSettings.Params);
            ShowNamespaceFilter();
            ShowLogs(parameters);
        }

        private void TryShowParameters(RefactorSettings.Parameters parameters)
        {
            _showParameters = EditorGUILayout.BeginFoldoutHeaderGroup(_showParameters, "Parameters");
            if (_showParameters)
            {
                EditorGUI.indentLevel++;
                refactorSettings = (RefactorSettings)EditorGUILayout.ObjectField(refactorSettings, typeof(RefactorSettings), false);
                if (refactorSettings != null)
                {
                    EditorGUILayout.Space();
                    GUI.enabled = false;
                    EditorGUILayout.Toggle("Refactor Check Enabled", parameters.enabled);
                    EditorGUILayout.Toggle("Show In Console", parameters.showInConsole);
                    GUI.enabled = true;
                }

                EditorGUILayout.Space();
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("<color=#c4c4c4>Need Refactor:</color>", _headerStyle);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

        }

        private void ShowNamespaceFilter()
        {
            float newHorizontalWidth = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true)).width;

            _horizontalWidth = newHorizontalWidth == 0 ? _horizontalWidth : newHorizontalWidth;

            float labelWidth = 120;
            EditorGUILayout.LabelField("<color=#c4c4c4>Namespace filter: </color>", _labelStyle, GUILayout.MaxWidth(labelWidth));

            float namespaceTextAreaWidth = Mathf.Max(_horizontalWidth - labelWidth, labelWidth);
            _logNamespace = EditorGUILayout.TextArea(_logNamespace, GUILayout.MinWidth(labelWidth), GUILayout.Width(namespaceTextAreaWidth));

            EditorGUILayout.EndHorizontal();
        }

        private void ShowLogs(RefactorSettings.Parameters parameters)
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            Log[] logs = RefactorManager.Logs.Where(log => log.Namespace.ToLower().Contains(_logNamespace.ToLower())).ToArray();
            int i;
            for (i = 0; i < logs.Length; i++)
            {
                Rect verticalRect = EditorGUILayout.BeginVertical("Button");

                if (GUI.Button(verticalRect, GUIContent.none))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(logs[i].FilePath, logs[i].LineNumber, logs[i].ColumnNumber);
                }
                EditorGUILayout.LabelField($"<color=#ffffff>{logs[i].Text}</color>", _logStyle, GUILayout.ExpandWidth(true));

                EditorGUILayout.EndVertical();
            }
            if (i == 0 && parameters.enabled)
            {
                EditorGUILayout.HelpBox($"Bro, nice job! There is nothing to refactor{(_logNamespace == "s" ? "" : " in this namespace")}. " +
                    $"Come back later! See you soooooooon. \n\n You still here?  ⊙﹏⊙",
                    MessageType.None, true);
            }

            EditorGUILayout.EndScrollView();
        }
    }
}

#endif
