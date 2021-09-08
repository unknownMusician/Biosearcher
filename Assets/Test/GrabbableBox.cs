using Biosearcher.Common;
using Biosearcher.Player.Interactions;
using System;
using UnityEngine;

namespace Biosearcher.Test
{
    [RequireComponent(typeof(Rigidbody))]
    public class GrabbableBox : MonoBehaviour, IGrabbable
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

        public void HandleDrop()
        {
            this.HandleDropDefault();
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
        }
    }
}