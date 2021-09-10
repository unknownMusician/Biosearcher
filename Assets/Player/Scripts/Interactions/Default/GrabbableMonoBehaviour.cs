using Biosearcher.Common;
using System;
using UnityEngine;

namespace Biosearcher.Player.Interactions.Default
{
    [RequireComponent(typeof(Rigidbody))]
    public class GrabbableMonoBehaviour : MonoBehaviour, IGrabbable
    {
        protected Rigidbody _rigidbody;
        protected Collider _collider;

        LayerMask IGrabbable.DefaultLayer { get; set; }
        Action IGrabbable.OnGrab { get; set; }

        protected virtual void Awake()
        {
            this.InitializeDefaultLayer();
            this.SetComponents(out _rigidbody, out _collider);
        }

        public virtual void HandleGrab()
        {
            this.HandleGrabDefault();
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
        }

        public virtual void HandleDrop()
        {
            this.HandleDropDefault();
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
        }
    }
}