using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Biosearcher.Common
{
    // todo
    public static class CommonMonoBehaviour
    {
        private static CommonMonoBehaviourComponent instance;
        private static CommonMonoBehaviourComponent Instance 
            => instance ??= new GameObject(typeof(CommonMonoBehaviour).ToString()).AddComponent<CommonMonoBehaviourComponent>();

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
        }
    }
}