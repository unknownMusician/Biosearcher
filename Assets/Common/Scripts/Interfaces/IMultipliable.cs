namespace Biosearcher.Common.Interfaces
{
    public interface IMultipliable<T> : IMultipliable<T, T> { }

    public interface IMultipliable<TMultiplied, TResult>
    {
        TResult Multiply(TMultiplied m);
    }
}
