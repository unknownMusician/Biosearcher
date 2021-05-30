using UnityEngine;

namespace Biosearcher.Planet.Orientation
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlanetTransform))]
    public class PlanetGravity : MonoBehaviour
    {
        protected new Rigidbody rigidbody;
        protected PlanetTransform planetTransform;

        protected void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            planetTransform = GetComponent<PlanetTransform>();
        }

        protected void FixedUpdate()
        {
            Vector3 planetPosition = Vector3.zero;
            float planetGravityScale = 9.8f;
            Vector3 gravityDirection = (planetPosition - transform.position).normalized;
            rigidbody.AddForce(gravityDirection * planetGravityScale * rigidbody.mass);
        }
    }
}