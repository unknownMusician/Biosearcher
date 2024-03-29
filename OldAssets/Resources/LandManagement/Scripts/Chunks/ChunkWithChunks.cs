﻿using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.LandManagement.Chunks
{
    public class ChunkWithChunks : Chunk, IChunkHolder
    {
        protected Chunk[] _children;
        protected HashSet<ChunkWithGeometry> _needToUniteIntoParent;

        protected internal ChunkWithChunks(Vector3Int position, int hierarchySize, IChunkHolder parent, ChunkTracker chunkTracker, GeometryManager geometryManager)
            : base(position, hierarchySize, parent, chunkTracker, geometryManager) { }
        protected internal ChunkWithChunks(Chunk fromChunk) : base(fromChunk) { }

        private ChunkWithGeometry CreateChunkWithGeometry(int chunkNumber)
        {
            int x = (chunkNumber & (1 << 0)) >> 0;
            int y = (chunkNumber & (1 << 1)) >> 1;
            int z = (chunkNumber & (1 << 2)) >> 2;

            int newHierarchySize = HierarchySize - 1;
            int newActualSize = HierarchySize2WorldSize(newHierarchySize);
            int deltaX = (newActualSize >> 1) * ((x << 1) - 1);
            int deltaY = (newActualSize >> 1) * ((y << 1) - 1);
            int deltaZ = (newActualSize >> 1) * ((z << 1) - 1);
            Vector3Int newPosition = Position + new Vector3Int(deltaX, deltaY, deltaZ);

            return new ChunkWithGeometry(newPosition, newHierarchySize, this, ChunkTracker, GeometryManager);
        }

        public override void StartInitializing()
        {
            int initializedCount = 0;
            _children = new Chunk[8];
            _needToUniteIntoParent = new HashSet<ChunkWithGeometry>();

            for (int i = 0; i < 8; i++)
            {
                var child = CreateChunkWithGeometry(i);

                _children[i] = child;
                child.Initialized += () =>
                {
                    if (++initializedCount == 8)
                    {
                        IsInitialized = true;
                        Initialized?.Invoke();
                    }
                };
                child.StartInitializing();
            }
        }

        protected internal override void Instantiate()
        {
            foreach (Chunk child in _children)
            {
                child.Instantiate();
            }
        }

        public override void DrawGizmos()
        {
            base.DrawGizmos();

            foreach (Chunk child in _children)
            {
                child.DrawGizmos();
            }
        }

        public void TryUnite(ChunkWithGeometry child)
        {
            if (_needToUniteIntoParent.Add(child) && _needToUniteIntoParent.Count == 8)
            {
                Unite();
            }
        }
        public void TryUnUnite(ChunkWithGeometry child) => _needToUniteIntoParent.Remove(child);

        protected void Unite()
        {
            foreach (ChunkWithGeometry chunk in _needToUniteIntoParent)
            {
                chunk.Hide();
            }
            var newChunk = new ChunkWithGeometry(this);
            newChunk.Initialized += () =>
            {
                foreach (ChunkWithGeometry chunk in _needToUniteIntoParent)
                {
                    chunk.Destroy();
                }
                IsInitialized = false;
                Parent.ReplaceChild(this, newChunk);
                newChunk.Instantiate();
            };
            newChunk.StartInitializing();
        }

        public void ReplaceChild(Chunk currentChunk, Chunk newChunk)
        {
            for (int i = 0; i < 8; i++)
            {
                if (_children[i] == currentChunk)
                {
                    _children[i] = newChunk;
                    return;
                }
            }
        }
    }
}