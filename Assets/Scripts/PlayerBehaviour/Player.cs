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

        protected void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            planetTransform = GetComponent<PlanetTransform>();

            input = new PlayerInput(new Presenter(this));
        }
        protected void OnDestroy() => input.OnDestroy();

        protected void OnEnable() => input.OnEnable();
        protected void OnDisable() => input.OnDisable();

        protected void Move(Vector2 direction)
        {
            Quaternion targetRotation = planetTransform.ToUniverse(Quaternion.Euler(0, camera.PlanetEulerAngles.y, 0));
            float planetYVelocity = planetTransform.ToPlanet(rigidbody.velocity).y;
            rigidbody.velocity = targetRotation * (direction * speed).ReProjectedXZ(planetYVelocity);
        }

        protected void Update()
        {
            planetTransform.planetRotation = Quaternion.identity;
        }

        public class Presenter
        {
            public Player Player { get; }

            public Presenter(Player player) => Player = player;
            public void Move(Vector2 direction) => Player.Move(direction);
        }
    }
}