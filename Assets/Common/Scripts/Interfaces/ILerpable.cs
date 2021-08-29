namespace Biosearcher.Common.Interfaces
{
    public interface ILerpable<T> : IAverageable<T>
    {
        T Lerp(T finValue, float t);
    }
}
