using Biosearcher.LandManagement.Chunks;
using Biosearcher.LandManagement.CubeMarching;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.LandManagement
{
    public class Land : MonoBehaviour
    {
        [SerializeField] protected Transform trigger;
        [SerializeField] protected LandSettings landSettings;

        protected ChunkTracker chunkTracker;
        protected GeometryManager geometryManager;
        protected CubeMarcher cubeMarcher;
        protected IChunkHolder chunksHolder;
        protected readonly Vector3Int PlanetPosition = Vector3Int.zero;

        protected void Awake()
        {
            Chunk.Initialize(landSettings);
            chunkTracker = new ChunkTracker(this, landSettings, trigger, landSettings.PreGenerationDuration);
            cubeMarcher = new CubeMarcher(landSettings);
            geometryManager = new GeometryManager(this, landSettings, cubeMarcher);
        }

        protected void OnDestroy()
        {
            geometryManager.Dispose();
            cubeMarcher.Dispose();
            chunkTracker.Dispose();
        }

        protected void Start() => InitializeChunksHolder(landSettings.WorldType);

        protected void OnDrawGizmos() => chunksHolder?.DrawGizmos();

        protected void InitializeChunksHolder(WorldType landType)
        {
            switch (landType)
            {
                case WorldType.Endless:
                    chunksHolder = new ChunksEndlessHolder(PlanetPosition, chunkTracker, geometryManager);
                    break;
                case WorldType.SingleChunk:
                    chunksHolder = new ChunksSingleHolder(PlanetPosition, chunkTracker, geometryManager);
                    break;
                default:
                    chunksHolder = new ChunksEndlessHolder(PlanetPosition, chunkTracker, geometryManager);
                    break;
            }
            chunksHolder.StartInitializing();
        }
    }
}