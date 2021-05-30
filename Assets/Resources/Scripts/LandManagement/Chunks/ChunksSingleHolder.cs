using UnityEngine;

namespace Biosearcher.LandManagement.Chunks
{
    public class ChunksSingleHolder : IChunkHolder
    {
        protected Chunk child;

        protected internal bool IsInitialized { get; protected set; } = false;
        protected internal Vector3Int Position { get; protected set; }
        protected internal ChunkTracker ChunkTracker { get; protected set; }
        protected internal GeometryManager GeometryManager { get; protected set; }

        public ChunksSingleHolder(Vector3Int position, ChunkTracker chunkTracker, GeometryManager geometryManager)
        {
            Position = position;
            ChunkTracker = chunkTracker;
            GeometryManager = geometryManager;
        }

        public void StartInitializing()
        {
            child = new ChunkWithGeometry(Position, Chunk.Settings.MaxHierarchySize, this, ChunkTracker, GeometryManager);
            child.Initialized += () =>
            {
                IsInitialized = true;
                child.Instantiate();
            };
            child.StartInitializing();
        }

        public void DrawGizmos() => child.DrawGizmos();

        public void ReplaceChild(Chunk currentChunk, Chunk newChunk)
        {
            if (child == currentChunk)
            {
                child = newChunk;
            }
        }

        public void TryUnite(ChunkWithGeometry child) { }
        public void TryUnUnite(ChunkWithGeometry child) { }
    }
}