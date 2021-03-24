using Biosearcher.Planet.Managing;
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
            ChunkManager chunkManager = planetTransform.ChunkManager;
            if (chunkManager != null)
            {
                Vector3 gravityDirection = (chunkManager.PlanetPosition - transform.position).normalized;
                rigidbody.AddForce(gravityDirection * planetTransform.ChunkManager.GravityScale);
            }
        }
    }
}