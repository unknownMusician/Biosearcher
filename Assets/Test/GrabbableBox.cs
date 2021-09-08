using Biosearcher.Common;
using Biosearcher.Player.Interactions;
using UnityEngine;

namespace Biosearcher.Test
{
    [RequireComponent(typeof(Rigidbody))]
    public class GrabbableBox : MonoBehaviour, IGrabbable
    {
        private Rigidbody _rigidbody;

        LayerMask IGrabbable.DefaultLayer { get; set; }

        private void Awake()
        {
            this.InitializeDefaultLayer();
            this.SetComponents(out _rigidbody);
        }

        public void HandleGrab()
        {
            this.HandleGrabDefault();
            _rigidbody.isKinematic = true;
        }

        public void HandleDrop()
        {
            this.HandleDropDefault();
            _rigidbody.isKinematic = false;
        }
    }
}