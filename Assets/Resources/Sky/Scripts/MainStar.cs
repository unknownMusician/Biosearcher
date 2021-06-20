using UnityEngine;

namespace Biosearcher.Sky
{
    public class MainStar : MonoBehaviour
    {
        [SerializeField] protected Light _starLight;

        protected void FixedUpdate()
        {
            // todo
            Vector3 planetPosition = Vector3.zero;

            transform.rotation = Quaternion.FromToRotation(Vector3.forward, planetPosition - transform.position);
            SkyGameManager.mainStarPosition = transform.position;
        }
    }
}
