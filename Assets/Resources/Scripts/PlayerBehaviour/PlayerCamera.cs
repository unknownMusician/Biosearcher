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

        protected float rotationX;

        public Quaternion RotationWithoutX { get; protected set; }

        protected void Awake()
        {
            planetTransform = GetComponent<PlanetTransform>();

            input = new PlayerCameraInput(new Presenter(this));
        }
        protected void OnDestroy() => input.OnDestroy();

        protected void OnEnable() => input.OnEnable();
        protected void OnDisable() => input.OnDisable();

        protected void Update()
        {
            RotationWithoutX = Quaternion.FromToRotation(transform.up, transform.position - planetTransform.ChunkManager.PlanetPosition) * transform.rotation;
            transform.rotation = RotationWithoutX;
            //planetTransform.planetRotation = Quaternion.Euler(PlanetEulerAngles);
            transform.rotation = Quaternion.AngleAxis(-rotationX, transform.right) * RotationWithoutX;
            Move();
        }

        protected void Move()
        {
            transform.position = player.position + transform.rotation * relativePosition;
        }

        protected void Rotate(Vector2 direction)
        {
            if (Mathf.Abs(rotationX + direction.y) <= 60)
            {
                rotationX += direction.y;
            }
            Quaternion rotateY = Quaternion.AngleAxis(direction.x, transform.position - planetTransform.ChunkManager.PlanetPosition);
            transform.rotation = rotateY * transform.rotation;

            //PlanetEulerAngles += new Vector3(-direction.y, direction.x, 0);
            //planetTransform.planetRotation = Quaternion.Euler(PlanetEulerAngles);
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
