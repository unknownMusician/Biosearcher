using System;
using UnityEngine.Scripting;

namespace Biosearcher.Common
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ExecutionOrderMethodAttribute : PreserveAttribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class AwakeMethodAttribute : ExecutionOrderMethodAttribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class OnDestroyMethodAttribute : ExecutionOrderMethodAttribute { }
}