using Biosearcher.Common;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Planets
{
    [ExecuteAlways]
    [NeedsRefactor]
    public sealed class MainStar : MonoBehaviour
    {
        public static MainStar Instance { get; private set; }

        [NeedsRefactor]
        private static readonly Vector3 RotationCenter = Vector3.zero;
        private static readonly Vector3 DefaultRotationAxis = Vector3.up;
        private static readonly Vector3 DefaultRandomDeltaVector = GetRandomDeltaVector(DefaultRotationAxis);

        [SerializeField] private float _rotationAnglePerSecond = 20f;
        [SerializeField] private float _rotationAngle = 0;
        [SerializeField] private float _rotationRadius = 300;
        [NeedsRefactor("Sync with Coordinates & Planet")]
        [SerializeField] private Vector3 _rotationAxis = DefaultRotationAxis;
        [SerializeField] private Material _skyMaterial;
        [SerializeField] private Material _atmosphereMaterial;

        private Vector3 _randomDeltaVector = DefaultRandomDeltaVector;

        public Vector3 RotationStartingVector { get; private set; } = GetRotationStartingVector(DefaultRandomDeltaVector, DefaultRotationAxis);
        public float RotationAngle => _rotationAngle;
        public Vector3 RotationAxis => _rotationAxis;

        private static Vector3 GetRotationStartingVector(Vector3 randomDeltaVector, Vector3 rotationAxis)
        {
            return Vector3.ProjectOnPlane(randomDeltaVector, rotationAxis).normalized;
        }
        private static Vector3 GetRandomDeltaVector(Vector3 rotationAxis)
        {
            return Quaternion.Euler(10, 10, 10) * rotationAxis;
        }

        private void OnValidate()
        {
            _randomDeltaVector = GetRandomDeltaVector(_rotationAxis);
            RotationStartingVector = GetRotationStartingVector(_randomDeltaVector, _rotationAxis);
            _rotationAngle.MakeCycleDegrees360();
            SetTransform();
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There can only be one instance of MainStarNew on the scene.");
            }
            Instance = this;
        }
        private void OnDestroy()
        {
            Instance = null;
            _skyMaterial.SetVector("MainStarDirection", Vector3.zero);
            _atmosphereMaterial.SetVector("MainStarDirection", Vector3.zero);
        }

        private void Update()
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, -transform.position);
            _skyMaterial.SetVector("MainStarDirection", transform.position.normalized);
            _atmosphereMaterial.SetVector("MainStarDirection", transform.position.normalized);
        }

        private void FixedUpdate()
        {
            _rotationAngle += _rotationAnglePerSecond * UnityEngine.Time.deltaTime;
            _rotationAngle.MakeCycleDegrees360();
            SetTransform();
        }

        private void SetTransform()
        {
            transform.position = Quaternion.AngleAxis(_rotationAngle, _rotationAxis) * RotationStartingVector * _rotationRadius;
        }

        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(RotationCenter, _rotationAxis, _rotationRadius);
        }
    }
}