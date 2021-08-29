using Biosearcher.Buildings.Resources.Interfaces;

namespace Biosearcher.Buildings.Types.Interfaces
{
    public interface IResourceProducer<TResource> : IResourceMover<TResource> where TResource : IResource<TResource>, new()
    {
        TResource MaxPossibleProduced { get; }
        TResource CurrentPossibleProduced { get; }
        TResource Produce();
    }
}
