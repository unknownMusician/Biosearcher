using Biosearcher.Refactoring;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Biosearcher.Common
{
    [NeedsRefactor("NeedsCustomEditor")]
    public sealed class CommonMonoBehaviour : MonoBehaviour
    {
        private const BindingFlags MembersFlags =
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        private static CommonMonoBehaviour s_instance;

        public static event UnityAction DrawGizmos;
        public static event UnityAction DrawGizmosSelected;
        public static new event UnityAction Destroy;

        public static new Coroutine StartCoroutine(IEnumerator coroutine) => ((MonoBehaviour)s_instance).StartCoroutine(coroutine);

        private void Awake()
        {
            s_instance = this;
            InvokeCommon<AwakeMethodAttribute>();
        }
        private void OnDestroy()
        {
            InvokeCommon<OnDestroyMethodAttribute>();
            Destroy?.Invoke();
            s_instance = null;
        }

        private static void InvokeCommon<TAttribute>() where TAttribute : ExecutionOrderMethodAttribute
        {
            ReflectionHelper.GetAllMethods<TAttribute>(MembersFlags)
                .Where(method => method.GetParameters().Length == 0)
                .Foreach(method => method.Invoke(method, Array.Empty<object>()));
        }

        private void OnDrawGizmos() => DrawGizmos?.Invoke();
        private void OnDrawGizmosSelected() => DrawGizmosSelected?.Invoke();
    }

    public static class CommonMonoBehaviourExtensions
    {
        public static Coroutine StartCoroutine<T>(this T _, IEnumerator coroutine) => CommonMonoBehaviour.StartCoroutine(coroutine);
    }
}
