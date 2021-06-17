using Biosearcher.Buildings.Resources.Interfaces;

namespace Biosearcher.Buildings.Types.Interfaces
{
    public interface IResourceReceiver<TResource> : IResourceMover<TResource> where TResource : IResource<TResource>, new()
    {
        public TResource MaxPossibleReceived { get; }
        public TResource CurrentPossibleReceived { get; }
        public void Receive(TResource resource);
    }
}
