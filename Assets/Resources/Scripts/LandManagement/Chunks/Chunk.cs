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
            return (size == 0 ? 0 : HierarchySize2WorldSize(size), HierarchySize2WorldSize(size + 1));
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
            bool isNeedToUnite = false;
            isNeedToUnite |= IsChunkTooSmallByAxis(chunkPosition.x, triggerPosition.x, maxDistance);
            isNeedToUnite |= IsChunkTooSmallByAxis(chunkPosition.y, triggerPosition.y, maxDistance);
            isNeedToUnite |= IsChunkTooSmallByAxis(chunkPosition.z, triggerPosition.z, maxDistance);
            isNeedToUnite &= chunkHierarchySize < Settings.MaxHierarchySize;
            return isNeedToUnite;
        }
        protected internal static bool IsChunkTooSmall(Vector3Int chunkPosition, int chunkHierarchySize, Vector3Int triggerPosition)
            => IsChunkTooSmall(chunkPosition, chunkHierarchySize, triggerPosition, Chunk.HierarchySize2DistanceRange(chunkHierarchySize).max);
        protected internal static bool IsChunkTooLarge(Vector3Int chunkPosition, int chunkHierarchySize, Vector3Int triggerPosition, float minDistance)
        {
            bool isNeedToDivide = true;
            isNeedToDivide &= IsChunkTooLargeByAxis(chunkPosition.x, triggerPosition.x, minDistance);
            isNeedToDivide &= IsChunkTooLargeByAxis(chunkPosition.y, triggerPosition.y, minDistance);
            isNeedToDivide &= IsChunkTooLargeByAxis(chunkPosition.z, triggerPosition.z, minDistance);
            isNeedToDivide &= chunkHierarchySize > Settings.MinHierarchySize;
            return isNeedToDivide;
        }
        protected internal static bool IsChunkTooLarge(Vector3Int chunkPosition, int chunkHierarchySize, Vector3Int triggerPosition)
            => IsChunkTooLarge(chunkPosition, chunkHierarchySize, triggerPosition, Chunk.HierarchySize2DistanceRange(chunkHierarchySize).min);

        protected internal static bool IsChunkWrongSize(Vector3Int chunkPosition, int chunkHierarchySize, Vector3Int triggerPosition, out ChangeType changeType)
        {
            (int min, int max) distanceRange = Chunk.HierarchySize2DistanceRange(chunkHierarchySize);
            bool isNeedToUnite = IsChunkTooSmall(chunkPosition, chunkHierarchySize, triggerPosition, distanceRange.max);
            bool isNeedToDivide = IsChunkTooLarge(chunkPosition, chunkHierarchySize, triggerPosition, distanceRange.min);

            changeType = isNeedToDivide ? ChangeType.Divide : ChangeType.Unite;
            return isNeedToUnite || isNeedToDivide;
        }

        protected internal static Vector3Int RoundPosition(Vector3Int position, int chunkHierarchySize)
        {
            int chunkWorldSize = HierarchySize2WorldSize(chunkHierarchySize);
            return position / chunkWorldSize * chunkWorldSize;
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

        protected internal enum ChangeType { Unite, Divide }
    }
}