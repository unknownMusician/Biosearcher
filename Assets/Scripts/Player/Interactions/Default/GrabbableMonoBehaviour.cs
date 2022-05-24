using System;
using AreYouFruits.Common;
using AreYouFruits.Common.ComponentGeneration;
using UnityEngine;

namespace Biosearcher.Player.Interactions.Default
{
    public sealed class GrabbableComponent : AbstractComponent<Grabbable>
    {
#nullable disable
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
#nullable enable

        protected override Grabbable Create() => new Grabbable(_rigidbody, _collider);
    }

    public class Grabbable : IGrabbable
    {
        public event Action? OnGrab;

        protected readonly Rigidbody Rigidbody;
        protected readonly Collider Collider;

        public Grabbable(Rigidbody rigidbody, Collider collider)
        {
            Rigidbody = rigidbody;
            Collider = collider;
        }

        public virtual void HandleGrab()
        {
            OnGrab?.Invoke();
            OnGrab = null;

            SetPhysicsActive(false);
        }

        public virtual void HandleDrop()
        {
            SetPhysicsActive(true);
        }

        protected virtual void SetPhysicsActive(bool isActive)
        {
            Rigidbody.isKinematic = !isActive;
            Collider.enabled = isActive;
        }
    }

    [RequireComponent(typeof(Rigidbody))]
    public class GrabbableMonoBehaviour : MonoBehaviour, IGrabbable
    {
        public event Action OnGrab;

        protected Rigidbody _rigidbody;
        protected Collider _collider;

        protected virtual void Awake()
        {
            gameObject.GetComponents(out _rigidbody, out _collider);
        }

        public virtual void HandleGrab()
        {
            OnGrab?.Invoke();
            OnGrab = null;

            SetPhysicsActive(false);
        }

        public virtual void HandleDrop()
        {
            SetPhysicsActive(true);
        }

        protected virtual void SetPhysicsActive(bool isActive)
        {
            _rigidbody.isKinematic = !isActive;
            _collider.enabled = isActive;
        }
    }
}
