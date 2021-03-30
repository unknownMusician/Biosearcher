using System.Collections;
using UnityEngine;

namespace Biosearcher.Planet.Managing
{
    public abstract class Chunk
    {
        #region Static fields

        //protected internal static readonly float[,] size2DistanceRange =
        //{
        //    {  0,  12}, // 0 → 1
        //    { 12,  24}, // 1 → 2
        //    { 24,  48}, // 2 → 4
        //    { 48,  96}, // 3 → 8
        //    { 96, 192}, // 4 → 16
        //};
        protected internal static (float min, float max) Size2DistanceRange(int size)
        {
            if (size == 0)
            {
                return (0, 12 * 2);
            }
            return (Size2ActualSize(size), Size2ActualSize(size + 1));
        }

        //protected internal static readonly float[] size2UpdatePeriod =
        //{
        //    0.5f, // 0 → 1
        //       1, // 1 → 2
        //       2, // 2 → 4
        //       4, // 3 → 8
        //       8, // 4 → 16
        //};
        protected internal static float Size2UpdatePeriod(int size) => (1 << size) / 2f;

        protected internal static int Size2ActualSize(int size) => chunkSize * (1 << size);

        protected internal static readonly int chunkSize = 6;

        #endregion

        protected ChunkHolder holder;

        protected internal Vector3Int Position { get; protected set; }
        protected internal int Size { get; protected set; }

        protected internal Chunk(Vector3Int position, int size, ChunkHolder holder)
        {
            this.holder = holder;
            Position = position;
            Size = size;
        }

        protected internal abstract void DrawGizmos();

        protected internal static Chunk Create(Vector3Int chunkPosition, int size, ChunkHolder holder, Vector3 triggerPosition)
        {
            float distanceToTrigger = (triggerPosition - chunkPosition).magnitude;

            if (size < 0)
            {
                Debug.LogError("Size should not go below zero.");
            }

            if (distanceToTrigger < Size2DistanceRange(size).min && size > 0)
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