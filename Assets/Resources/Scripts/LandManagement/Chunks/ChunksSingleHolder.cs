using UnityEngine;

namespace Biosearcher.LandManagement.Chunks
{
    public class ChunksSingleHolder : IChunkHolder
    {
        protected Chunk _child;

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
            _child = new ChunkWithGeometry(Position, Chunk.Settings.MaxHierarchySize, this, ChunkTracker, GeometryManager);
            _child.Initialized += () =>
            {
                IsInitialized = true;
                _child.Instantiate();
            };
            _child.StartInitializing();
        }

        public void DrawGizmos() => _child.DrawGizmos();

        public void ReplaceChild(Chunk currentChunk, Chunk newChunk)
        {
            if (_child == currentChunk)
            {
                _child = newChunk;
            }
        }

        public void TryUnite(ChunkWithGeometry child) { }
        public void TryUnUnite(ChunkWithGeometry child) { }
    }
}