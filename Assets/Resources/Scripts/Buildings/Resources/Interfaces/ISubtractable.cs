namespace Biosearcher.Buildings.Resources.Interfaces
{
    public interface ISubtractable<TResource>
    {
        public TResource Subtract(TResource a);
    }
}
