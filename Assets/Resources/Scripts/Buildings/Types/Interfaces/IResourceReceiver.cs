namespace Biosearcher.Buildings.Types.Interfaces
{
    public interface IResourceReceiver<TResource>
    {
        public TResource MaxPossibleReceived { get; }
        public TResource CurrentPossibleReceived { get; }
        public void Receive(TResource resource);
    }
}
