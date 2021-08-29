using Biosearcher.Refactoring;
using System;
using UnityEngine.Scripting;

namespace Biosearcher.Common
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    [NeedsRefactor(Needs.Remove)]
    public class ExecutionOrderMethodAttribute : PreserveAttribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    [NeedsRefactor(Needs.Remove)]
    public sealed class AwakeMethodAttribute : ExecutionOrderMethodAttribute
    {
        public readonly string DisplayedName;

        public AwakeMethodAttribute(string displayedName = null) => DisplayedName = displayedName;
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    [NeedsRefactor(Needs.Remove)]
    public sealed class OnDestroyMethodAttribute : ExecutionOrderMethodAttribute { }
}