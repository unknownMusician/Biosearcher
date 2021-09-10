using Biosearcher.Player.Interactions;
using UnityEngine;

namespace Biosearcher.Plants
{
    public class GreenHouse : MonoBehaviour, IInsertFriendly<Seed>
    {
        [SerializeField] private Vector3 _alignLocalPosition;

        public bool TryAlign(Seed seed)
        {
            seed.transform.SetPositionAndRotation(transform.position + _alignLocalPosition, transform.rotation);
            return true;
        }

        public bool TryInsert(Seed seed)
        {
            seed.transform.SetPositionAndRotation(transform.position + _alignLocalPosition, transform.rotation);
            seed.transform.SetParent(transform);

            return true;
        }
    }
}
