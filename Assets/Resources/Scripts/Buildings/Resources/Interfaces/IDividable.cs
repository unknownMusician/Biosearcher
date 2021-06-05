namespace Biosearcher.Buildings.Resources.Interfaces
{
    public interface IDividable<TResource>
    {
        public TResource Divide(TResource a);
    }
}