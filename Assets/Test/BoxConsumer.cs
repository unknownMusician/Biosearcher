using Biosearcher.Player.Interactions;
using UnityEngine;

namespace Biosearcher.Test
{
    public sealed class BoxConsumer : MonoBehaviour, IInsertFriendly<Seed>
    {
        [SerializeField] private Vector3 _alignLocalPosition;
        [SerializeField] private Quaternion _alignRotation;

        public bool TryAlign(Seed insertable)
        {
            insertable.transform.SetPositionAndRotation(transform.position + _alignLocalPosition, _alignRotation);
            return true;
        }

        public bool TryInsert(Seed insertable)
        {
            insertable.transform.SetPositionAndRotation(transform.position + _alignLocalPosition, _alignRotation);
            insertable.transform.SetParent(transform);

            return true;
        }
    }
}