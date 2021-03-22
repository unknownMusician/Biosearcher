using Biosearcher.InputHandling;
using Biosearcher.Planet.Orientation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.PlayerBehaviour
{
    [RequireComponent(typeof(PlanetTransform))]
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] protected Vector3 relativePosition;
        [SerializeField] protected Transform player;

        protected PlayerCameraInput input;
        protected PlanetTransform planetTransform;

        protected Vector3 _planetEulerAngles;
        public Vector3 PlanetEulerAngles
        {
            get => _planetEulerAngles;
            protected set => _planetEulerAngles = new Vector3(value.x % 380, value.y % 380, value.z % 380);
        }

        protected void Awake()
        {
            planetTransform = GetComponent<PlanetTransform>();

            input = new PlayerCameraInput(new Presenter(this));
        }
        protected void OnDestroy() => input.OnDestroy();

        protected void OnEnable() => input.OnEnable();
        protected void OnDisable() => input.OnDisable();

        protected void FixedUpdate() => Move();

        protected void Move()
        {
            transform.position = player.position + transform.rotation * relativePosition;
        }

        protected void Rotate(Vector2 direction)
        {
            float currentRotationX = PlanetEulerAngles.x;
            if (currentRotationX > 60 && currentRotationX < 180 && direction.y < 0)
            {
                direction.y = 0;
            }
            if (currentRotationX > 180 && currentRotationX < 300 && direction.y > 0)
            {
                direction.y = 0;
            }

            PlanetEulerAngles += new Vector3(-direction.y, direction.x, 0);
            planetTransform.planetRotation = Quaternion.Euler(PlanetEulerAngles);
            Move();
        }

        public class Presenter
        {
            public PlayerCamera Camera { get; }

            public Presenter(PlayerCamera camera) => Camera = camera;
            public void Rotate(Vector2 direction) => Camera.Rotate(direction);
        }
    }
}
