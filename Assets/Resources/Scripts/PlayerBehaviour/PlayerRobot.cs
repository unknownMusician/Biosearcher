using Biosearcher.InputHandling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.PlayerBehaviour
{
    public class PlayerRobot : MonoBehaviour
    {
        [SerializeField] protected float speed = 1;

        protected PlayerRobotInput input;
        protected new Rigidbody rigidbody;

        protected void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            input = new PlayerRobotInput(new Presenter(this));
        }
        protected void OnDestroy() => input.Dispose();

        protected void OnEnable() => input.OnEnable();
        protected void OnDisable() => input.OnDisable();

        protected void Start() => StartCoroutine(Moving());

        protected float WheelVelocity { get; set; }

        protected float WheelRotation
        {
            set
            {
                // todo
            }
        }

        protected IEnumerator Moving()
        {
            // todo
            while (true)
            {
                yield return new WaitForFixedUpdate();
                rigidbody.velocity = transform.forward * WheelVelocity * speed;
            }
        }

        public class Presenter
        {
            public PlayerRobot Player { get; }

            public Presenter(PlayerRobot player) => Player = player;
            public float WheelVelocity
            {
                set => Player.WheelVelocity = value;
            }
            public float WheelRotation
            {
                set => WheelRotation = value;
            }
        }

        public struct Wheels
        {
            public WheelCollider forwardLeft;
            public WheelCollider forwardRight;
            public WheelCollider middleLeft;
            public WheelCollider middleRight;
            public WheelCollider backLeft;
            public WheelCollider backRight;
        }
    }
}
