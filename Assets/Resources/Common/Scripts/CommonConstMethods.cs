using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Common
{
    [NeedsRefactor(Needs.Remove)]
    public static class CommonConstMethods
    {
        [AwakeMethod]
        [NeedsRefactor(Needs.Remove)]
        public static void Awake()
        {
            Debug.Log("AwakeMethod");
            // todo
        }
        [OnDestroyMethod]
        [NeedsRefactor(Needs.Remove)]
        public static void OnDestroy()
        {
            Debug.Log("OnDestroyMethod");
            // todo
        }
    }
}