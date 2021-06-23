using Biosearcher.InputHandling;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.PlayerTools
{
    public class Terraformer : MonoBehaviour
    {
        #region Properties

        [SerializeField] protected float _radius;
        [SerializeField] protected Transform _camera;
        [SerializeField] protected LayerMask _terraformable;
        [SerializeField] protected float _maxTerraformDistance;
        [SerializeField] protected GameObject _terraformSpherePrefab;

        protected TerraformerInput _input;
        protected GameObject _terraformSphere;

        protected Vector3? _terraformPoint;
        
        protected Vector3? TerraformPoint
        {
            get => _terraformPoint;
            set
            {
                if (_terraformPoint == value)
                {
                    return;
                }
                _terraformPoint = value;
                _terraformSphere.SetActive(_terraformPoint != null);
                _terraformSphere.transform.position = _terraformPoint ?? default;
            }
        }

        #endregion

        #region MonoBehaviour methods

        protected void Awake()
        {
            _input = new TerraformerInput(new Presenter(this));
            _terraformSphere = Instantiate(_terraformSpherePrefab);
            _terraformSphere.transform.localScale = Vector3.one * _radius * 2;
        }
        protected void OnDestroy() => _input.Dispose();

        protected void OnEnable() => _input.OnEnable();
        protected void OnDisable() => _input.OnDisable();

        protected void Update()
        {
            var lookRay = new Ray(_camera.position, _camera.forward);
            var hits = new RaycastHit[1];
            Physics.RaycastNonAlloc(lookRay, hits, _maxTerraformDistance, _terraformable);

            if (hits.Length == 0)
            {
                TerraformPoint = null;
                return;
            }
            TerraformPoint = hits[0].point;
        }
        
        #endregion

        #region Methods

        [NeedsRefactor(Needs.Remove)]
        protected void Add()
        {
            if (TerraformPoint == null) return;
            
            // todo
            //landManager.TerraformAdd((Vector3)TerraformPoint, radius);
        }

        #endregion

        #region Classes

        public class Presenter
        {
            public Terraformer Terraformer { get; }

            public Presenter(Terraformer player) => Terraformer = player;
            public void Add() => Terraformer.Add();
        }

        #endregion
    }
}
