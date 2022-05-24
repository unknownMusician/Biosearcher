using System;
using AreYouFruits.Common;
using AreYouFruits.Common.Collections;
using AreYouFruits.Common.States;
using UnityEngine;

namespace Biosearcher.Player.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(WheelAnimator))]
    // todo
    public sealed class Walker : MonoBehaviour
    {
        #region Properties

        [Header("Moving - Tangent")]
        [SerializeField] private float _tangentAcceleration = 100;
        [SerializeField] private float _maxSpeed = 20;
        [SerializeField] private float _tangentDamp = 10;
        [Header("Moving - Normal")]
        [SerializeField] private float _normalAcceleration = 40;
        [SerializeField] private float _maxRotationSpeed = 20;
        [SerializeField] private float _normalDamp = 10;
        [Header("Stabilizer")]
        [SerializeField] private float _springTangentAcceleration = 1000;
        [SerializeField] private float _springTangentDamp = 10;
        [SerializeField] private float _springNormalAcceleration = 200;
        [SerializeField] private float _springNormalDamp = 10;

        private Rigidbody _rigidbody;
        private WheelAnimator _animator;
        private PlayerInput _input;

        private ChangeableStateManager<WalkerState, ActionName> _states;

        private RaycastHit _hitInfo;

        private Vector3? _lastFramePositionRelativeToDesiredPosition;

        private Vector3? _desiredPosition;
        private Vector3 _downDirection;

        private float GroundDesiredHeight => GroundCheckHeight * (3f / 4f);
        private float GroundCheckHeight => _animator.Suspension.Max;
        private LayerMask GroundMask => _animator.GroundMask;
        public StateHook<WalkerState> StateHook => _states.Hook;

        internal float TangentAcceleration { get; set; }
        internal float NormalAcceleration { get; set; }

        #endregion

        private Walker() => RegisterStates();

        #region MonoBehaviour methods

        private void Awake() => gameObject.GetComponents(out _rigidbody, out _animator);
        private void OnDestroy() => _states.Dispose();

        private void FixedUpdate()
        {
            _downDirection = -transform.up.normalized;

            _states.TryChange(_animator.AtLeastOneWeelTouchesGround ? WalkerState.OnGroundState : WalkerState.InAirState);

            _states.Active.Get<Action>(ActionName.StabilizeTangent)?.Invoke();
            _states.Active.Get<Action>(ActionName.StabilizeNormal)?.Invoke();
            _states.Active.Get<Action>(ActionName.Move)?.Invoke();
        }

        private void OnDrawGizmos() => _states.Active.Get<Action>(ActionName.DrawGizmos)?.Invoke();

        #endregion

        #region Methods

        private void RegisterStates()
        {
            _states = new ChangeableStateManager<WalkerState, ActionName>();

            _states.Register(WalkerState.OnGroundState)
                .Register<Action>(
                    (ActionName.StabilizeTangent, StabilizeTangent),
                    (ActionName.StabilizeNormal, StabilizeNormal),
                    (ActionName.Move, Move),
                    (ActionName.DrawGizmos, DrawGizmos));

            _states.Register(WalkerState.InAirState)
                .Register<Action>(
                    (ActionName.StabilizeTangent, () => _desiredPosition = null),
                    (ActionName.StabilizeNormal, null),
                    (ActionName.Move, null),
                    (ActionName.DrawGizmos, null));

            _states.TryChange(WalkerState.InAirState);
        }

        // todo: Remove
        private bool RaycastLocalDown() => Physics.Raycast(transform.position, _downDirection, out _hitInfo, GroundCheckHeight, GroundMask);

        private void DrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_desiredPosition.Value, 0.1f);
        }

        // todo: Make Height check based on wheels, not on overall height
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

        // todo
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
            //Quaternion rotation = _planetTransform.PlanetRotation;

            //Vector3 hitNormalLocalPositionRotated = Quaternion.Euler(0f, -rotation.eulerAngles.y, 0f) * _planetTransform.ToPlanet(normal);
            //Vector3 hitNormalRotationAngles = Quaternion.FromToRotation(Vector3.up, hitNormalLocalPositionRotated).eulerAngles;

            //rotation = Quaternion.Euler(hitNormalRotationAngles.x, rotation.eulerAngles.y, hitNormalRotationAngles.z);

            //return _planetTransform.ToUniverse(rotation);

            return Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
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
                Vector3 acceleration = NormalAcceleration * _normalAcceleration * transform.up;
                playerRigidbody.angularVelocity += acceleration * deltaTime;
            }

            Vector3 normalDamping = -playerRigidbody.angularVelocity * _normalDamp;
            playerRigidbody.angularVelocity += normalDamping * deltaTime;
        }

        #endregion

        #region Classes

        private enum ActionName
        {
            StabilizeTangent,
            StabilizeNormal,
            Move,
            DrawGizmos
        }

        #endregion
    }
}
