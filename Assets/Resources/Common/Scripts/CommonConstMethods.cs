using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Common
{
    [NeedsRefactor(Need.Optimization, Need.Reformat, Need.Remove)]
    public static class CommonConstMethods
    {
        [NeedsRefactor(Need.Optimization | Need.Reformat)]
        public static void Awake(MonoBehaviour behaviour)
        {
            // todo
        }
    }
}