using System;
using UnityEngine;

namespace Biosearcher.Player.Interactions
{
    public static class InsertAction
    {
        public static bool CanInsert(IInsertFriendly insertFriendly, IInsertable insertable)
        {
            return insertFriendly.CanInsert(insertable) && insertable.CanBeInserted(insertFriendly);
        }

        public static bool CanInsert<TInsertable>(
            this IInsertFriendly insertFriendly, IInsertable insertable, out TInsertable tInsertable
        )
        {
            if (insertable is TInsertable genericInsertable && insertFriendly.CanInsert(insertable))
            {
                tInsertable = genericInsertable;

                return true;
            }

            tInsertable = default;

            return false;
        }

        public static void ThrowIfCannotInsert<TInsertable>(
            this IInsertFriendly insertFriendly, IInsertable insertable, out TInsertable tInsertable
        )
        {
            if (!insertFriendly.CanInsert(insertable, out tInsertable))
            {
                throw new ArgumentException("IInsertable argument is passed even though it cannot be.");
            }
        }

        public static bool TryInsert(IInsertFriendly insertFriendly, IInsertable insertable)
        {
            if (!CanInsert(insertFriendly, insertable))
            {
                return false;
            }

            insertFriendly.HandleInsert(insertable);
            insertable.HandleInsert(insertFriendly);

            return true;
        }

        public static bool TryAlignStart(IInsertFriendly insertFriendly, IInsertable insertable)
        {
            if (!CanInsert(insertFriendly, insertable))
            {
                return false;
            }

            insertFriendly.HandleAlignStart(insertable);
            insertable.HandleAlignStart(insertFriendly);

            return true;
        }

        public static void HandleAlignEnd(IInsertFriendly insertFriendly, IInsertable insertable)
        {
            insertFriendly.HandleAlignEnd(insertable);
            insertable.HandleAlignEnd(insertFriendly);
        }
    }
}
