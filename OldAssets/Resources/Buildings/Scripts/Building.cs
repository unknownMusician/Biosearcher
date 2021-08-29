using Biosearcher.Buildings.Resources.Interfaces;
using Biosearcher.Buildings.Settings;
using Biosearcher.Buildings.Types.Interfaces;
using Biosearcher.Player;
using Biosearcher.Common;
using System.Linq;
using UnityEngine;

namespace Biosearcher.Buildings
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class Building : MonoBehaviour, IGrabbable
    {
        private LayerMask _realMask;

        [SerializeField] protected LayerMask _groundMask;
        [SerializeField] protected float _minDistanceToGround;
        [SerializeField] protected BuildingsSettings _buildingsSettings;
        [Space]

        protected Rigidbody _rigidbody;
        protected Collider _collider;

        protected float _maxConnectRadius;
        protected float _cyclesPerSecond;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();

            LoadBuildingParameters(_buildingsSettings);
        }

        protected virtual void LoadBuildingParameters(BuildingsSettings buildingsSettings)
        {
            _maxConnectRadius = buildingsSettings.NetworkSettings.MaxConnectRadius;
            _cyclesPerSecond = buildingsSettings.NetworkSettings.CyclesPerSecond;
        }

        public void HandleGrab()
        {
            this.HandleGrabDefault(out _realMask);
            
            TryDisconnect();
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
        }
        public void HandleDrop()
        {
            this.HandleDropDefault(_realMask);

            if (Physics.OverlapSphere(transform.position, _minDistanceToGround, _groundMask).Length == 0)
            {
                _rigidbody.isKinematic = false;
            }
            _collider.enabled = true;
            TryConnect();
        }

        protected abstract void TryConnect();
        protected abstract void TryDisconnect();

        protected void TryConnect<TResource>(IResourceMover<TResource> resourceMover) where TResource : IResource<TResource>, new()
        {
            Physics.OverlapSphere(transform.position, _maxConnectRadius)
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