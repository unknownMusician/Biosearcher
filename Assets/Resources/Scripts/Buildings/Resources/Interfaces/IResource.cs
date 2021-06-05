namespace Biosearcher.Buildings.Resources.Interfaces
{
    public interface IResource<TResource> : IAddable<TResource>, ISubtractable<TResource>, 
        IMultipliable<TResource>, IDividable<TResource>,
        System.IComparable<TResource>
    {
        public float Value { get; set;  }
    }
}