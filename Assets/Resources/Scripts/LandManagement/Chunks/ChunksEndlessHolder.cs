using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Biosearcher.LandManagement.Chunks
{
    public class ChunksEndlessHolder : IChunkHolder
    {
        Dictionary<Vector3Int, Chunk> children;
        List<ChunkWithGeometry> needToBeDestroyed;

        protected internal Vector3Int Position { get; protected set; }
        protected internal int HierarchySize { get; protected set; }
        protected internal ChunkTracker ChunkTracker { get; protected set; }
        protected internal GeometryManager GeometryManager { get; protected set; }

        public ChunksEndlessHolder(Vector3Int position, ChunkTracker chunkTracker, GeometryManager geometryManager)
        {
            Position = position;
            HierarchySize = Chunk.Settings.MaxHierarchySize;
            ChunkTracker = chunkTracker;
            GeometryManager = geometryManager;
        }

        public void StartInitializing()
        {
            children = new Dictionary<Vector3Int, Chunk>();
            needToBeDestroyed = new List<ChunkWithGeometry>();

            var chunk = new ChunkWithGeometry(Position, HierarchySize, this, ChunkTracker, GeometryManager);
            children.Add(Position, chunk);
            chunk.Initialized += chunk.Instantiate;
            chunk.StartInitializing();
        }

        public void DrawGizmos()
        {
            foreach (KeyValuePair<Vector3Int, Chunk> pair in children)
            {
                pair.Value.DrawGizmos();
            }
        }

        protected void TryDestroyThatNeedsToBeDestroyed()
        {
            Queue<ChunkWithGeometry> destroying = new Queue<ChunkWithGeometry>();
            foreach (ChunkWithGeometry chunk in needToBeDestroyed)
            {
                if (chunk.IsInitialized)
                {
                    destroying.Enqueue(chunk);
                }
            }
            int size = destroying.Count;
            for (int i = 0; i < size; i++)
            {
                ChunkWithGeometry destroingChunk = destroying.Dequeue();
                needToBeDestroyed.Remove(destroingChunk);
                destroingChunk.Destroy();
            }
        }

        protected void OnChildDivided(ChunkWithChunks chunk)
        {
            TryDestroyThatNeedsToBeDestroyed();
            for (int i = 0; i < 27; i++)
            {
                if (i == 13)
                {
                    continue;
                }
                int x = (i % 3) - 1;
                int y = ((i - x) / 3) % 3 - 1;
                int z = ((i - y) / 9) % 3 - 1;

                Vector3Int newPosition = chunk.Position + new Vector3Int(x, y, z) * Chunk.HierarchySize2WorldSize(HierarchySize);
                if (!children.ContainsKey(newPosition))
                {
                    var newChunk = new ChunkWithGeometry(newPosition, HierarchySize, this, ChunkTracker, GeometryManager);
                    children.Add(newPosition, newChunk);
                    newChunk.Initialized += newChunk.Instantiate;
                    newChunk.StartInitializing();
                }
            }
        }

        protected void DestroyIfCan(ChunkWithGeometry chunk)
        {
            for (int i = 0; i < 27; i++)
            {
                if (i == 13)
                {
                    continue;
                }
                int x = (i % 3) - 1;
                int y = ((i - x) / 3) % 3 - 1;
                int z = ((i - y) / 9) % 3 - 1;

                Vector3Int newPosition = chunk.Position + new Vector3Int(x, y, z) * Chunk.HierarchySize2WorldSize(HierarchySize);
                if (children.ContainsKey(newPosition) && children[newPosition] is ChunkWithChunks)
                {
                    return;
                }
            }
            children.Remove(chunk.Position);
            chunk.Hide();
            if (!chunk.IsInitialized)
            {
                needToBeDestroyed.Add(chunk);
                return;
            }
            chunk.Destroy();
        }

        protected void OnChildUnited(ChunkWithGeometry chunk)
        {
            TryDestroyThatNeedsToBeDestroyed();
            for (int i = 0; i < 27; i++)
            {
                if (i == 13)
                {
                    continue;
                }
                int x = (i % 3) - 1;
                int y = ((i - x) / 3) % 3 - 1;
                int z = ((i - y) / 9) % 3 - 1;

                Vector3Int newPosition = chunk.Position + new Vector3Int(x, y, z) * Chunk.HierarchySize2WorldSize(HierarchySize);
                if (children.ContainsKey(newPosition) && children[newPosition] is ChunkWithGeometry geometryChunk)
                {
                    DestroyIfCan(geometryChunk);
                }
            }
            DestroyIfCan(chunk);
        }

        public void ReplaceChild(Chunk currentChunk, Chunk newChunk)
        {
            Vector3Int? chunkPosition = null;
            foreach (KeyValuePair<Vector3Int, Chunk> pair in children)
            {
                if (pair.Value == currentChunk)
                {
                    chunkPosition = pair.Key;
                    break;
                }
            }
            if (chunkPosition != null)
            {
                children[(Vector3Int)chunkPosition] = newChunk;

                if (newChunk is ChunkWithGeometry geometryChunk)
                {
                    OnChildUnited(geometryChunk);
                }
                else if (newChunk is ChunkWithChunks chunksChunk)
                {
                    OnChildDivided(chunksChunk);
                }
            }
        }

        public void TryUnite(ChunkWithGeometry child) { }
        public void TryUnUnite(ChunkWithGeometry child) { }
    }
}