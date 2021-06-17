namespace Biosearcher.Buildings.Resources.Interfaces
{
    public interface IResource<TResource> : IAddable<TResource>, ISubtractable<TResource>, 
        IMultipliable<TResource>, IDividable<TResource>,
        System.IComparable<TResource>
    {
        float Value { get; set; }
    }
}