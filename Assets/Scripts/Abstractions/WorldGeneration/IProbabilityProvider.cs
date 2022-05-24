namespace Biosearcher.WorldGeneration
{
    public interface IProbabilityProvider<T>
    {
        public float Get(ref T info);
    }
}
