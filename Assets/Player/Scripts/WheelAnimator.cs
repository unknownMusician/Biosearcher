using Biosearcher.Common;
using Biosearcher.Common.States;
using Biosearcher.Refactoring;
using System;
using UnityEngine;

namespace Biosearcher.Player
{
    [NeedsRefactor]
    public sealed class WheelAnimator : MonoBehaviour
    {
        [SerializeField] private Vector2 _wheelOffset = new Vector2(0.55f, 0.58f);
        [SerializeField] private GameObject _wheelPrefab;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] [Min(0.01f)] private float _wheelRadius = 0.2f;
        [SerializeField] private Range<float> _suspension = new Range<float>(0, 1);

        private Vector3 _lastPosition;

        private readonly Wheel[,] _wheels = new Wheel[2, 3];

        public Wheel[,] Wheels => (Wheel[,])_wheels.Clone();
        public Range<float> Suspension => _suspension;
        public LayerMask GroundMask => _groundMask;
        public bool AtLeastOneWeelTouchesGround
        {
            get
            {
                bool isOnGround = false;
                ForeachWheel((_, __, wheel) => isOnGround |= wheel.IsOnGround);
                return isOnGround;
            }
        }

        [NeedsRefactor]
        private void OnValidate()
        {
            if (Application.isPlaying && _wheels[0, 0] != null)
            {
                ForeachWheel((_, __, wheel) =>
                {
                    wheel.GroundMask = _groundMask;
                    wheel.WheelRadius = _wheelRadius;
                    wheel.Suspension = _suspension;
                });
            }
        }

        [NeedsRefactor("remove GetChild(0)")]
        private void Awake()
        {
            ForeachWheel((_, position, __) =>
            {
                Transform wheelTransform = Instantiate(_wheelPrefab, transform).transform;
                wheelTransform.localPosition = GetLocalPositionXZ(position).ReProjectedXZ();

                ParticleSystem particles = wheelTransform.GetChild(0).GetComponent<ParticleSystem>();
                return Wheel.Create(wheelTransform, particles, _groundMask, _wheelRadius, _suspension);
            });
        }

        private void OnDestroy()
        {
            ForeachWheel((_, __, wheel) =>
            {
                Destroy(wheel.Transform);
                wheel.Dispose();
            });
        }

        private void FixedUpdate()
        {
            Vector3 deltaPosition = transform.position - _lastPosition;
            float speed = (Quaternion.Inverse(transform.rotation) * deltaPosition).z;
            float angle = speed / _wheelRadius * Mathf.Rad2Deg;
            ForeachWheel((_, position, wheel) => wheel.Move(GetLocalPositionXZ(position), angle));
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
        private Vector3 GetLocalPosition((int x, int y) position, out float localHeight) => GetLocalPosition(GetLocalPositionXZ(position), out localHeight);
        private Vector3 GetLocalPosition(Vector2 localPositionXZ, out float localHeight)
        {
            localHeight = GetLocalHeight(localPositionXZ);
            return localPositionXZ.ReProjectedXZ(localHeight);
        }
        [NeedsRefactor(Needs.Remove), Obsolete]
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

        private void ForeachWheel(Func<(int xi, int yi), (int xp, int yp), Wheel, Wheel> func)
        {
            ForeachWheel((index, position, transform) => { _wheels[index.xi, index.yi] = func(index, position, transform); });
        }
        private void ForeachWheel(Action<(int xi, int yi), (int xp, int yp), Wheel> action)
        {
            ForeachWheel((index, position) => action(index, position, _wheels[index.xi, index.yi]));
        }
        private void ForeachWheel(Action<(int xi, int yi), (int xp, int yp)> action)
        {
            ForeachWheel(((int xi, int yi) indices) =>
            {
                int x = (indices.xi * 2) - 1;
                int y = indices.yi - 1;
                action(indices, (x, y));
            });
        }
        private void ForeachWheel(Action<(int xi, int yi)> action)
        {
            for (int xIndex = 0; xIndex < 2; xIndex++)
            {
                for (int yIndex = 0; yIndex < 3; yIndex++)
                {
                    action((xIndex, yIndex));
                }
            }
        }
    }

    public sealed class Wheel : IDisposable
    {
        private ChangeableStateManager<WalkerState> _state;

        public Transform Transform { get; }
        public ParticleSystem Particles { get; }

        public LayerMask GroundMask { get; internal set; }
        public float WheelRadius { get; internal set; }
        public Range<float> Suspension { get; internal set; }

        public bool IsOnGround { get; private set; }

        private Wheel(Transform transform, ParticleSystem particles, LayerMask groundMask, float wheelRadius, Range<float> suspension)
        {
            Transform = transform;
            Particles = particles;
            GroundMask = groundMask;
            WheelRadius = wheelRadius;
            Suspension = suspension;
        }

        internal static Wheel Create(Transform transform, ParticleSystem particles, LayerMask groundMask, float wheelRadius, Range<float> suspension)
        {
            var wheel = new Wheel(transform, particles, groundMask, wheelRadius, suspension);
            wheel.RegisterStates();

            return wheel;
        }

        [NeedsRefactor]
        private void HandleStateChange(WalkerState newState)
        {
            ParticleSystem.EmissionModule emission = Particles.emission;

            emission.rateOverDistance = newState switch
            {
                WalkerState.InAirState => 0,
                WalkerState.OnGroundState => 2,
                _ => 0
            };
        }

        private void RegisterStates()
        {
            _state = new ChangeableStateManager<WalkerState>();

            _state.Register(WalkerState.OnGroundState)
                .Register<float>(RotateWheel, RotateWheel)
                .Register<Vector3, Vector3, float>(CalculateLocalHeight, CalculateLocalHeight);

            _state.Register(WalkerState.InAirState)
                .Register<float>(RotateWheel, null)
                .Register<Vector3, Vector3, float>(CalculateLocalHeight, CalculateLocalHeightInAir);

            _state.OnStateChange += HandleStateChange;
        }

        private void RotateWheel(float angle)
        {
            Transform.localRotation = Quaternion.Euler(angle, 0f, 0f) * Transform.localRotation;
        }

        internal void Move(Vector2 localPositionXZ, float angle)
        {
            Transform.localPosition = GetLocalPosition(localPositionXZ, out float _);
            _state.Active.Invoke(RotateWheel, angle);
        }
        private Vector3 GetLocalPosition(Vector2 localPositionXZ, out float localHeight)
        {
            localHeight = GetLocalHeight(localPositionXZ);
            return localPositionXZ.ReProjectedXZ(localHeight);
        }
        private float GetLocalHeight(Vector2 localPositionXZ)
        {
            Transform parentTransform = Transform.parent;
            Vector3 down = -parentTransform.up;
            Vector3 raycastSource = parentTransform.position + parentTransform.rotation * localPositionXZ.ReProjectedXZ(-Suspension.Min);
            IsOnGround = Physics.Raycast(raycastSource, down, out RaycastHit hit, Suspension.Max - Suspension.Min + WheelRadius, GroundMask);

            _state.TryChange(IsOnGround ? WalkerState.OnGroundState : WalkerState.InAirState);
            return _state.Active.Invoke(CalculateLocalHeight, raycastSource, hit.point);
        }
        private float CalculateLocalHeight(Vector3 raycastSource, Vector3 hitPoint)
        {
            return WheelRadius - (raycastSource - hitPoint).magnitude - Suspension.Min;
        }
        private float CalculateLocalHeightInAir(Vector3 raycastSource, Vector3 hitPoint)
        {
            return -Suspension.Max;
        }

        public void Dispose() => _state.Dispose();
    }
}
