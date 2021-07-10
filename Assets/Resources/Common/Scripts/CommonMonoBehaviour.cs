using Biosearcher.Refactoring;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Biosearcher.Common
{
    [NeedsRefactor]
    public sealed class CommonMonoBehaviour : MonoBehaviour
    {
        private static CommonMonoBehaviour instance;

        public static event UnityAction DrawGizmos;
        public static event UnityAction DrawGizmosSelected;

        public static new Coroutine StartCoroutine(IEnumerator coroutine) => ((MonoBehaviour)instance).StartCoroutine(coroutine);

        private void Awake()
        {
            instance = this;
            CommonConstMethods.Awake(instance);
        }
        private void OnDestroy()
        {
            CommonConstMethods.OnDestroy(instance);
            instance = null;
        }

        private void OnDrawGizmos() => DrawGizmos?.Invoke();
        private void OnDrawGizmosSelected() => DrawGizmosSelected?.Invoke();
    }

    public static class CommonMonoBehaviourExtensions
    {
        public static Coroutine StartCoroutine<T>(this T _, IEnumerator coroutine) => CommonMonoBehaviour.StartCoroutine(coroutine);
    }
}