namespace Biosearcher.Player
{
    public interface IInsertable : IGrabbable
    {
        void HandleInsert();
        
        bool TryInsertIn(IInsertFriendly insertFriendly);
        bool TryAlignWith(IInsertFriendly insertFriendly);
    }
}
