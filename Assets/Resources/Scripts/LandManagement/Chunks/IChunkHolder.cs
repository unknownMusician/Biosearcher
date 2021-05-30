
namespace Biosearcher.LandManagement.Chunks
{
    public interface IChunkHolder
    {
        void ReplaceChild(Chunk currentChunk, Chunk newChunk);
        void StartInitializing();
        void DrawGizmos();

        void TryUnite(ChunkWithGeometry child);
        void TryUnUnite(ChunkWithGeometry child);
    }
}