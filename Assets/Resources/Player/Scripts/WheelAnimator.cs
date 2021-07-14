using Biosearcher.Common;
using Biosearcher.Planets.Orientation;
using Biosearcher.Refactoring;
using System;
using UnityEngine;

namespace Biosearcher.Player
{
    [RequireComponent(typeof(PlanetTransform))]
    [NeedsRefactor]
    public sealed class WheelAnimator : MonoBehaviour
    {
        [SerializeField] private Vector2 _wheelOffset = new Vector2(0.55f, 0.58f);
        [SerializeField] private GameObject _wheelPrefab;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] [Min(0.01f)] private float _wheelRadius = 0.2f;
        [SerializeField] private Range<float> _suspension = new Range<float>(0, 1);

        private Vector3 _lastPosition;

        private readonly Transform[,] _wheels = new Transform[2, 3];

        public Transform[,] Wheels => (Transform[,])_wheels.Clone();
        public Range<float> Suspension => _suspension;
        public LayerMask GroundMask => _groundMask;

        private void Awake()
        {
            ForeachWheel((_, position, wheel) =>
            {
                Transform wheelTransform = Instantiate(_wheelPrefab, transform).transform;
                wheelTransform.localPosition = GetLocalPositionXZ(position).ReProjectedXZ();
                return wheelTransform;
            });
        }

        private void FixedUpdate()
        {
            Vector3 deltaPosition = transform.position - _lastPosition;
            float speed = (Quaternion.Inverse(transform.rotation) * deltaPosition).z;
            float angle = speed / _wheelRadius * Mathf.Rad2Deg;
            WheelHeights = new float[2, 3];
            ForeachWheel((indices, position, wheel) =>
            {
                wheel.localPosition = GetLocalPosition(position, out WheelHeights[indices.xi, indices.yi]);
                wheel.localRotation = Quaternion.Euler(angle, 0f, 0f) * wheel.localRotation;
            });
            _lastPosition = transform.position;
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 transformPosition = transform.position;
            Quaternion transformRotation = transform.rotation;
            ForeachWheel((index, position) =>
            {
                Vector3 localPosition = GetLocalPosition(position);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(
                    transformPosition + transformRotation * localPosition.DroppedY(-_suspension.Min),
                    transformPosition + transformRotation * localPosition.DroppedY(-_suspension.Max));
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transformPosition + transformRotation * localPosition, _wheelRadius);
            });
        }

        private Vector3 GetLocalPosition((int x, int y) position) => GetLocalPosition(position, out _);
        private Vector3 GetLocalPosition((int x, int y) position, out float localHeight)
        {
            Vector2 localPositionXZ = GetLocalPositionXZ(position);
            localHeight = GetLocalHeight(localPositionXZ);
            return localPositionXZ.ReProjectedXZ(localHeight);
        }
        [NeedsRefactor(Needs.Remove), System.Obsolete]
        private float GetLocalHeight((int x, int y) position) => GetLocalHeight(GetLocalPositionXZ(position));
        private float GetLocalHeight(Vector2 localPositionXZ)
        {
            Vector3 down = -transform.up;
            Vector3 raycastSource = transform.position + transform.rotation * localPositionXZ.ReProjectedXZ(-_suspension.Min);
            if (Physics.Raycast(raycastSource, down, out RaycastHit hit, _suspension.Max - _suspension.Min + _wheelRadius, _groundMask))
            {
                return _wheelRadius - (raycastSource - hit.point).magnitude - _suspension.Min;
            }
            return -_suspension.Max;
        }
        private Vector2 GetLocalPositionXZ((int x, int y) position)
        {
            return new Vector2(position.x * _wheelOffset.x, position.y * _wheelOffset.y);
        }

        private void ForeachWheel(System.Func<(int xi, int yi), (int xp, int yp), Transform, Transform> func)
        {
            ForeachWheel((index, position, transform) => { _wheels[index.xi, index.yi] = func(index, position, transform); });
        }
        private void ForeachWheel(System.Action<(int xi, int yi), (int xp, int yp), Transform> action)
        {
            ForeachWheel((index, position) => action(index, position, _wheels[index.xi, index.yi]));
        }
        private void ForeachWheel(System.Action<(int xi, int yi), (int xp, int yp)> action)
        {
            ForeachWheel(((int xi, int yi) indices) =>
             {
                 int x = (indices.xi * 2) - 1;
                 int y = indices.yi - 1;
                 action(indices, (x, y));
             });
        }
        private void ForeachWheel(System.Action<(int xi, int yi)> action)
        {
            for (int xIndex = 0; xIndex < 2; xIndex++)
            {
                for (int yIndex = 0; yIndex < 3; yIndex++)
                {
                    int x = (xIndex * 2) - 1;
                    int y = yIndex - 1;
                    action((xIndex, yIndex));
                }
            }
        }
    }
}
