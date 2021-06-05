namespace Biosearcher.Buildings.Types.Interfaces
{
    public interface IResourceProducer<out TResource>
    {
        public TResource MaxPossibleProduced { get; }
        public TResource CurrentPossibleProduced { get; }
        public TResource Produce();
    }
}
