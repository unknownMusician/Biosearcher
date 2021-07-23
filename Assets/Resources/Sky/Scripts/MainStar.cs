using Biosearcher.Planets;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Sky
{
    public class MainStar : MonoBehaviour
    {
        [SerializeField] protected Light _starLight;

        [NeedsRefactor]
        protected void FixedUpdate()
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, Planet.Current.Center - transform.position);
            // todo
            //SkyGameManager.mainStarPosition = transform.position;
        }
    }
}
