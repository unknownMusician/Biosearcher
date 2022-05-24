using System;

namespace Biosearcher.Player.Interactions
{
    public interface IInsertable : IGrabbable
    {
        public event Action<IInsertFriendly> OnInsert;
        public event Action<IInsertFriendly> OnAlignStart;
        public event Action<IInsertFriendly> OnAlignEnd;

        public bool CanBeInserted(IInsertFriendly insertFriendly);

        public void HandleInsert(IInsertFriendly insertFriendly);
        public void HandleAlignStart(IInsertFriendly insertFriendly);
        public void HandleAlignEnd(IInsertFriendly insertFriendly);
    }
}
