using Biosearcher.Refactoring;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Biosearcher.Common
{
    [NeedsRefactor]
    public static class CommonMonoBehaviour
    {
        private static CommonMonoBehaviourComponent instance;
        public static CommonMonoBehaviourComponent Instance
        {
            [NeedsRefactor("if nobody calls Instance, Awake will not be called in the start")]
            get
            {
                if (instance == null)
                {
                    instance = new GameObject(nameof(CommonMonoBehaviour)).AddComponent<CommonMonoBehaviourComponent>();

                    CommonConstMethods.Awake(instance);
                }
                return instance;
            }
        }

        public static event UnityAction OnDrawGizmos
        {
            add => Instance._onGizmos += value;
            remove => Instance._onGizmos -= value;
        }
        public static event UnityAction OnDrawGizmosSelected
        {
            add => Instance._onGizmos += value;
            remove => Instance._onGizmosSelected -= value;
        }

        public static Coroutine StartCoroutine(IEnumerator coroutine) => Instance.StartCoroutine(coroutine);
        public static Coroutine StartCoroutine<T>(this T _, IEnumerator coroutine) => StartCoroutine(coroutine);

        public sealed class CommonMonoBehaviourComponent : MonoBehaviour
        {
            public event UnityAction _onGizmos;
            public event UnityAction _onGizmosSelected;

            private void OnDrawGizmos() => _onGizmos?.Invoke();
            private void OnDrawGizmosSelected() => _onGizmosSelected?.Invoke();

            private void OnDestroy() => instance = null;
        }
    }
}