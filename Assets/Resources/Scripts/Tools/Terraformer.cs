using Biosearcher.InputHandling;
using Biosearcher.Planet.Managing;
using UnityEngine;

namespace Biosearcher.Tools
{
    public class Terraformer : MonoBehaviour
    {
        [SerializeField] float radius;
        [SerializeField] protected new Transform camera;
        [SerializeField] protected LayerMask terraformable;
        [SerializeField] protected float maxTerraformDistance;
        [SerializeField] protected ChunkManager landManager;
        [SerializeField] protected GameObject terraformSpherePrefab;

        protected TerraformerInput input;
        protected GameObject terraformSphere;

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
                terraformSphere.SetActive(_terraformPoint != null);
                terraformSphere.transform.position = _terraformPoint ?? default;
            }
        }

        protected void Awake()
        {
            input = new TerraformerInput(new Presenter(this));
            terraformSphere = Instantiate(terraformSpherePrefab);
            terraformSphere.transform.localScale = Vector3.one * radius * 2;
        }
        protected void OnDestroy() => input.OnDestroy();

        protected void OnEnable() => input.OnEnable();
        protected void OnDisable() => input.OnDisable();

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

        protected void Add()
        {
            if (TerraformPoint == null)
            {
                return;
            }
            landManager.TerraformAdd((Vector3)TerraformPoint, radius);
        }

        public class Presenter
        {
            public Terraformer Terraformer { get; }

            public Presenter(Terraformer player) => Terraformer = player;
            public void Add() => Terraformer.Add();
        }
    }
}
