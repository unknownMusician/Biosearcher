using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Player.Interactions
{
    public static class GrabbableExtensions
    {
        [NeedsRefactor]
        public static LayerMask GrabbedLayerIndex { get; } = LayerMask.NameToLayer("Grabbed");

        /// <summary>
        /// Layer setup.
        /// </summary>
        public static void InitializeDefaultLayer<TGrabbableBehaviour>(this TGrabbableBehaviour grabbableBehaviour)
            where TGrabbableBehaviour : MonoBehaviour, IGrabbable
        {
            grabbableBehaviour.DefaultLayer = grabbableBehaviour.gameObject.layer;
        }

        /// <summary>
        /// Layer setup.
        /// </summary>
        public static void HandleGrabDefault<TGrabbableBehaviour>(this TGrabbableBehaviour grabbableBehaviour)
            where TGrabbableBehaviour : MonoBehaviour, IGrabbable
        {
            grabbableBehaviour.gameObject.layer = GrabbedLayerIndex;
            grabbableBehaviour.OnGrab?.Invoke();
            grabbableBehaviour.OnGrab = null;
        }

        /// <summary>
        /// Layer setup.
        /// </summary>
        public static void HandleDropDefault<TGrabbableBehaviour>(this TGrabbableBehaviour grabbableBehaviour)
            where TGrabbableBehaviour : MonoBehaviour, IGrabbable
        {
            grabbableBehaviour.gameObject.layer = grabbableBehaviour.DefaultLayer;
        }
    }
}
