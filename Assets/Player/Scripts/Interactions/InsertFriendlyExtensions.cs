namespace Biosearcher.Player.Interactions
{
    public static class InsertFriendlyExtensions
    {
        /// <summary>
        /// Use TryInsert() instead unless you are trying to call this from TryInsert()
        /// </summary>
        public static bool TryInsertGeneric<TInsertable>(this IInsertFriendly<TInsertable> insertFriendly, IInsertable insertable) 
            where TInsertable : IInsertable
        {
            return insertable.IsCompatibleGeneric(insertFriendly) && insertFriendly.TryInsert((TInsertable)insertable);
        }
    }
}