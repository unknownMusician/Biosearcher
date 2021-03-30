using Biosearcher.HDRP;
using Biosearcher.Planet.Managing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.Planet.Lighting
{
    public class MainStar : MonoBehaviour
    {
        [SerializeField] protected ChunkManager chunkManager;
        [SerializeField] protected Light starLight;

        protected void FixedUpdate()
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, chunkManager.PlanetPosition - transform.position);
            SkyGameManager.mainStarPosition = transform.position;
        }
    }
}
