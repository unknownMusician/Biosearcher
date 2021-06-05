namespace Biosearcher.Buildings.Resources.Interfaces
{
    public interface IMultipliable<TResource>
    {
        public TResource Multiply(TResource a);
    }
}
