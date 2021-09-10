namespace Biosearcher.Player.Interactions.Default
{
    public sealed class InsertableMonoBehaviour : GrabbableMonoBehaviour, IInsertable
    {
        public void HandleInsert() => this.HandleInsertDefault();

        public bool TryAlignWith(IInsertFriendly insertFriendly) => this.TryAlignWithGeneric(insertFriendly);
        public bool TryInsertIn(IInsertFriendly insertFriendly) => this.TryInsertInGeneric(insertFriendly);
    }
}