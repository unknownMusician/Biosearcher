using UnityEngine;

namespace Biosearcher.Player
{
    public static class InsertableExtensions
    {
        public static void HandleInsertDefault<TInsertable>(this TInsertable insertable, LayerMask realMask) 
            where TInsertable : MonoBehaviour, IInsertable
        {
            insertable.HandleDropDefault(realMask);
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
            if (insertable.IsCompatibleGeneric(insertFriendly))
            {
                return (insertFriendly as IInsertFriendly<TInsertable>).TryInsert(insertable);
            }
            return false;
        }

        /// <summary>
        /// Use TryAlignWith() instead unless you are trying to call this from TryAlignWith()
        /// </summary>
        public static bool TryAlignWithGeneric<TInsertable>(this TInsertable insertable, IInsertFriendly insertFriendly)
            where TInsertable : IInsertable
        {
            if (insertable.IsCompatibleGeneric(insertFriendly))
            {
                return (insertFriendly as IInsertFriendly<TInsertable>).TryAlign(insertable);
            }
            return false;
        }
    }
}