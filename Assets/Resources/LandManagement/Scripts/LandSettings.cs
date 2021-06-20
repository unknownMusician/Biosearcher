using UnityEngine;

namespace Biosearcher.LandManagement
{
    [CreateAssetMenu(fileName = "Land Settings", menuName = "Land Settings", order = 52)]
    public class LandSettings : ScriptableObject
    {
        #region Properties

        [Header("Chunk")]
        [SerializeField] protected int _minHierarchySize = 0;
        [SerializeField] protected int _maxHierarchySize = 5;
        [SerializeField] protected int _generatingFrequency = 8;
        [Header("Marching")]
        [Tooltip("If changing this, change numthreads in MarchingCubesFlat.compute and MarchingCubesPlanet.compute")]
        [SerializeField] protected int _cubesPerChunk = 7;
        [SerializeField] [Range(0, 1)] protected float _surfaceValue = 0.76f;
        [SerializeField] protected MarchingGeometryType _geometryType = MarchingGeometryType.Planet;
        [SerializeField] protected ComputeShader _planetShader;
        [SerializeField] protected ComputeShader _flatShader;
        [Header("Generation")]
        [SerializeField] protected float _preGenerationDuration = 20;
        [SerializeField] protected float _seed = 6;
        [SerializeField] protected WorldType _worldType = WorldType.SingleChunk;
        [SerializeField] protected GameObject _chunkPrefab;

        public int MinHierarchySize => _minHierarchySize;
        public int MaxHierarchySize => _maxHierarchySize;
        public int GeneratingFrequency => _generatingFrequency;

        public int CubesPerChunk => _cubesPerChunk;
        public int PointsPerChunk => _cubesPerChunk + 1;
        public float SurfaceValue => _surfaceValue;

        public float PreGenerationDuration => _preGenerationDuration;
        public float Seed => _seed;
        public WorldType WorldType => _worldType;
        public GameObject ChunkPrefab => _chunkPrefab;
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
    }
}
