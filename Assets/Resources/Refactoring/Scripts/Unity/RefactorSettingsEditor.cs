#if UNITY_EDITOR

using Biosearcher.Common;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Biosearcher.Refactoring
{
    [CustomEditor(typeof(RefactorSettings))]
    [NeedsRefactor(Needs.Optimization, Needs.Review)]
    internal sealed class RefactorSettingsEditor : Editor
    {
        private SerializedProperty _refactorCheckEnabled;
        private SerializedProperty _showInConsole;

        private GUIStyle _headerStyle;
        private GUIStyle _labelStyle;
        private GUIStyle _logStyle;

        private string _logNamespace = "";
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

        public override void OnInspectorGUI()
        {
            HandleRefactorProperties();
            ShowRefactorHeader();

            Log[] logs = RefactorManager.Logs;

            ShowNamespcaeFilter(logs);
            ShowLogs(logs);
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

        private void ShowNamespcaeFilter(Log[] logs)
        {
            float newHorizontalWidth = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)).width;

            _horizontalWidth = newHorizontalWidth == 0 ? _horizontalWidth : newHorizontalWidth;

            float labelWidth = 120;
            EditorGUILayout.LabelField("<color=#c4c4c4>Namespace filter: </color>", _labelStyle, GUILayout.MaxWidth(labelWidth));

            ShowDropDown(labelWidth, logs);

            float namespaceTextAreaWidth = Mathf.Max(_horizontalWidth - labelWidth, labelWidth);
            _logNamespace = EditorGUILayout.TextArea(_logNamespace, GUILayout.MinWidth(labelWidth),
                GUILayout.Width(namespaceTextAreaWidth), GUILayout.ExpandHeight(true));

            EditorGUILayout.EndHorizontal();
        }

        private void ShowLogs(Log[] logs)
        {
            Log[] filteredLogs = logs.Where(log => log.Namespace.ToLower().Contains(_logNamespace.ToLower())).ToArray();

            int i;
            for (i = 0; i < filteredLogs.Length; i++)
            {
                Rect verticalRect = EditorGUILayout.BeginVertical("Button");

                if (GUI.Button(verticalRect, GUIContent.none))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(filteredLogs[i].FilePath, filteredLogs[i].LineNumber, filteredLogs[i].ColumnNumber);
                }
                EditorGUILayout.LabelField($"<color=#ffffff>{filteredLogs[i].Text}</color>", _logStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                EditorGUILayout.EndVertical();
            }
            if (i == 0 && _refactorCheckEnabled.boolValue)
            {
                EditorGUILayout.HelpBox($"Bro, nice job! There is nothing to refactor{(_logNamespace == "s" ? "" : " in this namespace")}. Come back later! See you soooooooon. \n\n You still here?  ⊙﹏⊙",
                    MessageType.None, true);
            }
        }

        private void ShowDropDown(float labelWidth, Log[] logs)
        {
            if (EditorGUILayout.DropdownButton(new GUIContent("Biosearcher..."), FocusType.Keyboard, GUILayout.Width(labelWidth)))
            {
                GUI.FocusControl("");
                var menu = new GenericMenu();

                menu.AddItem(new GUIContent("[Biosearcher]"), "Biosearcher".Contains(_logNamespace), () => _logNamespace = "");
                menu.AddSeparator("");

                ToDropdownNamespaces(logs.Select(log => log.Namespace.Substring(log.Namespace.IndexOf('.') + 1)))
                    .Foreach(@namespace =>
                    {
                        menu.AddItem(new GUIContent(@namespace.Replace('.', '/') + $"/[{@namespace.Split('.').Last()}]"),
                            @namespace.Contains(_logNamespace), () => _logNamespace = @namespace);
                        menu.AddSeparator(@namespace.Replace('.', '/') + "/");
                    });

                menu.ShowAsContext();
            }
        }

        private static List<string> ToDropdownNamespaces(IEnumerable<string> namespaces)
        {
            //namespaces = namespaces.Select(@namespace => @namespace.Replace('.', '/'));

            var finNamespaces = new List<string>();

            for (int i = 0; namespaces.Count() > 0; i++)
            {
                namespaces = namespaces.Where(@namespace => @namespace.Split('.').Length > i);
                namespaces.Select(@namespace => @namespace.Split('.').Length == i + 1 ? @namespace : @namespace.Substring(0, @namespace.IndexOf(@namespace.Split('.')[i + 1]) - 1))
                    .Distinct().Foreach(@namespace => finNamespaces.Add(@namespace));
            }

            return finNamespaces;
        }
    }
}

#endif
