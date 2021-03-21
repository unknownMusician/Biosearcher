using System.Collections;
using UnityEngine;

namespace Biosearcher.Land.Managing
{
    public abstract class Chunk
    {
        #region Static fields
        // todo: make formula
        public static readonly float[,] size2DistanceRange =
        {
            {  0,  12}, // 0 → 1
            { 12,  24}, // 1 → 2
            { 24,  48}, // 2 → 4
            { 48,  96}, // 3 → 8
            { 96, 192}, // 4 → 16
        };

        public static readonly float[] size2UpdatePeriod =
        {
            0.5f, // 0 → 1
               1, // 1 → 2
               2, // 2 → 4
               4, // 3 → 8
               8, // 4 → 16
        };

        // todo: size2ActualSize

        #endregion

        protected ChunkHolder holder;

        public Vector3Int Position { get; protected set; }
        public int Size { get; protected set; }

        public Chunk(Vector3Int position, int size, ChunkHolder holder)
        {
            this.holder = holder;
            Position = position;
            Size = size;
        }

        public abstract void DrawGizmos();

        public static Chunk Create(Vector3Int chunkPosition, int size, ChunkHolder holder, Vector3 triggerPosition)
        {
            float distanceToTrigger = (triggerPosition - chunkPosition).magnitude;

            if (size < 0)
            {
                Debug.LogError("Size should not go below zero.");
            }

            if (distanceToTrigger < size2DistanceRange[size, 0] && size > 0)
            {
                return new ChunkWithChunks(chunkPosition, size, holder);
            }
            else
            {
                return new ChunkWithGeometry(chunkPosition, size, holder);
            }
        }
    }
}