using UnityEngine;

namespace Biosearcher.LandManagement
{
    [CreateAssetMenu(fileName = "Land Settings", menuName = "Land Settings", order = 52)]
    public class LandSettings : ScriptableObject
    {
        [Header("Chunk")]
        [SerializeField] protected int minHierarchySize = 0;
        [SerializeField] protected int maxHierarchySize = 5;
        [SerializeField] protected int generatingFrequency = 8;
        [Header("Marching")]
        // If changing this, change numthreads in MarchingCubesFlat.compute and MarchingCubesPlanet.compute
        [Tooltip("If changing this, change numthreads in MarchingCubesFlat.compute and MarchingCubesPlanet.compute")]
        [SerializeField] protected int cubesPerChunk = 7;
        [SerializeField] [Range(0, 1)] protected float surfaceValue = 0.76f;
        [SerializeField] protected MarchingGeometryType geometryType = MarchingGeometryType.Flat;
        [SerializeField] protected ComputeShader planetShader;
        [SerializeField] protected ComputeShader flatShader;
        [Header("Generation")]
        [SerializeField] protected float preGenerationDuration = 20;
        [SerializeField] protected float seed = 6;
        [SerializeField] protected WorldType worldType = WorldType.Endless;
        [SerializeField] protected GameObject chunkPrefab;

        public int MinHierarchySize => minHierarchySize;
        public int MaxHierarchySize => maxHierarchySize;
        public int GeneratingFrequency => generatingFrequency;
        public int CubesPerChunk => cubesPerChunk;
        public int PointsPerChunk => cubesPerChunk + 1;
        public float SurfaceValue => surfaceValue;
        public float PreGenerationDuration => preGenerationDuration;
        public float Seed => seed;
        public WorldType WorldType => worldType;
        public GameObject ChunkPrefab => chunkPrefab;
        public ComputeShader Shader
        {
            get
            {
                switch (geometryType)
                {
                    case MarchingGeometryType.Flat:
                        return flatShader;
                    case MarchingGeometryType.Planet:
                        return planetShader;
                    default:
                        return flatShader;
                }
            }
        }
    }

    public enum MarchingGeometryType { Flat, Planet }
    public enum WorldType { SingleChunk, Endless }
}