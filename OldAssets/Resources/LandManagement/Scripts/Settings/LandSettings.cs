using Biosearcher.Common;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.LandManagement.Settings
{
    [CreateAssetMenu(fileName = "Land Settings", menuName = "Land Settings", order = 52)]
    public sealed class LandSettings : ScriptableObject
    {
        #region Properties

        [Header("Chunk")]
        [NeedsRefactor(Needs.Rename)]
        [SerializeField] private int _minHierarchySize = 0;
        [SerializeField] private int _maxHierarchySize = 5;
        [SerializeField] private int _generatingFrequency = 8;
        [Header("Marching")]
        [Tooltip("If changing this, change numthreads in MarchingCubesFlat.compute and MarchingCubesPlanet.compute")]
        [SerializeField] private int _cubesPerChunk1D = 8;
        [SerializeField] [Range(0, 1)] private float _surfaceValue = 0.76f;
        [SerializeField] private MarchingGeometryType _geometryType = MarchingGeometryType.Planet;
        [SerializeField] private CubeMarchingType _cubeMarchingType = CubeMarchingType.CPU;
        [SerializeField] private AsyncType _asyncType = AsyncType.Jobs;
        [SerializeField] private ComputeShader _planetShader;
        [SerializeField] private ComputeShader _flatShader;
        [Header("Generation")]
        [SerializeField] private float _preGenerationDuration = 20;
        [SerializeField] private float _seed = 6;
        [SerializeField] private WorldType _worldType = WorldType.SingleChunk;
        [SerializeField] private GameObject _chunkPrefab;
        [SerializeField] private GameObject _chunkGrassPrefab;

        public int MinHierarchySize => _minHierarchySize;
        public int MaxHierarchySize => _maxHierarchySize;
        public int GeneratingFrequency => _generatingFrequency;

        public int CubesPerChunk1D => _cubesPerChunk1D;
        public int PointsPerChunk1D => _cubesPerChunk1D + 1;
        public float SurfaceValue => _surfaceValue;
        public MarchingGeometryType GeometryType => _geometryType;
        public CubeMarchingType CubeMarchingType => _cubeMarchingType;
        public AsyncType AsyncType => _asyncType;

        public float PreGenerationDuration => _preGenerationDuration;
        public float Seed => _seed;
        public WorldType WorldType => _worldType;
        public GameObject ChunkPrefab => _chunkPrefab;
        public GameObject ChunkGrassPrefab => _chunkGrassPrefab;
        public ComputeShader Shader
        {
            get
            {
                switch (_geometryType)
                {
                    case MarchingGeometryType.Flat:
                        return _flatShader;
                    case MarchingGeometryType.Planet:
                        return _planetShader;
                    default:
                        return _flatShader;
                }
            }
        }

        #endregion

        private void OnValidate() => _cubesPerChunk1D.MakeEven();
    }
}
