using Biosearcher.HDRP;
using UnityEngine;

namespace Biosearcher.Planet.Lighting
{
    public class MainStar : MonoBehaviour
    {
        [SerializeField] protected Light starLight;

        protected void FixedUpdate()
        {
            Vector3 planetPosition = Vector3.zero;

            transform.rotation = Quaternion.FromToRotation(Vector3.forward, planetPosition - transform.position);
            SkyGameManager.mainStarPosition = transform.position;
        }
    }
}
