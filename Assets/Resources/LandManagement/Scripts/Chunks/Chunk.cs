using UnityEngine;
using UnityEngine.Events;

namespace Biosearcher.LandManagement.Chunks
{
    public abstract class Chunk
    {
        protected internal UnityAction Initialized;

        protected internal bool IsInitialized { get; protected set; } = false;
        protected internal Vector3Int Position { get; protected set; }
        protected internal int HierarchySize { get; protected set; }
        protected internal IChunkHolder Parent { get; protected set; }
        protected internal ChunkTracker ChunkTracker { get; protected set; }
        protected internal GeometryManager GeometryManager { get; protected set; }

        protected internal Chunk(Vector3Int position, int hierarchySize, IChunkHolder parent, ChunkTracker chunkTracker, GeometryManager geometryManager)
        {
            Position = position;
            HierarchySize = hierarchySize;
            Parent = parent;
            ChunkTracker = chunkTracker;
            GeometryManager = geometryManager;
        }
        protected internal Chunk(Chunk fromChunk)
            : this(fromChunk.Position, fromChunk.HierarchySize, fromChunk.Parent, fromChunk.ChunkTracker, fromChunk.GeometryManager) { }

        public abstract void StartInitializing();

        protected internal abstract void Instantiate();

        public virtual void DrawGizmos() { }

        #region Static

        protected internal static LandSettings Settings { get; private set; }
        protected internal static void Initialize(LandSettings settings) => Settings = settings;

        protected internal static (int min, int max) HierarchySize2DistanceRange(int size)
        {
            return (size == 0 ? 0 : HierarchySize2WorldSize(size) + HierarchySize2WorldSize(2), HierarchySize2WorldSize(size + 1) + HierarchySize2WorldSize(2));
        }

        protected internal static int HierarchySize2WorldSize(int size) => Settings.CubesPerChunk << size;

        protected static bool IsChunkTooSmallByAxis(float chunkPosition, float triggerPosition, float maxDistance)
        {
            return Mathf.Abs(chunkPosition - triggerPosition) > maxDistance;
        }
        protected static bool IsChunkTooLargeByAxis(float chunkPosition, float triggerPosition, float minDistance)
        {
            return Mathf.Abs(chunkPosition - triggerPosition) < minDistance;
        }

        protected internal static bool IsChunkTooSmall(Vector3Int chunkPosition, int chunkHierarchySize, Vector3Int triggerPosition, float maxDistance)
        {
            bool isTooSmall = false;
            isTooSmall |= IsChunkTooSmallByAxis(chunkPosition.x, triggerPosition.x, maxDistance);
            isTooSmall |= IsChunkTooSmallByAxis(chunkPosition.y, triggerPosition.y, maxDistance);
            isTooSmall |= IsChunkTooSmallByAxis(chunkPosition.z, triggerPosition.z, maxDistance);
            isTooSmall &= chunkHierarchySize < Settings.MaxHierarchySize;
            return isTooSmall;
        }
        protected internal static bool IsChunkTooSmall(Vector3Int chunkPosition, int chunkHierarchySize, Vector3Int triggerPosition)
            => IsChunkTooSmall(chunkPosition, chunkHierarchySize, triggerPosition, Chunk.HierarchySize2DistanceRange(chunkHierarchySize).max);
        protected internal static bool IsChunkTooLarge(Vector3Int chunkPosition, int chunkHierarchySize, Vector3Int triggerPosition, float minDistance)
        {
            bool isTooLarge = true;
            isTooLarge &= IsChunkTooLargeByAxis(chunkPosition.x, triggerPosition.x, minDistance);
            isTooLarge &= IsChunkTooLargeByAxis(chunkPosition.y, triggerPosition.y, minDistance);
            isTooLarge &= IsChunkTooLargeByAxis(chunkPosition.z, triggerPosition.z, minDistance);
            isTooLarge &= chunkHierarchySize > Settings.MinHierarchySize;
            return isTooLarge;
        }
        protected internal static bool IsChunkTooLarge(Vector3Int chunkPosition, int chunkHierarchySize, Vector3Int triggerPosition)
            => IsChunkTooLarge(chunkPosition, chunkHierarchySize, triggerPosition, Chunk.HierarchySize2DistanceRange(chunkHierarchySize).min);

        protected internal static bool IsChunkWrongSize(Vector3Int chunkPosition, int chunkHierarchySize, Vector3Int triggerPosition, out WrongSizeType changeType)
        {
            (int minDistance, int maxDistance) = Chunk.HierarchySize2DistanceRange(chunkHierarchySize);
            bool isTooSmall = IsChunkTooSmall(chunkPosition, chunkHierarchySize, triggerPosition, maxDistance);
            bool isTooLarge = IsChunkTooLarge(chunkPosition, chunkHierarchySize, triggerPosition, minDistance);

            changeType = isTooLarge ? WrongSizeType.TooLarge : WrongSizeType.TooSmall;
            return isTooSmall || isTooLarge;
        }

        protected internal static Vector3Int RoundPosition(Vector3Int position, int chunkHierarchySize)
        {
            int chunkWorldSize = HierarchySize2WorldSize(chunkHierarchySize);
            return (position / chunkWorldSize) * chunkWorldSize;
        }

        protected internal static Chunk CreateByDistanceToTrigger(
            Vector3Int position, int hierarchySize, ChunkWithChunks parent, ChunkTracker chunkTracker, GeometryManager geometryManager, Vector3Int triggerPosition)
        {
            if (IsChunkTooLarge(position, hierarchySize, triggerPosition))
            {
                return new ChunkWithChunks(position, hierarchySize, parent, chunkTracker, geometryManager);
            }
            return new ChunkWithGeometry(position, hierarchySize, parent, chunkTracker, geometryManager);
        }

        #endregion

        protected internal enum WrongSizeType { TooSmall, TooLarge }
    }
}