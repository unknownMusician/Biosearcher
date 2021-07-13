using Biosearcher.Common;
using Biosearcher.InputHandling;
using Biosearcher.Planets.Orientation;
using Biosearcher.Refactoring;
using System;
using System.Collections;
using System.Collections.Generic;
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
            //Vector3 planetCenterLocalPosition = planetPosition - transform.position;
            Vector3 planetCenterLocalPosition = -transform.up;
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
                //rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
                //
                Vector3 hitNormalRotationAngles = Quaternion.FromToRotation(Vector3.up, Quaternion.Euler(0f, -rotation.eulerAngles.y, 0f) * _planetTransform.ToPlanet(hitInfo.normal)).eulerAngles;

                rotation = Quaternion.Euler(hitNormalRotationAngles.x, rotation.eulerAngles.y, hitNormalRotationAngles.z);
                //
                _desiredRotation = _planetTransform.ToUniverse(rotation);

                //transform.rotation = (Quaternion)_desiredRotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion)_desiredRotation, 0.1f);

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



    public class Test1 : MonoBehaviour
    {
        private AState _walkingState;
        private AState _sittingState;
        private AState _state;

        private float _value;

        private void Awake()
        {
            _walkingState = new WalkingState(this);
            _sittingState = new SittingState(this);
        }

        private void Update() => Debug.Log(_value);

        private abstract class AState
        {
            protected readonly Test1 _test;

            public AState(Test1 test) => _test = test;
            public abstract void Stand();
            public abstract void Sit();
        }

        private class WalkingState : AState
        {
            public WalkingState(Test1 test) : base(test) { }

            public override void Sit() => _test._value = 1;
            public override void Stand() => _test._value = 2;
        }

        private class SittingState : AState
        {
            public SittingState(Test1 test) : base(test) { }

            public override void Sit() => _test._value = 3;
            public override void Stand() => _test._value = 4;
        }

        public void Stand() => _state.Stand();
        public void Sit() => _state.Sit();

        public void ChangeState(bool value) => _state = value ? _walkingState : _sittingState;
    }




    public class Test2 : MonoBehaviour
    {
        private readonly State _walkingState = new State();
        private State _state;

        private float _value;

        private Test2()
        {
            _walkingState.Register(Sit, () => _value = 1f);
            _walkingState.Register(Stand, () => _value = 2f);
            _walkingState.Register<int, int, int, Vector2>(GetSomething, (x, y, z) => new Vector2(x + y, z));
        }

        private void Update()
        {
            Debug.Log(_value);
        }

        public void Sit() => _state.Invoke(Sit);
        public void Sit2() => _state.Get(Sit).Invoke();
        public void Stand() => _state.Invoke(Stand);

        public Vector2 GetSomething(int x, int y, int z) => _state.Get<int, int, int, Vector2>(GetSomething).Invoke(x, y, z);
    }

    public sealed class State : IDisposable
    {
        private Dictionary<object, object> _actions = new Dictionary<object, object>();

        public void Register<T>(T method, T action) where T : Delegate => _actions[method] = action;
        public T Get<T>(T method) where T : Delegate => (T)_actions[method];

        public void Register(Action method, Action action) => Register<Action>(method, action);
        public void Register<T>(Action<T> method, Action<T> action) => Register<Action<T>>(method, action);
        public void Register<T1, T2>(Action<T1, T2> method, Action<T1, T2> action) => Register<Action<T1, T2>>(method, action);
        public void Register<T1, T2, T3>(Action<T1, T2, T3> method, Action<T1, T2, T3> action) => Register<Action<T1, T2, T3>>(method, action);
        public void Register<R>(Func<R> method, Func<R> action) => Register<Func<R>>(method, action);
        public void Register<T, R>(Func<T, R> method, Func<T, R> action) => Register<Func<T, R>>(method, action);
        public void Register<T1, T2, R>(Func<T1, T2, R> method, Func<T1, T2, R> action) => Register<Func<T1, T2, R>>(method, action);
        public void Register<T1, T2, T3, R>(Func<T1, T2, T3, R> method, Func<T1, T2, T3, R> action) => Register<Func<T1, T2, T3, R>>(method, action);

        public Action Get(Action method) => Get<Action>(method);
        public Action<T> Get<T>(Action<T> method) => Get<Action<T>>(method);
        public Action<T1, T2> Get<T1, T2>(Action<T1, T2> method) => Get<Action<T1, T2>>(method);
        public Action<T1, T2, T3> Get<T1, T2, T3>(Action<T1, T2, T3> method) => Get<Action<T1, T2, T3>>(method);
        public Func<R> Get<R>(Func<R> method) => Get<Func<R>>(method);
        public Func<T, R> Get<T, R>(Func<T, R> method) => Get<Func<T, R>>(method);
        public Func<T1, T2, R> Get<T1, T2, R>(Func<T1, T2, R> method) => Get<Func<T1, T2, R>>(method);
        public Func<T1, T2, T3, R> Get<T1, T2, T3, R>(Func<T1, T2, T3, R> method) => Get<Func<T1, T2, T3, R>>(method);

        public void Invoke(Action method) => ((Action)_actions[method]).Invoke();
        public void Invoke<T>(Action<T> method, T param) => Get<Action<T>>(method).Invoke(param);
        public void Invoke<T1, T2>(Action<T1, T2> method, T1 param1, T2 param2) => ((Action<T1, T2>)_actions[method]).Invoke(param1, param2);
        public void Invoke<T1, T2, T3>(Action<T1, T2, T3> method, T1 param1, T2 param2, T3 param3) => ((Action<T1, T2, T3>)_actions[method]).Invoke(param1, param2, param3);
        public R Invoke<R>(Func<R> method) => ((Func<R>)_actions[method]).Invoke();
        public R Invoke<T, R>(Func<T, R> method, T param) => Get<Func<T, R>>(method).Invoke(param);
        public R Invoke<T1, T2, R>(Func<T1, T2, R> method, T1 param1, T2 param2) => ((Func<T1, T2, R>)_actions[method]).Invoke(param1, param2);
        public R Invoke<T1, T2, T3, R>(Func<T1, T2, T3, R> method, T1 param1, T2 param2, T3 param3) => ((Func<T1, T2, T3, R>)_actions[method]).Invoke(param1, param2, param3);

        public void Dispose() => _actions.Clear();
    }
}
