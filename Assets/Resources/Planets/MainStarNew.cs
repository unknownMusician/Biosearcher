using Biosearcher.Common;
using Biosearcher.Refactoring;
using System.Collections;
using UnityEngine;

namespace Biosearcher.Planets
{
    [NeedsRefactor]
    public sealed class MainStarNew : MonoBehaviour
    {
        public static MainStarNew Instance { get; private set; }

        [NeedsRefactor]
        private static readonly Vector3 RotationCenter = Vector3.zero;
        private static readonly Vector3 DefaultRotationAxis = Vector3.up;
        private static readonly Vector3 DefaultRandomDeltaVector = GetRandomDeltaVector(DefaultRotationAxis);

        [SerializeField] private float _rotationAnglePerSecond = 20f;
        [SerializeField] private float _rotationAngle = 0;
        [SerializeField] private float _rotationRadius = 300;
        [NeedsRefactor("Sync with Coordinates & Planet")]
        [SerializeField] private Vector3 _rotationAxis = DefaultRotationAxis;
        private bool _isAlive = true;

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
            _rotationAngle.MakeCycleDegrees();
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
        private void OnDestroy() => Instance = null;

        private void Start() => StartCoroutine(Rotating());

        private void SetTransform()
        {
            transform.position = Quaternion.AngleAxis(_rotationAngle, _rotationAxis) * RotationStartingVector * _rotationRadius;
        }

        private IEnumerator Rotating()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();

            yield return waitForFixedUpdate;
            while (_isAlive)
            {
                _rotationAngle += _rotationAnglePerSecond * UnityEngine.Time.deltaTime;
                _rotationAngle.MakeCycleDegrees();
                SetTransform();

                yield return waitForFixedUpdate;
            }
        }

        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(RotationCenter, _rotationAxis, _rotationRadius);
        }
    }
}