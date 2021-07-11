using Biosearcher.Common;
using Biosearcher.Planets.Orientation;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Player
{
    [RequireComponent(typeof(PlanetTransform))]
    [NeedsRefactor]
    public class WheelAnimator : MonoBehaviour
    {
        [SerializeField] private Vector2 _wheelOffset;
        [SerializeField] private GameObject _wheelPrefab;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField][Min(0.01f)] private float _wheelRadius;
        [SerializeField] private Range<float> _suspension;

        private Vector3 _lastPosition;

        private readonly Transform[,] _wheels = new Transform[2, 3];

        private void Awake()
        {
            ForeachWheel((wheel, position) =>
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
            ForeachWheel((wheel, position) => wheel.localPosition = GetLocalPositionXZ(position).ReProjectedXZ(GetLocalHeight(position)));
            ForeachWheel((wheel, position) => wheel.localRotation = Quaternion.Euler(angle, 0f, 0f) * wheel.localRotation);
            _lastPosition = transform.position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            ForeahWheelPosition((position, index) => Gizmos.DrawLine(transform.position + transform.rotation * GetLocalPosition(position).DroppedY(-_suspension.Min), transform.position + transform.rotation * GetLocalPosition(position).DroppedY(-_suspension.Max)));
            Gizmos.color = Color.blue;
            ForeahWheelPosition((position, index) => Gizmos.DrawWireSphere(transform.position + transform.rotation * GetLocalPosition(position), _wheelRadius));
        }

        private Vector3 GetLocalPosition((int x, int y) position) => GetLocalPositionXZ(position).ReProjectedXZ(GetLocalHeight(position));
        private float GetLocalHeight((int x, int y) position) => GetLocalHeight(GetLocalPositionXZ(position));
        private float GetLocalHeight(Vector2 localPositionXZ)
        {
            Vector3 down = -transform.up;
            Vector3 raycastSource = transform.position + transform.rotation * localPositionXZ.ReProjectedXZ(-_suspension.Min);
            if (Physics.Raycast(raycastSource, down, out RaycastHit hit, _suspension.Max + _wheelRadius, _groundMask))
            {
                return _wheelRadius - (raycastSource - hit.point).magnitude;
            }
            return -_suspension.Max;
        }
        private Vector2 GetLocalPositionXZ((int x, int y) position)
        {
            return new Vector2(position.x * _wheelOffset.x, position.y * _wheelOffset.y);
        }

        private void ForeachWheel(System.Func<Transform, (int x, int y), Transform> func)
        {
            ForeahWheelPosition((position, index) => _wheels[index.x, index.y] = func(_wheels[index.x, index.y], position));
        }
        private void ForeachWheel(System.Action<Transform, (int x, int y)> action)
        {
            ForeahWheelPosition((position, index) => action(_wheels[index.x, index.y], position));
        }
        private void ForeahWheelPosition(System.Action<(int x, int y), (int x, int y)> action)
        {
            for (int xIndex = 0; xIndex < 2; xIndex++)
            {
                for (int yIndex = 0; yIndex < 3; yIndex++)
                {
                    int x = (xIndex * 2) - 1;
                    int y = yIndex - 1;
                    action((x, y), (xIndex, yIndex));
                }
            }
        }
    }
}
