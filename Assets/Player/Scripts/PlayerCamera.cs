using Biosearcher.Common;
using Biosearcher.InputHandling;
using UnityEngine;

namespace Biosearcher.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        #region Properties

        [SerializeField] protected Vector3 _relativePosition;
        [SerializeField] protected Transform _player;
        [SerializeField] [Range(0.001f, 1.0f)] protected float _playerRotationSmoothTime;
        [SerializeField] [Range(0.001f, 1.0f)] protected float _smoothTime;

        protected PlayerCameraInput _input;

        protected Vector2 _rotation;
        protected Vector3 _lastPlayerForward;
        protected Vector3 _lastPlayerPositionVelocity;
        protected Quaternion _lastCameraRotation;

        public Quaternion RotationWithoutX { get; protected set; }

        #endregion

        #region MonoBehaviour methods

        protected void Awake()
        {
            _input = new PlayerCameraInput(new Presenter(this));
        }
        protected void OnDestroy() => _input.Dispose();

        protected void OnEnable() => _input.OnEnable();
        protected void OnDisable() => _input.OnDisable();

        protected void Update()
        {
            _lastPlayerForward = Vector3.SmoothDamp(_lastPlayerForward, _player.forward, ref _lastPlayerPositionVelocity, _playerRotationSmoothTime);

            transform.rotation = Quaternion.LookRotation(_lastPlayerForward, Vector3.up);

            Quaternion finalRotation = Quaternion.AngleAxis(_rotation.y, transform.up) * Quaternion.AngleAxis(-_rotation.x, transform.right) * transform.rotation;
            _lastCameraRotation = transform.rotation = Quaternion.Slerp(_lastCameraRotation, finalRotation, _smoothTime);

            Move();
        }

        #endregion

        #region Methods

        protected void Move()
        {
            transform.position = _player.position + transform.rotation * _relativePosition;
        }
        protected void Rotate(Vector2 direction)
        {
            if (Mathf.Abs(_rotation.x + direction.y) <= 60)
            {
                _rotation.x += direction.y;
            }
            _rotation.y += direction.x;
            _rotation.y.MakeCycleDegrees180();

            //Quaternion rotateY = Quaternion.AngleAxis(direction.x, transform.position);
            //transform.rotation = rotateY * transform.rotation;

            //PlanetEulerAngles += new Vector3(-direction.y, direction.x, 0);
            //planetTransform.planetRotation = Quaternion.Euler(PlanetEulerAngles);
            Move();
        }

        #endregion

        #region Classes

        public class Presenter
        {
            public PlayerCamera Camera { get; }

            public Presenter(PlayerCamera camera) => Camera = camera;
            public void Rotate(Vector2 direction) => Camera.Rotate(direction);
        }

        #endregion
    }
}
