using Biosearcher.Player.Interactions;

namespace Biosearcher.Test
{
    public class InsertableBox : GrabbableBox, IInsertable
    {
        public void HandleInsert() => this.HandleInsertDefault();

        public bool TryAlignWith(IInsertFriendly insertFriendly) => this.TryAlignWithGeneric(insertFriendly);
        public bool TryInsertIn(IInsertFriendly insertFriendly) => this.TryInsertInGeneric(insertFriendly);
    }
}