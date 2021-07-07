namespace Biosearcher.Common.Interfaces
{
    public interface IAverageable<T>
    {
        T Average(T other);
    }

    public static class AverageableExtensions
    {
        public static float Average(this float f1, float f2) => (f1 + f2) * 0.5f;
    }
}