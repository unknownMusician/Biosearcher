using Biosearcher.InputHandling;
using Biosearcher.Planet.Orientation;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Biosearcher.PlayerBehaviour
{
    public class PlayerRobot : MonoBehaviour
    {
        [Header("Moving - Tangent")]
        [SerializeField] protected float tangentAcceleration = 100;
        [SerializeField] protected float maxSpeed = 20;
        [SerializeField] protected float tangentDamp = 10;
        [Header("Moving - Normal")]
        [SerializeField] protected float normalAcceleration = 20;
        [SerializeField] protected float maxRotationSpeed = 10;
        [SerializeField] protected float normalDamp = 10;
        [Header("Stabilizer")]
        [SerializeField] protected LayerMask groundMask;
        [SerializeField] protected float groundCheckHeight = 1.5f;
        [SerializeField] protected float groundDesiredHeight = 1.5f;
        [SerializeField] protected float springTangentAcceleration = 100;
        [SerializeField] protected float springTangentDamp = 10;
        [SerializeField] protected float springNormalAcceleration = 100;
        [SerializeField] protected float springNormalDamp = 10;
        [Header("Other")]
        [SerializeField] protected new PlayerCamera camera;

        protected PlayerRobotInput input;
        protected new Rigidbody rigidbody;
        protected State state;
        protected PlanetTransform planetTransform;

        protected Vector3? lastFramePositionRelativeToDesiredPosition;
        protected Vector3 tangentVelocityRelativeToDesiredPosition;

        protected Vector3? desiredPosition;


        protected Quaternion? lastFrameRotationRelativeToDesiredRotation;
        protected Quaternion normalVelocityRelativeToDesiredRotation;

        protected Quaternion? desiredRotation;

        protected void Awake()
        {
            state = new State(this);
            rigidbody = GetComponent<Rigidbody>();
            planetTransform = GetComponent<PlanetTransform>();
            input = new PlayerRobotInput(new Presenter(this));
        }
        protected void OnDestroy() => input.Dispose();

        protected void OnEnable() => input.OnEnable();
        protected void OnDisable() => input.OnDisable();

        protected void Start() => StartCoroutine(Moving());

        protected float TangentAcceleration { get; set; }
        protected float NormalAcceleration { get; set; }

        protected void FixedUpdate()
        {
            Vector3 planetPosition = Vector3.zero;
            // todo
            Color debugColor;
            Vector3 planetCenterlocalPosition = planetPosition - transform.position;
            if (Physics.Raycast(transform.position, planetCenterlocalPosition, out RaycastHit hitInfo, groundCheckHeight, groundMask))
            {
                desiredPosition = hitInfo.point - planetCenterlocalPosition.normalized * groundDesiredHeight;

                Vector3 positionRelativeToDesiredPosition = transform.position - (Vector3)desiredPosition;
                if (lastFramePositionRelativeToDesiredPosition != null)
                {
                    tangentVelocityRelativeToDesiredPosition = ((Vector3)lastFramePositionRelativeToDesiredPosition - positionRelativeToDesiredPosition) / Time.deltaTime;
                }
                else
                {
                    tangentVelocityRelativeToDesiredPosition = Vector3.zero;
                }
                lastFramePositionRelativeToDesiredPosition = positionRelativeToDesiredPosition;

                Vector3 tangentDistance = -planetCenterlocalPosition.normalized * groundDesiredHeight - (transform.position - hitInfo.point);
                Vector3 tangentAcceleration = tangentDistance.normalized * springTangentAcceleration * (tangentDistance.magnitude * tangentDistance.magnitude);
                Vector3 tangentDamping = -rigidbody.velocity.normalized * tangentVelocityRelativeToDesiredPosition.magnitude * springTangentDamp;
                rigidbody.velocity += (tangentAcceleration + tangentDamping) * Time.deltaTime;


                Quaternion rotation = transform.rotation;
                rotation = planetTransform.ToPlanet(rotation);
                rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
                desiredRotation = planetTransform.ToUniverse(rotation);

                transform.rotation = (Quaternion)desiredRotation;

                //Quaternion rotationRelativeToDesiredRotation = Quaternion.Inverse((Quaternion)desiredRotation) * transform.rotation;
                //if (lastFrameRotationRelativeToDesiredRotation != null)
                //{
                //    normalVelocityRelativeToDesiredRotation = Quaternion.Euler(((Quaternion.Inverse(rotationRelativeToDesiredRotation) * (Quaternion)lastFrameRotationRelativeToDesiredRotation)).eulerAngles / Time.deltaTime);
                //}
                //else
                //{
                //    normalVelocityRelativeToDesiredRotation = Quaternion.identity;
                //}
                //lastFrameRotationRelativeToDesiredRotation = rotationRelativeToDesiredRotation;


                //Vector3 normalDistance = (Quaternion.Inverse(transform.rotation) * (Quaternion)desiredRotation).eulerAngles;
                //Vector3 normalAcceleration = normalDistance.normalized * springNormalAcceleration * (normalDistance.magnitude * normalDistance.magnitude);
                //Vector3 normalDamping = -rigidbody.angularVelocity.normalized * normalVelocityRelativeToDesiredRotation.eulerAngles.magnitude * springNormalDamp;
                //rigidbody.angularVelocity += (normalAcceleration + normalDamping) * Time.deltaTime;


                state.currentMove = state.MoveOnGround;
                debugColor = Color.green;
            }
            else
            {
                desiredPosition = null;
                desiredRotation = null;
                state.currentMove = state.MoveInAir;
                debugColor = Color.red;
            }
            Debug.DrawLine(transform.position, transform.position + planetCenterlocalPosition.normalized * groundCheckHeight, debugColor, 0.02f);
        }

        protected IEnumerator Moving()
        {
            // todo
            while (true)
            {
                yield return new WaitForFixedUpdate();
                state.Move();
            }
        }

        protected void OnDrawGizmos()
        {
            if (desiredPosition != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere((Vector3)desiredPosition, 0.1f);
            }
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position + (transform.forward + transform.up) / 2, 0.1f);
        }

        public class Presenter
        {
            public PlayerRobot Player { get; }

            public Presenter(PlayerRobot player) => Player = player;
            public float TangentAcceleration
            {
                set => Player.TangentAcceleration = value;
            }
            public float NormalAcceleration
            {
                set => Player.NormalAcceleration = value;
            }
        }

        public class State
        {
            public UnityAction currentMove;

            protected PlayerRobot player;

            public State(PlayerRobot player, UnityAction moveState)
            {
                this.player = player;
                this.currentMove = moveState;
            }
            public State(PlayerRobot player)
            {
                // todo
                this.player = player;
                this.currentMove = MoveOnGround;
            }
            public void MoveInAir() { }
            public void MoveOnGround()
            {
                //player.transform.rotation = player.camera.RotationWithoutX;
                if (player.rigidbody.velocity.magnitude < player.maxSpeed)
                {
                    Vector3 acceleration = player.TangentAcceleration * player.tangentAcceleration * player.transform.forward;
                    player.rigidbody.velocity += acceleration * Time.deltaTime;
                }
                Vector3 tangentDamping = -player.rigidbody.velocity * player.tangentDamp;
                player.rigidbody.velocity += tangentDamping * Time.deltaTime;
                if (player.rigidbody.angularVelocity.magnitude < player.maxRotationSpeed)
                {
                    Vector3 acceleration = player.planetTransform.ToUniverse(player.NormalAcceleration * player.normalAcceleration * Vector3.up);
                    player.rigidbody.angularVelocity += acceleration * Time.deltaTime;
                }
                Vector3 normalDamping = -player.rigidbody.angularVelocity * player.tangentDamp;
                player.rigidbody.angularVelocity += normalDamping * Time.deltaTime;
            }
            public void Move() => currentMove?.Invoke();
        }
    }
}