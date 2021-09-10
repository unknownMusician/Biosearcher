using Biosearcher.Player.Interactions;
using Biosearcher.Common;
using System;
using System.Collections;
using UnityEngine;

namespace Biosearcher.Test
{
    public sealed class Seed : MonoBehaviour, IInsertable
    {
        [SerializeField] private PlantSettings _plantSettings;

        public PlantSettings PlantSettings => _plantSettings;
        private Rigidbody _rigidbody;
        private Collider _collider;
        private MeshRenderer _meshRenderer;

        LayerMask IGrabbable.DefaultLayer { get; set; }
        Action IGrabbable.OnGrab { get; set; }
        Action OnGrowStart { get; set; }


        private void Awake()
        {
            this.InitializeDefaultLayer();
            this.SetComponents(out _rigidbody, out _collider, out _meshRenderer);
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
        public void HandleInsert()
        {
            this.HandleInsertDefault();

            StartCoroutine(GrowthProcess());
        }
        
        public bool TryInsertIn(IInsertFriendly insertFriendly)
        {
            return this.TryInsertInGeneric(insertFriendly);
        }
        public bool TryAlignWith(IInsertFriendly insertFriendly)
        {
            return this.TryAlignWithGeneric(insertFriendly);
        }

        private void StartGrow()
        {
            print("seed has planted");
            OnGrowStart?.Invoke();
        }
        private void EndGrow()
        {
            print("Growth finished");
        }

        private IEnumerator GrowthProcess()
        {
            StartGrow();
            float time = 0;
            while (_meshRenderer.material.color != Color.red)
            {
                _meshRenderer.material.color = Color.Lerp(Color.green, Color.red, time / _plantSettings.GrowthTime);
                time += Time.deltaTime;
                yield return null;
            }
            EndGrow();
        }
    }
}
