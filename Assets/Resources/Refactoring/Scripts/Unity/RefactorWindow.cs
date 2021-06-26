#if UNITY_EDITOR

using Biosearcher.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Biosearcher.Refactoring
{
    [NeedsRefactor(Needs.Optimization, Needs.Review)]
    internal sealed class RefactorWindow : EditorWindow
    {
        private static RefactorSettings refactorSettings;

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
            RefactorWindow window = GetWindow<RefactorWindow>(false, "Refactor", true);

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

            refactorSettings = Resources.Load<RefactorSettings>("Settings/Refactor Settings");
        }

        private void OnGUI()
        {
            RefactorSettings.Parameters parameters = refactorSettings.GetParamsSafe();

            TryShowParameters(parameters);

            Log[] logs = RefactorManager.Logs;

            ShowNamespaceFilter(logs);
            ShowLogs(parameters, logs);
            GUI.SetNextControlName("");
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

        private void ShowNamespaceFilter(Log[] logs)
        {
            float newHorizontalWidth = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true)).width;

            _horizontalWidth = newHorizontalWidth == 0 ? _horizontalWidth : newHorizontalWidth;

            float labelWidth = 120;
            EditorGUILayout.LabelField("<color=#c4c4c4>Namespace filter: </color>", _labelStyle, GUILayout.MaxWidth(labelWidth));

            ShowDropDown(labelWidth, logs);

            float namespaceTextAreaWidth = Mathf.Max(_horizontalWidth - labelWidth * 2, labelWidth);
            _logNamespace = EditorGUILayout.TextField(_logNamespace, GUILayout.MinWidth(labelWidth), GUILayout.Width(namespaceTextAreaWidth));

            EditorGUILayout.EndHorizontal();
        }

        private void ShowLogs(RefactorSettings.Parameters parameters, Log[] logs)
        {
            Log[] filteredLogs = logs.Where(log => log.Namespace.ToLower().Contains(_logNamespace.ToLower())).ToArray();
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            int i;
            for (i = 0; i < filteredLogs.Length; i++)
            {
                Rect verticalRect = EditorGUILayout.BeginVertical("Button");

                if (GUI.Button(verticalRect, GUIContent.none))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(filteredLogs[i].FilePath, filteredLogs[i].LineNumber, filteredLogs[i].ColumnNumber);
                }
                EditorGUILayout.LabelField($"<color=#ffffff>{filteredLogs[i].Text}</color>", _logStyle, GUILayout.ExpandWidth(true));

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
