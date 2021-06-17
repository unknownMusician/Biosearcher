namespace Biosearcher.Buildings.Resources.Interfaces
{
    public interface IResource<T> : System.IComparable<T>,
        IAddable<T>, ISubtractable<T>,
        IMultipliable<T>, IDividable<T>
    {
        float Value { get; set; }
    }
}