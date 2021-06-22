using Biosearcher.LandManagement.Chunks;
using Biosearcher.LandManagement.Settings;
using UnityEngine;

namespace Biosearcher.LandManagement
{
    public class Land : MonoBehaviour
    {
        [SerializeField] protected Transform _trigger;
        [SerializeField] protected LandSettings _landSettings;

        protected ChunkTracker _chunkTracker;
        protected GeometryManager _geometryManager;
        protected IChunkHolder _chunksHolder;
        // TODO: Make manager & settings.
        protected readonly Vector3Int PlanetPosition = Vector3Int.zero;

        protected void Awake()
        {
            Chunk.Initialize(_landSettings);
            _chunkTracker = new ChunkTracker(_landSettings, _trigger);
            _geometryManager = new GeometryManager(this, _landSettings);
        }

        protected void OnDestroy()
        {
            _geometryManager.Dispose();
            _chunkTracker.Dispose();
        }

        protected void Start() => InitializeChunksHolder(_landSettings.WorldType);

        protected void OnDrawGizmos() => _chunksHolder?.DrawGizmos();

        protected void InitializeChunksHolder(WorldType landType)
        {
            switch (landType)
            {
                case WorldType.Endless:
                    _chunksHolder = new ChunksEndlessHolder(PlanetPosition, _chunkTracker, _geometryManager);
                    break;
                case WorldType.SingleChunk:
                    _chunksHolder = new ChunksSingleHolder(PlanetPosition, _chunkTracker, _geometryManager);
                    break;
                default:
                    _chunksHolder = new ChunksEndlessHolder(PlanetPosition, _chunkTracker, _geometryManager);
                    break;
            }
            _chunksHolder.StartInitializing();
        }
    }
}