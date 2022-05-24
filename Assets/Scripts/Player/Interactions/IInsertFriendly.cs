using System;

namespace Biosearcher.Player.Interactions
{
    public interface IInsertFriendly
    {
        public event Action<IInsertable> OnInsert;
        
        public bool CanInsert(IInsertable insertable);
        
        public void HandleInsert(IInsertable insertable);
        public void HandleAlignStart(IInsertable insertable);
        public void HandleAlignEnd(IInsertable insertable);
    }
}
