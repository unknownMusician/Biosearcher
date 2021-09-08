using Biosearcher.Refactoring;

namespace Biosearcher.Player.Interactions
{
    public interface IInsertFriendly { }

    public interface IInsertFriendly<TInsertable> : IInsertFriendly where TInsertable : IInsertable
    {
        bool TryInsert(TInsertable insertable);
        bool TryAlign(TInsertable insertable);
    }
}
