using Biosearcher.InputHandling;
using Biosearcher.Planet.Orientation;
using UnityEngine;

namespace Biosearcher.PlayerBehaviour
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlanetTransform))]
    public class Player : MonoBehaviour
    {
        [SerializeField] protected float speed = 1;
        [SerializeField] protected new PlayerCamera camera;

        protected new Rigidbody rigidbody;
        protected PlayerInput input;
        protected PlanetTransform planetTransform;

        protected Vector2 MoveDirection { get; set; }

        protected void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            planetTransform = GetComponent<PlanetTransform>();

            input = new PlayerInput(new Presenter(this));
        }
        protected void OnDestroy() => input.OnDestroy();

        protected void OnEnable() => input.OnEnable();
        protected void OnDisable() => input.OnDisable();

        protected void FixedUpdate() => Move(MoveDirection);

        protected void Move(Vector2 direction)
        {
            if (direction.magnitude == 0)
            {
                return;
            }
            // Quaternion targetRotation = planetTransform.ToUniverse(Quaternion.Euler(0, camera.PlanetEulerAngles.y, 0));
            float planetYVelocity = planetTransform.ToPlanet(rigidbody.velocity).y;
            //Vector3 universeVerticalVelocity = planetTransform.ToUniverse(new Vector3(0, planetYVelocity, 0));
            transform.rotation = camera.RotationWithoutX;
            Vector3 universeVerticalVelocity = transform.up * planetYVelocity;
            Vector3 moveUniverseDirection = transform.right * direction.x + transform.forward * direction.y;
            ////moveUniverseDirection *= speed;
            //Debug.Log(planetYVelocity);
            //Debug.DrawLine(transform.position, transform.position + universeVerticalVelocity, Color.yellow, 20);
            rigidbody.velocity = moveUniverseDirection * speed + universeVerticalVelocity;
            ////rigidbody.AddForce(moveUniverseDirection);
        }

        protected void Update()
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, transform.position - planetTransform.ChunkManager.PlanetPosition) * transform.rotation;
            //planetTransform.planetRotation = Quaternion.identity;
        }

        public class Presenter
        {
            public Player Player { get; }

            public Presenter(Player player) => Player = player;
            public void Move(Vector2 direction) => Player.MoveDirection = direction;
        }
    }
}