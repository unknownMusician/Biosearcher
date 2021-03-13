using Biosearcher.InputHandling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.PlayerBehaviour
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] protected Vector3 relativePosition;
        [SerializeField] protected Transform player;

        protected PlayerCameraInput input;

        protected void Awake()
        {
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
            float currentRotationX = transform.rotation.eulerAngles.x;
            if (currentRotationX > 60 && currentRotationX < 180 && direction.y < 0)
            {
                direction.y = 0;
            }
            if (currentRotationX > 180 && currentRotationX < 300 && direction.y > 0)
            {
                direction.y = 0;
            }

            transform.eulerAngles = transform.eulerAngles + new Vector3(-direction.y, direction.x, 0);
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
