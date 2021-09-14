using UnityEngine;
using Biosearcher.Player.Interactions;

namespace Biosearcher.Plants
{
    public sealed class SeedConsumer : MonoBehaviour, IInsertFriendly<Seed>
    {
        [SerializeField] private GameObject _seedSlot;

        public bool TryInsert(Seed insertableSeed)
        {
            insertableSeed.transform.SetPositionAndRotation(_seedSlot.transform.position, _seedSlot.transform.rotation);
            insertableSeed.transform.SetParent(_seedSlot.transform);
            return true;
        }
        public bool TryAlign(Seed insertableSeed)
        {
            insertableSeed.transform.SetPositionAndRotation(_seedSlot.transform.position, _seedSlot.transform.rotation);
            return true;
        }
    }
}
