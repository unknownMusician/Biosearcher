using Biosearcher.Buildings.Resources;
using Biosearcher.Buildings.Resources.Interfaces;
using Biosearcher.Buildings.Types.Interfaces;
using Biosearcher.PlayerBehaviour;
using Biosearcher.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Biosearcher.Buildings
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class Building : MonoBehaviour, IGrabbable
    {
        [SerializeField] protected LayerMask _groundMask;
        [SerializeField] protected float _minDistanceToGround;

        protected Rigidbody _rigidbody;
        protected Collider _collider;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public void Drop()
        {
            if (Physics.OverlapSphere(transform.position, _minDistanceToGround, _groundMask).Length == 0)
            {
                _rigidbody.isKinematic = false;
            }
            _collider.enabled = true;
            TryConnect();
        }

        public void Grab()
        {
            TryDisconnect();
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
        }

        protected abstract void TryConnect();
        protected abstract void TryDisconnect();

        protected void TryConnect<TResource>(IResourceMover<TResource> resourceMover) where TResource : IResource<TResource>, new()
        {
            Physics.OverlapSphere(transform.position, Network<TResource>.MaxConnectRadius)
                .Select(collider => collider.GetComponent<IResourceMover<TResource>>())
                .Where(mover => mover != null)
                .Foreach(networkMember => networkMember.Network.Connection.TryAdd(resourceMover, networkMember));
        }
        protected void TryDisconnect<TResource>(IResourceMover<TResource> resourceMover) where TResource : IResource<TResource>, new()
        {
            resourceMover.Network.Connection.TryRemove(resourceMover);
        }
    }
}