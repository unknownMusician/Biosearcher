using Biosearcher.InputHandling;
using UnityEngine;

namespace Biosearcher.Tools
{
    public class Terraformer : MonoBehaviour
    {
        #region Properties

        [SerializeField] float radius;
        [SerializeField] protected new Transform camera;
        [SerializeField] protected LayerMask terraformable;
        [SerializeField] protected float maxTerraformDistance;
        [SerializeField] protected GameObject terraformSpherePrefab;

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
            _terraformSphere = Instantiate(terraformSpherePrefab);
            _terraformSphere.transform.localScale = Vector3.one * radius * 2;
        }
        protected void OnDestroy() => _input.Dispose();

        protected void OnEnable() => _input.OnEnable();
        protected void OnDisable() => _input.OnDisable();

        protected void Update()
        {

            Ray lookRay = new Ray(camera.position, camera.forward);
            RaycastHit[] hits = Physics.RaycastAll(lookRay, maxTerraformDistance, terraformable);

            if (hits.Length == 0)
            {
                TerraformPoint = null;
                return;
            }
            TerraformPoint = hits[0].point;
        }
        
        #endregion
        
        protected void Add()
        {
            if (TerraformPoint == null) return;
            
            // todo
            //landManager.TerraformAdd((Vector3)TerraformPoint, radius);
        }

        public class Presenter
        {
            public Terraformer Terraformer { get; }

            public Presenter(Terraformer player) => Terraformer = player;
            public void Add() => Terraformer.Add();
        }
    }
}
