using UnityEngine;
using Biosearcher.Player.Interactions;

namespace Biosearcher.Plants
{
    public sealed class SeedConsumer : MonoBehaviour, IInsertFriendly<Seed>
    {
        [SerializeField] private GameObject _seedSlot;

        private void Align(Seed insertableSeed)
        {
            insertableSeed.transform.SetPositionAndRotation(_seedSlot.transform.position, _seedSlot.transform.rotation);
        }

        public bool TryInsert(Seed insertableSeed)
        {
            Align(insertableSeed);
            insertableSeed.transform.SetParent(_seedSlot.transform);
            return true;
        }
        public bool TryAlign(Seed insertableSeed)
        {
            Align(insertableSeed);
            return true;
        }
    }
}
