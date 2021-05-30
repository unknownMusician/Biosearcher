﻿using UnityEngine;
using UnityEngine.Events;

namespace Biosearcher.LandManagement.Chunks
{
    public class ChunkWithGeometry : Chunk
    {
        protected Geometry geometry;

        protected internal ChunkWithGeometry(Vector3Int position, int hierarchySize, IChunkHolder parent, ChunkTracker chunkTracker, GeometryManager geometryManager)
            : base(position, hierarchySize, parent, chunkTracker, geometryManager) { }
        protected internal ChunkWithGeometry(Chunk fromChunk) : base(fromChunk) { }

        public override void StartInitializing() => GeometryManager.Create(this);

        protected internal override void Instantiate()
        {
            geometry = GeometryManager.InstantiateChunk(geometry, this);
            Show();
        }

        public override void DrawGizmos()
        {
            base.DrawGizmos();

            float colorValue = (HierarchySize / 8f) * 2f * Mathf.PI;
            Gizmos.color = new Color(Mathf.Sin(colorValue), Mathf.Cos(colorValue), -Mathf.Sin(colorValue));
            int fullSize = HierarchySize2WorldSize(HierarchySize);
            Gizmos.DrawWireCube(Position, new Vector3(fullSize, fullSize, fullSize));
        }

        protected internal void TryUniteIntoParent() => Parent.TryUnite(this);
        protected internal void TryUnUniteIntoParent() => Parent.TryUnUnite(this);

        protected internal void Initialize(Geometry geometry)
        {
            this.geometry = geometry;
            IsInitialized = true;
            Initialized?.Invoke();
        }

        protected internal void Divide()
        {
            Hide();

            var newChunk = new ChunkWithChunks(this);
            newChunk.Initialized += () =>
            {
                Destroy();
                Parent.ReplaceChild(this, newChunk);
                newChunk.Instantiate();
            };
            newChunk.StartInitializing();
        }

        protected internal void Hide() => ChunkTracker.UnTrackChunk(this);
        protected void Show() => ChunkTracker.TrackChunk(this);
        protected internal void Destroy()
        {
            GeometryManager.Clear(geometry, this);
            geometry = default;
            IsInitialized = false;
        }
    }
}