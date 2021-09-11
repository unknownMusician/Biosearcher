using Biosearcher.Player.Interactions;
using Biosearcher.Common;
using System;
using System.Collections;
using UnityEngine;
using Biosearcher.Refactoring;

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
        public event Action OnGrowStart;
        public event Action OnGrowEnd;


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

        public bool TryInsertIn(IInsertFriendly insertFriendly) => this.TryInsertInGeneric(insertFriendly);
        public bool TryAlignWith(IInsertFriendly insertFriendly) => this.TryAlignWithGeneric(insertFriendly);

        [NeedsRefactor("need to add seed handling by plant or garden bed on start of growth")]
        private void StartGrow() => OnGrowStart?.Invoke();
        
        [NeedsRefactor("need to add seed handling by plant or garden bed on end of growth"),]
        private void EndGrow() => OnGrowEnd?.Invoke();

        private IEnumerator GrowthProcess()
        {
            StartGrow();
            float time = 0f;
            while (time != 1f)
            {
                _meshRenderer.material.color = Color.Lerp(Color.green, Color.red, time);
                time += Time.deltaTime / _plantSettings.GrowthTime;
                yield return null;
            }
            EndGrow();
        }
    }
}
