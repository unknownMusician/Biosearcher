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
        private readonly static int needCount = Enum.GetNames(typeof(Needs)).Length;

        internal string NeededAction { get; }
#endif

        public NeedsRefactorAttribute(string neededAction)
        {
#if UNITY_EDITOR
            NeededAction = neededAction;
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
            NeededAction = NeedsToString(finalNeed);
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
#endif
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

    [Flags]
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
        MakeOwnClass = 128,
    }
}