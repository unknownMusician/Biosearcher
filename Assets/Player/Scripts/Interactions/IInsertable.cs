namespace Biosearcher.Player.Interactions
{
    public interface IInsertable : IGrabbable
    {
        void HandleInsert();
        
        bool TryInsertIn(IInsertFriendly insertFriendly);
        bool TryAlignWith(IInsertFriendly insertFriendly);
    }
}
