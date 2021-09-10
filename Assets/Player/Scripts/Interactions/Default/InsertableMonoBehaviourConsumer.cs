using UnityEngine;

namespace Biosearcher.Player.Interactions.Default
{
    public sealed class InsertableMonoBehaviourConsumer : MonoBehaviour, IInsertFriendly<InsertableMonoBehaviour>
    {
        [SerializeField] private Vector3 _alignLocalPosition;
        [SerializeField] private Quaternion _alignRotation;

        public bool TryAlign(InsertableMonoBehaviour insertable)
        {
            insertable.transform.SetPositionAndRotation(transform.position + _alignLocalPosition, _alignRotation);
            return true;
        }

        public bool TryInsert(InsertableMonoBehaviour insertable)
        {
            insertable.transform.SetPositionAndRotation(transform.position + _alignLocalPosition, _alignRotation);
            insertable.transform.SetParent(transform);

            return true;
        }
    }
}