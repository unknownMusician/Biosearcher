using Biosearcher.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Biosearcher.Refactoring
{
    internal static class EditorCommon
    {
        internal static void ShowNamespaceFilter(Log[] logs, GUIStyle labelStyle, string logNamespace, ref float horizontalWidth, Action<string> logNamespaceSetter)
        {
            float newHorizontalWidth = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true)).width;

            horizontalWidth = newHorizontalWidth == 0 ? horizontalWidth : newHorizontalWidth;

            const float labelWidth = 120;
            EditorGUILayout.LabelField("<color=#c4c4c4>Namespace filter: </color>", labelStyle, GUILayout.MaxWidth(labelWidth));

            ShowDropDown(labelWidth, logs, logNamespace, logNamespaceSetter);

            float namespaceTextAreaWidth = Mathf.Max(horizontalWidth - labelWidth * 2, labelWidth);
            logNamespace = EditorGUILayout.TextField(logNamespace, GUILayout.MinWidth(labelWidth), GUILayout.Width(namespaceTextAreaWidth));

            logNamespaceSetter(logNamespace);

            EditorGUILayout.EndHorizontal();
        }

        [NeedsRefactor(Needs.Optimization)]
        private static void ShowDropDown(float labelWidth, Log[] logs, string logNamespace, Action<string> logNamespaceSetter)
        {
            if (EditorGUILayout.DropdownButton(new GUIContent("Biosearcher..."), FocusType.Keyboard, GUILayout.Width(labelWidth)))
            {
                GUI.FocusControl("");
                var menu = new GenericMenu();

                menu.AddItem(new GUIContent("[Biosearcher]"), "Biosearcher".Contains(logNamespace), () => logNamespaceSetter(""));
                menu.AddSeparator("");

                ToDropdownNamespaces(logs.Select(log => log.Namespace.Substring(log.Namespace.IndexOf('.') + 1)))
                    .Foreach(@namespace =>
                    {
                        menu.AddItem(new GUIContent(@namespace.Replace('.', '/') + $"/[{@namespace.Split('.').Last()}]"),
                            @namespace.Contains(logNamespace), () => logNamespaceSetter(@namespace));
                        menu.AddSeparator(@namespace.Replace('.', '/') + "/");
                    });

                menu.ShowAsContext();
            }
        }

        [NeedsRefactor(Needs.Optimization)]
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

        internal static void ShowLogs(RefactorSettings.Parameters parameters, Log[] logs, string logNamespace, GUIStyle logStyle, ref Vector2 scrollPosition)
        {
            Log[] filteredLogs = logs.Where(log => log.Namespace.ToLower().Contains(logNamespace.ToLower())).ToArray();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(false));

            int i;
            for (i = 0; i < filteredLogs.Length; i++)
            {
                Rect verticalRect = EditorGUILayout.BeginVertical("Button");


                if (GUI.Button(verticalRect, GUIContent.none))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(filteredLogs[i].FilePath, filteredLogs[i].LineNumber, filteredLogs[i].ColumnNumber);
                }
                EditorGUILayout.LabelField($"<color=#ffffff>{filteredLogs[i].Text}</color>", logStyle);

                EditorGUILayout.EndVertical();
            }
            if (i == 0 && parameters.enabled)
            {
                EditorGUILayout.HelpBox($"Bro, nice job! There is nothing to refactor{(logNamespace == "" ? "" : " in this namespace")}. " +
                    $"Come back later! See you soooooooon. \n\n You still here?  ⊙﹏⊙",
                    MessageType.None, true);
            }

            EditorGUILayout.EndScrollView();
        }

        internal static void ShowRefactorSettings(ref bool showParameters, Action showParams, GUIStyle headerStyle)
        {
            showParameters = EditorGUILayout.BeginFoldoutHeaderGroup(showParameters, "Parameters");
            if (showParameters)
            {
                EditorGUI.indentLevel++;

                showParams();

                EditorGUILayout.Space();
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("<color=#c4c4c4>Need Refactor:</color>", headerStyle);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}