namespace Biosearcher.Player.Interactions.Default
{
    public class InsertableMonoBehaviour : GrabbableMonoBehaviour, IInsertable
    {
        public virtual void HandleInsert() => this.HandleInsertDefault();

        public bool TryAlignWith(IInsertFriendly insertFriendly) => this.TryAlignWithGeneric(insertFriendly);
        public bool TryInsertIn(IInsertFriendly insertFriendly) => this.TryInsertInGeneric(insertFriendly);
    }
}