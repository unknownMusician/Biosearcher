using Biosearcher.Common;
using System;

namespace Biosearcher.Refactoring
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class NeedsRefactorAttribute : Attribute
    {
#if UNITY_EDITOR
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
            : this(neededAction.ToString())
#endif
        { }
        public NeedsRefactorAttribute(params Needs[] neededActions)
        {
#if UNITY_EDITOR
            Needs finalNeed = 0;
            neededActions.Foreach(need => finalNeed |= need);
            finalNeed.ToString();
#endif
        }
    }
}