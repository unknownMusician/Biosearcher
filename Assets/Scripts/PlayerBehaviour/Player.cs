using Biosearcher.InputHandling;
using UnityEngine;

namespace Biosearcher.PlayerBehaviour
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField] protected float speed = 1;
        [SerializeField] protected new Transform camera;

        protected new Rigidbody rigidbody;
        protected PlayerInput input;

        protected void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();

            input = new PlayerInput(new Presenter(this));
        }
        protected void OnDestroy() => input.OnDestroy();

        protected void OnEnable() => input.OnEnable();
        protected void OnDisable() => input.OnDisable();

        protected void Move(Vector2 direction)
        {
            Quaternion targetRotation = Quaternion.Euler(0, camera.eulerAngles.y, 0);
            rigidbody.velocity = targetRotation * direction.ReProjectedXZ() * speed;
            transform.rotation = targetRotation;
        }

        public class Presenter
        {
            public Player Player { get; }

            public Presenter(Player player) => Player = player;
            public void Move(Vector2 direction) => Player.Move(direction);
        }
    }
}