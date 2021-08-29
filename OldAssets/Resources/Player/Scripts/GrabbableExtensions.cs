using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Player
{
    public static class GrabbableExtensions
    {
        [NeedsRefactor]
        public static LayerMask GrabbableMask { get; private set; } = LayerMask.NameToLayer("Grabbable");

        public static void HandleGrabDefault<TGrabbable>(this TGrabbable grabbable, out LayerMask realMask)
            where TGrabbable : MonoBehaviour, IGrabbable
        {
            realMask = HandleGrabDefault(grabbable);
        }
        public static LayerMask HandleGrabDefault<TGrabbable>(this TGrabbable grabbable)
            where TGrabbable : MonoBehaviour, IGrabbable
        {
            LayerMask realMask = grabbable.gameObject.layer;
            grabbable.gameObject.layer = GrabbableMask;
            return realMask;
        }

        public static void HandleDropDefault<TGrabbable>(this TGrabbable grabbable, LayerMask realMask)
            where TGrabbable : MonoBehaviour, IGrabbable
        {
            grabbable.gameObject.layer = realMask;
        }
    }
}
