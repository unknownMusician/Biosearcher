using Biosearcher.Player.Interactions;
using Biosearcher.Common;
using System;
using UnityEngine;

namespace Biosearcher.Plants
{
    public sealed class Seed : MonoBehaviour, IInsertable
    {
        [SerializeField] private PlantSettings _plantSettings;

        private Rigidbody _rigidbody;
        private Collider _collider;

        LayerMask IGrabbable.DefaultLayer { get; set; }
        Action IGrabbable.OnGrab { get; set; }

        public PlantSettings PlantSettings => _plantSettings;

        private void Awake()
        {
            this.InitializeDefaultLayer();
            this.SetComponents(out _rigidbody, out _collider);
        }

        public void HandleGrab()
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
        public void HandleInsert() => this.HandleInsertDefault();

        public bool TryInsertIn(IInsertFriendly insertFriendly) => this.TryInsertInGeneric(insertFriendly);
        public bool TryAlignWith(IInsertFriendly insertFriendly) => this.TryAlignWithGeneric(insertFriendly);
    }
}
