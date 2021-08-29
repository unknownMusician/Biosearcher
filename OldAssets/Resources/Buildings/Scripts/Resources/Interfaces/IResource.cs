using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Common.Interfaces;

namespace Biosearcher.Buildings.Resources.Interfaces
{
    public interface IResource<T> : System.IComparable<T>,
        IAddable<T>, ISubtractable<T>,
        IDivisibleBySelf<T>, IDivisibleByFloat<T>,
        IMultipliable<float, T>, IAverageable<T>
    { }
}