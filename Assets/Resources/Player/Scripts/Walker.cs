using Biosearcher.InputHandling;
using Biosearcher.Planets.Orientation;
using Biosearcher.Refactoring;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Biosearcher.Player
{
    [NeedsRefactor]
    public class Walker : MonoBehaviour
    {
        #region Properties

        [Header("Moving - Tangent")]
        [SerializeField] protected float _tangentAcceleration = 100;
        [SerializeField] protected float _maxSpeed = 20;
        [SerializeField] protected float _tangentDamp = 10;
        [Header("Moving - Normal")]
        [SerializeField] protected float _normalAcceleration = 20;
        [SerializeField] protected float _maxRotationSpeed = 10;
        [SerializeField] protected float _normalDamp = 10;
        [Header("Stabilizer")]
        [SerializeField] protected LayerMask _groundMask;
        [SerializeField] protected float _groundCheckHeight = 1.5f;
        [SerializeField] protected float _groundDesiredHeight = 1.5f;
        [SerializeField] protected float _springTangentAcceleration = 100;
        [SerializeField] protected float _springTangentDamp = 10;
        [SerializeField] protected float _springNormalAcceleration = 100;
        [SerializeField] protected float _springNormalDamp = 10;
        [Header("Other")]
        [SerializeField] protected PlayerCamera _camera;

        protected PlayerInput _input;
        protected Rigidbody _rigidbody;
        protected State _state;
        protected PlanetTransform _planetTransform;

        protected Vector3? _lastFramePositionRelativeToDesiredPosition;
        protected Vector3 _tangentVelocityRelativeToDesiredPosition;

        protected Vector3? _desiredPosition;
        
        protected Quaternion? _lastFrameRotationRelativeToDesiredRotation;
        protected Quaternion _normalVelocityRelativeToDesiredRotation;

        protected Quaternion? _desiredRotation;

        #endregion

        #region MonoBehaviour methods

        protected void Awake()
        {
            _state = new State(this);
            _rigidbody = GetComponent<Rigidbody>();
            _planetTransform = GetComponent<PlanetTransform>();
            _input = new PlayerInput(new Presenter(this));
        }
        protected void OnDestroy() => _input.Dispose();

        protected void OnEnable() => _input.OnEnable();
        protected void OnDisable() => _input.OnDisable();

        protected void Start() => StartCoroutine(Moving());
        
        [NeedsRefactor]
        protected void FixedUpdate()
        {
            Vector3 planetPosition = Vector3.zero;
            // todo
            Color debugColor;
            Vector3 planetCenterLocalPosition = planetPosition - transform.position;
            if (Physics.Raycast(transform.position, planetCenterLocalPosition, out RaycastHit hitInfo, _groundCheckHeight, _groundMask))
            {
                _desiredPosition = hitInfo.point - planetCenterLocalPosition.normalized * _groundDesiredHeight;

                Vector3 positionRelativeToDesiredPosition = transform.position - (Vector3)_desiredPosition;
                if (_lastFramePositionRelativeToDesiredPosition != null)
                {
                    _tangentVelocityRelativeToDesiredPosition = ((Vector3)_lastFramePositionRelativeToDesiredPosition - positionRelativeToDesiredPosition) / Time.deltaTime;
                }
                else
                {
                    _tangentVelocityRelativeToDesiredPosition = Vector3.zero;
                }
                _lastFramePositionRelativeToDesiredPosition = positionRelativeToDesiredPosition;

                Vector3 tangentDistance = -planetCenterLocalPosition.normalized * _groundDesiredHeight - (transform.position - hitInfo.point);
                Vector3 tangentAcceleration = tangentDistance.normalized * _springTangentAcceleration * (tangentDistance.magnitude * tangentDistance.magnitude);
                Vector3 tangentDamping = -_rigidbody.velocity.normalized * _tangentVelocityRelativeToDesiredPosition.magnitude * _springTangentDamp;
                _rigidbody.velocity += (tangentAcceleration + tangentDamping) * Time.deltaTime;
                
                Quaternion rotation = transform.rotation;
                rotation = _planetTransform.ToPlanet(rotation);
                rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
                _desiredRotation = _planetTransform.ToUniverse(rotation);

                //transform.rotation = (Quaternion)_desiredRotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion)_desiredRotation, 0.1f);

                //Quaternion rotationRelativeToDesiredRotation = Quaternion.Inverse((Quaternion)desiredRotation) * transform.rotation;
                //if (lastFrameRotationRelativeToDesiredRotation != null)
                //{
                //    normalVelocityRelativeToDesiredRotation = Quaternion.Euler(((Quaternion.Inverse(rotationRelativeToDesiredRotation) * (Quaternion)lastFrameRotationRelativeToDesiredRotation)).eulerAngles / Time.deltaTime);
                //}
                //else
                //{
                //    normalVelocityRelativeToDesiredRotation = Quaternion.identity;
                //}
                //lastFrameRotationRelativeToDesiredRotation = rotationRelativeToDesiredRotation;

                //Vector3 normalDistance = (Quaternion.Inverse(transform.rotation) * (Quaternion)desiredRotation).eulerAngles;
                //Vector3 normalAcceleration = normalDistance.normalized * springNormalAcceleration * (normalDistance.magnitude * normalDistance.magnitude);
                //Vector3 normalDamping = -rigidbody.angularVelocity.normalized * normalVelocityRelativeToDesiredRotation.eulerAngles.magnitude * springNormalDamp;
                //rigidbody.angularVelocity += (normalAcceleration + normalDamping) * Time.deltaTime;
                
                _state.CurrentMove = _state.MoveOnGround;
                debugColor = Color.green;
            }
            else
            {
                _desiredPosition = null;
                _desiredRotation = null;
                _state.CurrentMove = _state.MoveInAir;
                debugColor = Color.red;
            }
            Debug.DrawLine(transform.position, transform.position + planetCenterLocalPosition.normalized * _groundCheckHeight, debugColor, 0.02f);
        }
        
        protected void OnDrawGizmos()
        {
            if (_desiredPosition != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere((Vector3)_desiredPosition, 0.1f);
            }
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position + (transform.forward + transform.up) / 2, 0.1f);
        }

        #endregion
        
        #region Methods

        protected float TangentAcceleration { get; set; }
        protected float NormalAcceleration { get; set; }

        [NeedsRefactor]
        protected IEnumerator Moving()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            // todo
            while (true)
            {
                yield return waitForFixedUpdate;
                _state.Move();
            }
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

        protected class State
        {
            #region Properties 
            
            private UnityAction _currentMove;
            protected readonly Walker _player;

            public UnityAction CurrentMove
            {
                set => _currentMove = value;
            }

            #endregion

            #region Constructors

            public State(Walker player, UnityAction moveState)
            {
                _player = player;
                _currentMove = moveState;
            }

            [NeedsRefactor]
            public State(Walker player)
            {
                // todo
                _player = player;
                _currentMove = MoveOnGround;
            }

            #endregion

            #region Methods

            public void MoveInAir() { }
            public void MoveOnGround()
            {
                var playerRigidbody = _player._rigidbody;
                
                //player.transform.rotation = player.camera.RotationWithoutX;
                if (playerRigidbody.velocity.magnitude < _player._maxSpeed)
                {
                    Vector3 acceleration = _player.TangentAcceleration * _player._tangentAcceleration * _player.transform.forward;
                    playerRigidbody.velocity += acceleration * Time.deltaTime;
                }
                
                Vector3 tangentDamping = -playerRigidbody.velocity * _player._tangentDamp;
                playerRigidbody.velocity += tangentDamping * Time.deltaTime;
                
                if (playerRigidbody.angularVelocity.magnitude < _player._maxRotationSpeed)
                {
                    Vector3 acceleration = _player._planetTransform.ToUniverse(_player.NormalAcceleration * _player._normalAcceleration * Vector3.up);
                    playerRigidbody.angularVelocity += acceleration * Time.deltaTime;
                }
                
                Vector3 normalDamping = -playerRigidbody.angularVelocity * _player._tangentDamp;
                playerRigidbody.angularVelocity += normalDamping * Time.deltaTime;
            }
            public void Move() => _currentMove?.Invoke();

            #endregion
        }

        #endregion
    }
}
