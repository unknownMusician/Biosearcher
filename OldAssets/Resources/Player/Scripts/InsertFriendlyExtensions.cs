namespace Biosearcher.Player
{
    public static class InsertFriendlyExtensions
    {
        /// <summary>
        /// Use IInsertable.IsCompatible() instead
        /// </summary>
        //public static bool IsCompatible(this IInsertFriendly insertFriendly, IInsertable insertable)
        //{
        //    return insertable.IsCompatible(insertFriendly);
        //}

        /// <summary>
        /// Use TryInsert() instead unless you are trying to call this from TryInsert()
        /// </summary>
        public static bool TryInsertGeneric<TInsertable>(this IInsertFriendly<TInsertable> insertFriendly, IInsertable insertable) 
            where TInsertable : IInsertable
        {
            if (insertable.IsCompatibleGeneric(insertFriendly))
            {
                return insertFriendly.TryInsert((TInsertable)insertable);
            }
            return false;
        }
    }
}