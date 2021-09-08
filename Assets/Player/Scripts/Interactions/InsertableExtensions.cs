using UnityEngine;

namespace Biosearcher.Player.Interactions
{
    public static class InsertableExtensions
    {
        public static void HandleInsertDefault<TInsertableBehaviour>(this TInsertableBehaviour insertableBehaviour)
            where TInsertableBehaviour : MonoBehaviour, IInsertable
        {
            insertableBehaviour.HandleDropDefault();
        }

        /// <summary>
        /// Use IsCompatible() instead unless you are trying to call this from IsCompatible()
        /// </summary>
        public static bool IsCompatibleGeneric<TInsertable>(this TInsertable insertable, IInsertFriendly insertFriendly)
            where TInsertable : IInsertable
        {
            return insertFriendly is IInsertFriendly<TInsertable>;
        }

        /// <summary>
        /// Use TryInsertIn() instead unless you are trying to call this from TryInsertIn()
        /// </summary>
        public static bool TryInsertInGeneric<TInsertable>(this TInsertable insertable, IInsertFriendly insertFriendly)
            where TInsertable : IInsertable
        {
            return insertable.IsCompatibleGeneric(insertFriendly) && (insertFriendly as IInsertFriendly<TInsertable>).TryInsert(insertable);
        }

        /// <summary>
        /// Use TryAlignWith() instead unless you are trying to call this from TryAlignWith()
        /// </summary>
        public static bool TryAlignWithGeneric<TInsertable>(this TInsertable insertable, IInsertFriendly insertFriendly)
            where TInsertable : IInsertable
        {
            return insertable.IsCompatibleGeneric(insertFriendly) && (insertFriendly as IInsertFriendly<TInsertable>).TryAlign(insertable);
        }
    }
}