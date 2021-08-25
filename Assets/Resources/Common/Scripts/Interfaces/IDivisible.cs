using Biosearcher.Refactoring;

namespace Biosearcher.Common.Interfaces
{
    [NeedsRefactor(Needs.Remove)]
    public interface IDivisible<TDivided, TResult>
    {
        TResult Divide(TDivided d);
    }

    public interface IDivisibleBySelf<T>
    {
        float Divide(T d);
    }

    public interface IDivisibleByFloat<T>
    {
        T Divide(float d);
    }
}