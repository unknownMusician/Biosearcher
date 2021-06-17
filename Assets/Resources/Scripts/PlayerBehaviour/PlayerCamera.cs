using Biosearcher.InputHandling;
using Biosearcher.Planet.Orientation;
using UnityEngine;

namespace Biosearcher.PlayerBehaviour
{
    [RequireComponent(typeof(PlanetTransform))]
    public class PlayerCamera : MonoBehaviour
    {
        #region Properties

        [SerializeField] protected Vector3 _relativePosition;
        [SerializeField] protected Transform _player;

        protected PlayerCameraInput _input;
        protected PlanetTransform _planetTransform;

        protected float _rotationX;

        public Quaternion RotationWithoutX { get; protected set; }

        #endregion

        #region MonoBehaviour methods

        protected void Awake()
        {
            _planetTransform = GetComponent<PlanetTransform>();

            _input = new PlayerCameraInput(new Presenter(this));
        }
        protected void OnDestroy() => _input.Dispose();

        protected void OnEnable() => _input.OnEnable();
        protected void OnDisable() => _input.OnDisable();

        protected void Update()
        {
            Vector3 planetPosition = Vector3.zero;

            RotationWithoutX = Quaternion.FromToRotation(transform.up, transform.position - planetPosition) * transform.rotation;
            transform.rotation = RotationWithoutX;
            //planetTransform.planetRotation = Quaternion.Euler(PlanetEulerAngles);
            transform.rotation = Quaternion.AngleAxis(-_rotationX, transform.right) * RotationWithoutX;
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
            if (Mathf.Abs(_rotationX + direction.y) <= 60)
            {
                _rotationX += direction.y;
            }
            Vector3 planetPosition = Vector3.zero;

            Quaternion rotateY = Quaternion.AngleAxis(direction.x, transform.position - planetPosition);
            transform.rotation = rotateY * transform.rotation;

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
