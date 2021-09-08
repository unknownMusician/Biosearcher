using Biosearcher.Player.Interactions;
using UnityEngine;

namespace Biosearcher.Test
{
    public sealed class BoxConsumer : MonoBehaviour, IInsertFriendly<InsertableBox>
    {
        [SerializeField] private Vector3 _alignLocalPosition;
        [SerializeField] private Quaternion _alignRotation;

        public bool TryAlign(InsertableBox insertable)
        {
            insertable.transform.SetPositionAndRotation(transform.position + _alignLocalPosition, _alignRotation);
            return true;
        }

        public bool TryInsert(InsertableBox insertable)
        {
            insertable.transform.SetPositionAndRotation(transform.position + _alignLocalPosition, _alignRotation);
            insertable.transform.SetParent(transform);

            return true;
        }
    }
}