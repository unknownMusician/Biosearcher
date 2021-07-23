using Biosearcher.Common;
using Biosearcher.Common.States;
using Biosearcher.InputHandling;
using Biosearcher.Planets.Orientation;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Player
{
    [RequireComponent(typeof(WheelAnimator))]
    [NeedsRefactor]
    public sealed class Walker : MonoBehaviour
    {
        #region Properties

        [Header("Moving - Tangent")]
        [SerializeField] private float _tangentAcceleration = 100;
        [SerializeField] private float _maxSpeed = 20;
        [SerializeField] private float _tangentDamp = 10;
        [Header("Moving - Normal")]
        [SerializeField] private float _normalAcceleration = 20;
        [SerializeField] private float _maxRotationSpeed = 10;
        [SerializeField] private float _normalDamp = 10;
        [Header("Stabilizer")]
        [SerializeField] private float _springTangentAcceleration = 1000;
        [SerializeField] private float _springTangentDamp = 10;
        [SerializeField] private float _springNormalAcceleration = 200;
        [SerializeField] private float _springNormalDamp = 10;
        [Header("Other")]
        [SerializeField] private PlayerCamera _camera;

        private PlanetTransform _planetTransform;
        private Rigidbody _rigidbody;
        private WheelAnimator _animator;
        private PlayerInput _input;

        private ChangeableStateManager<WalkerState> _state;

        private RaycastHit _hitInfo;

        private Vector3? _lastFramePositionRelativeToDesiredPosition;

        private Vector3? _desiredPosition;
        private Vector3 _downDirection;

        private float GroundDesiredHeight => GroundCheckHeight * (3f / 4f);
        private float GroundCheckHeight => _animator.Suspension.Max;
        private LayerMask GroundMask => _animator.GroundMask;
        public StateHook<WalkerState> StateHook => _state.Hook;

        private float TangentAcceleration { get; set; }
        private float NormalAcceleration { get; set; }

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            RegisterStates();

            this.GetComponents(out _planetTransform, out _rigidbody, out _animator);

            _input = new PlayerInput(new Presenter(this));
        }
        private void OnDestroy()
        {
            _input.Dispose();
            _state.Dispose();
        }

        private void OnEnable() => _input.OnEnable();
        private void OnDisable() => _input.OnDisable();

        private void FixedUpdate()
        {
            _downDirection = -transform.up.normalized;

            _state.TryChange(_animator.AtLeastOneWeelTouchesGround ? WalkerState.OnGroundState : WalkerState.InAirState);

            _state.Active.Invoke(StabilizeTangent);
            _state.Active.Invoke(StabilizeNormal);
            _state.Active.Invoke(Move);
        }

        private void OnDrawGizmos() => _state.Active.Invoke(DrawGizmos);

        #endregion

        #region Methods

        private void RegisterStates()
        {
            _state = new ChangeableStateManager<WalkerState>();

            _state.Register(WalkerState.OnGroundState)
                .Register(StabilizeTangent, StabilizeTangent)
                .Register(StabilizeNormal, StabilizeNormal)
                .Register(Move, Move)
                .Register(DrawGizmos, DrawGizmos);

            _state.Register(WalkerState.InAirState)
                .Register(StabilizeTangent, () => _desiredPosition = null)
                .Register(StabilizeNormal, null)
                .Register(Move, null)
                .Register(DrawGizmos, null);

            _state.TryChange(WalkerState.InAirState);
        }

        private bool RaycastLocalDown() => Physics.Raycast(transform.position, _downDirection, out _hitInfo, GroundCheckHeight, GroundMask);

        private void DrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_desiredPosition.Value, 0.1f);
        }

        [NeedsRefactor("Make Height check based on wheels, not on overall height")]
        private void StabilizeTangent()
        {
            Vector3 hitSum = Vector3.zero;
            _animator.Wheels.For(wheel => hitSum += wheel.Transform.position);
            Vector3 hitPoint = hitSum * (1f / 6f);
            _desiredPosition = hitPoint - _downDirection * GroundDesiredHeight;

            Vector3 positionRelativeToDesiredPosition = transform.position - _desiredPosition.Value;
            Vector3 tangentVelocityRelativeToDesiredPosition;
            if (_lastFramePositionRelativeToDesiredPosition != null)
            {
                tangentVelocityRelativeToDesiredPosition = (_lastFramePositionRelativeToDesiredPosition.Value - positionRelativeToDesiredPosition) / Time.deltaTime;
            }
            else
            {
                tangentVelocityRelativeToDesiredPosition = Vector3.zero;
            }
            _lastFramePositionRelativeToDesiredPosition = positionRelativeToDesiredPosition;

            Vector3 tangentDistance = -_downDirection * GroundDesiredHeight - (transform.position - hitPoint);
            Vector3 tangentAcceleration = (tangentDistance.magnitude * tangentDistance.magnitude) * _springTangentAcceleration * tangentDistance.normalized;
            Vector3 tangentDamping = _springTangentDamp * tangentVelocityRelativeToDesiredPosition.magnitude * -_rigidbody.velocity.normalized;
            _rigidbody.velocity += (tangentAcceleration + tangentDamping) * Time.deltaTime;
        }

        [NeedsRefactor]
        private void StabilizeNormal()
        {
            Vector3[,] wheelPositions = new Vector3[2, 3];
            Wheel[,] wheels = _animator.Wheels;

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    wheelPositions[x, y] = wheels[x, y].Transform.position;
                }
            }

            Vector3 leftAverage = (wheelPositions[0, 0] + wheelPositions[0, 1] + wheelPositions[0, 2]) * (1f / 3f);
            Vector3 rightAverage = (wheelPositions[1, 0] + wheelPositions[1, 1] + wheelPositions[1, 2]) * (1f / 3f);

            Vector3 forwardAverage = (wheelPositions[0, 2] + wheelPositions[1, 2]) * 0.5f;
            Vector3 backAverage = (wheelPositions[0, 0] + wheelPositions[1, 0]) * 0.5f;

            Vector3 normal = Vector3.Cross(((leftAverage + rightAverage) * 0.5f + forwardAverage) * 0.5f - (leftAverage + backAverage) * 0.5f,
                (rightAverage + backAverage) * 0.5f - (leftAverage + backAverage) * 0.5f).normalized;

            Quaternion deltaRotation = transform.rotation.To(GetDesiredRotation(normal));

            deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);

            axis = float.IsInfinity(axis.x) ? Vector3.zero : axis.normalized;

            angle.MakeCycleDegrees180();

            Vector3 normalAcceleration = Mathf.Deg2Rad * angle * _springNormalAcceleration * axis;
            Vector3 normalDamping = -_rigidbody.angularVelocity * _springNormalDamp;

            _rigidbody.angularVelocity += (normalAcceleration + normalDamping) * Time.deltaTime;
        }

        private Quaternion GetDesiredRotation(Vector3 normal)
        {
            Quaternion rotation = _planetTransform.PlanetRotation;

            Vector3 hitNormalLocalPositionRotated = Quaternion.Euler(0f, -rotation.eulerAngles.y, 0f) * _planetTransform.ToPlanet(normal);
            Vector3 hitNormalRotationAngles = Quaternion.FromToRotation(Vector3.up, hitNormalLocalPositionRotated).eulerAngles;

            rotation = Quaternion.Euler(hitNormalRotationAngles.x, rotation.eulerAngles.y, hitNormalRotationAngles.z);

            return _planetTransform.ToUniverse(rotation);
        }

        private void Move()
        {
            Rigidbody playerRigidbody = _rigidbody;
            float deltaTime = Time.deltaTime;

            if (playerRigidbody.velocity.magnitude < _maxSpeed)
            {
                Vector3 acceleration = TangentAcceleration * _tangentAcceleration * transform.forward;
                playerRigidbody.velocity += acceleration * deltaTime;
            }

            Vector3 tangentDamping = -playerRigidbody.velocity * _tangentDamp;
            playerRigidbody.velocity += tangentDamping * deltaTime;

            if (playerRigidbody.angularVelocity.magnitude < _maxRotationSpeed)
            {
                Vector3 acceleration = _planetTransform.ToUniverse(NormalAcceleration * _normalAcceleration * Vector3.up);
                playerRigidbody.angularVelocity += acceleration * deltaTime;
            }

            Vector3 normalDamping = -playerRigidbody.angularVelocity * _normalDamp;
            playerRigidbody.angularVelocity += normalDamping * deltaTime;
        }

        #endregion

        #region Classes

        public class Presenter
        {
            public Walker Player { get; }

            public Presenter(Walker player) => Player = player;
            public float TangentAcceleration
            {
                set => Player.TangentAcceleration = value;
            }
            public float NormalAcceleration
            {
                set => Player.NormalAcceleration = value;
            }
        }

        #endregion
    }
}
