using Biosearcher.Buildings.Resources.Interfaces;

namespace Biosearcher.Buildings.Types.Interfaces
{
    public interface IResourceReceiver<TResource> : IResourceMover<TResource> where TResource : IResource<TResource>, new()
    {
        TResource MaxPossibleReceived { get; }
        TResource CurrentPossibleReceived { get; }
        void Receive(TResource resource);
    }
}
