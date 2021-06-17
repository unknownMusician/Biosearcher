using Biosearcher.Buildings.Resources.Interfaces;

namespace Biosearcher.Buildings.Types.Interfaces
{
    public interface IResourceProducer<TResource> : IResourceMover<TResource> where TResource : IResource<TResource>, new()
    {
        public TResource MaxPossibleProduced { get; }
        public TResource CurrentPossibleProduced { get; }
        public TResource Produce();
    }
}
