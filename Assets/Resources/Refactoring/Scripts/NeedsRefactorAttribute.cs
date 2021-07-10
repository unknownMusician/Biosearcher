using Biosearcher.Common;
using System;
using UnityEngine.Scripting;

namespace Biosearcher.Refactoring
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class NeedsRefactorAttribute : PreserveAttribute
    {
#if UNITY_EDITOR
        internal string NeededAction { get; }
#endif

        public NeedsRefactorAttribute()
#if UNITY_EDITOR
            : this(Needs.Refactor.ToString())
#endif
        { }
        public NeedsRefactorAttribute(string neededAction, params Needs[] neededActions)
#if UNITY_EDITOR
            : this(neededActions)
#endif
        {
#if UNITY_EDITOR
            NeededAction = $"{neededAction}, {NeededAction}";
#endif
        }
        public NeedsRefactorAttribute(params Needs[] neededActions)
        {
#if UNITY_EDITOR
            Needs finalNeed = 0;
            neededActions.Foreach(need => finalNeed |= need);
            NeededAction = finalNeed.ToString();
#endif
        }
    }
}