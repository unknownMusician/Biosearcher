using Biosearcher.Buildings.Resources;
using Biosearcher.Buildings.Resources.Interfaces;

namespace Biosearcher.Buildings.Types.Interfaces
{
    public interface IResourceMover<TResource> where TResource : IResource<TResource>, new()
    {
        Network<TResource> Network { get; set; }
    }
}