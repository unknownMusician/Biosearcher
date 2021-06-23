using Biosearcher.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Biosearcher.Refactoring
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class NeedsRefactorAttribute : Attribute
    {
#if UNITY_EDITOR
        private const BindingFlags MembersFlags = BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.Public |
                BindingFlags.NonPublic;

        private readonly static int needCount = Enum.GetNames(typeof(Needs)).Length;

        private readonly string _neededAction;

        public static Log[] Logs { get; private set; }

        private static RefactorSettings RefactorSettings => Resources.Load<RefactorSettings>("Settings/Refactor Settings");
#endif

        public NeedsRefactorAttribute(string neededAction)
        {
#if UNITY_EDITOR
            _neededAction = neededAction;
#endif
        }
        public NeedsRefactorAttribute()
#if UNITY_EDITOR
            : this(Needs.Refactor.ToString())
#endif
        { }
        public NeedsRefactorAttribute(Needs neededAction)
#if UNITY_EDITOR
            : this(NeedsToString(neededAction))
#endif
        { }
        public NeedsRefactorAttribute(params Needs[] neededActions)
        {
#if UNITY_EDITOR
            Needs finalNeed = 0;
            foreach (Needs need in neededActions)
            {
                finalNeed |= need;
            }
            _neededAction = NeedsToString(finalNeed);
#endif
        }

#if UNITY_EDITOR
        private static string NeedsToString(Needs neededAction)
        {
            System.Text.StringBuilder neededActionBuilder = new System.Text.StringBuilder();
            int neededActionInt = (int)neededAction;
            if (neededActionInt == 0)
            {
                return Needs.Refactor.ToString();
            }
            for (int i = 0; i < needCount - 1; i++)
            {
                int needInt = neededActionInt & (1 << i);
                if (needInt == 0)
                {
                    continue;
                }

                neededActionBuilder.Append($"{(Needs)needInt}, ");
            }
            neededActionBuilder.Remove(neededActionBuilder.Length - 2, 2);
            return neededActionBuilder.ToString();
        }

        [UnityEditor.InitializeOnLoadMethod]
        internal static void CheckAssembly()
        {
            RefactorSettings.Parameters parameters = RefactorSettings.GetParamsSafe();
            if (parameters.enabled)
            {
                Logs = CreateLogs();
                if (parameters.showInConsole)
                {
                    Logs.Foreach(log => Debug.LogWarningFormat(log.text));
                }
            }
            else
            {
                Logs = new Log[0];
            }
        }

        private static Log[] CreateLogs()
        {
            var logs = new List<Log>();

            Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.Contains(nameof(Biosearcher)))
                //.Where(t => t.IsInterface || t.IsClass || t.IsEnum)
                .Foreach(t => AppendFullTypeLogsIfNeeded(t, logs));

            return logs.ToArray();
        }

        private static void AppendFullTypeLogsIfNeeded(Type type, List<Log> logs)
        {
            if (type.TryGetCustomAttribute(out NeedsRefactorAttribute attribute))
            {
                logs.Add(GetLog(type, attribute));
            }
            type.GetMembers(MembersFlags).Foreach(m => AppendMemberLogIfNeeded(m, type, logs));
        }
        private static void AppendMemberLogIfNeeded(MemberInfo member, Type memberParent, List<Log> logs)
        {
            if (member.TryGetCustomAttribute(out NeedsRefactorAttribute attribute))
            {
                logs.Add(GetLog(member, memberParent, attribute));
            }
        }

        private static Log GetLog(Type type, NeedsRefactorAttribute attribute)
        {
            return new Log
            {
                filePath = ClassFileFinder.FindTypePathOrDefault(type),
                text = GetLogText(type.Namespace, type.Name, attribute),
            };
        }
        private static Log GetLog(MemberInfo member, Type memberParent, NeedsRefactorAttribute attribute)
        {
            return new Log
            {
                filePath = ClassFileFinder.FindTypePathOrDefault(memberParent),
                text = GetLogText(memberParent.Namespace, memberParent.Name, member.Name, attribute),
            };
        }
        private static string GetLogText(string @namespace, string shortName, NeedsRefactorAttribute attribute)
        {
            return $"<b><color=#99ccff>{@namespace}</color>. <color=#99ff99>{shortName}</color></b> needs <color=#ff6699>{attribute._neededAction}</color>.";
        }
        private static string GetLogText(string @namespace, string shortName, string memberName, NeedsRefactorAttribute attribute)
        {
            return $"<b><color=#99ccff>{@namespace}</color>. <color=#99ff99>{shortName}</color>. " +
                $"<color=#ffff99>{memberName}</color></b> needs <color=#ff6699>{attribute._neededAction}</color>.";
        }
#endif
    }

    public struct Log
    {
        public string text;
        public string filePath;
    }

#if UNITY_EDITOR
    internal static class NeedsRefactorAttributeExtensions
    {
        internal static bool TryGetCustomAttribute(this MemberInfo element, out NeedsRefactorAttribute attribute)
        {
            attribute = element.GetCustomAttribute<NeedsRefactorAttribute>();
            return attribute != null;
        }
    }
#endif

    public enum Needs
    {
        Refactor = 0,

        Reformat = 1,
        Optimization = 2,
        Review = 4,
        Remove = 8,
        Implementation = 16,
        Rename = 32,
        RemoveTodo = 64,
    }
}