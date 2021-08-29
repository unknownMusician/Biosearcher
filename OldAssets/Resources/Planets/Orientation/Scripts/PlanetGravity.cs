using UnityEngine;

namespace Biosearcher.Planets.Orientation
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlanetTransform))]
    public class PlanetGravity : MonoBehaviour
    {
        protected Rigidbody _rigidbody;
        protected PlanetTransform _planetTransform;

        protected void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _planetTransform = GetComponent<PlanetTransform>();
        }

        protected void FixedUpdate()
        {
            Vector3 planetPosition = Vector3.zero;
            float planetGravityScale = 9.8f;
            Vector3 gravityDirection = (planetPosition - transform.position).normalized;
            _rigidbody.AddForce(gravityDirection * planetGravityScale * _rigidbody.mass);
        }
    }
}