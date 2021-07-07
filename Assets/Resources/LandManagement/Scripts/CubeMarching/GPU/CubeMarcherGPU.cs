using Biosearcher.LandManagement.Settings;
using UnityEngine;
#if BIOSEARCHER_PROFILING
using UnityEngine.Profiling;
#endif

namespace Biosearcher.LandManagement.CubeMarching.GPU
{
    public class CubeMarcherGPU : CubeMarcher
    {
        protected readonly int _pointsPerChunk1DId = Shader.PropertyToID("_PointsPerChunk1D");
        protected readonly int _cubesPerChunk1DId = Shader.PropertyToID("_CubesPerChunk1D");
        protected readonly int _surfaceValueId = Shader.PropertyToID("_SurfaceValue");
        protected readonly int _seedId = Shader.PropertyToID("_Seed");
        protected readonly int _chunkPositionId = Shader.PropertyToID("_ChunkPosition");
        protected readonly int _cubeSizeId = Shader.PropertyToID("_CubeSize");
        protected readonly int _edgeIndex2PointIndexesTextureId = Shader.PropertyToID("_EdgeIndex2PointIndexesT");
        protected readonly int _pointsHash2EdgesHashTextureId = Shader.PropertyToID("_PointsHash2EdgesHashT");
        protected readonly int _pointsHash2EdgesIndexesTextureId = Shader.PropertyToID("_PointsHash2EdgesIndexesT");
        protected readonly int _pointsBufferId = Shader.PropertyToID("_Points");
        protected readonly int _meshV3T1BufferId = Shader.PropertyToID("_MeshV3T1");

        protected Texture2D _edgeIndex2PointIndexesTexture;
        protected Texture2D _pointsHash2EdgesHashTexture;
        protected Texture2D _pointsHash2EdgesIndexesTexture;
        protected ComputeBuffer _meshV3T1Buffer;
        protected ComputeBuffer _pointsBuffer;
        protected Vector4[] _meshV3T1Values;

        protected ComputeShader _shader;
        protected float _seed;
        protected int _cubesPerChunk1D;
        protected int _pointsPerChunk1D;
        protected float _surfaceValue;
        protected int _meshBufferSize;
        protected int _generateMeshKernel;
        protected int _generatePointsKernel;
        protected int _threadGroups;
        protected int _pointsBufferSize;

        // if changing this, change numthreads in .compute files
        protected const int CubeNumthreads = 7;

        public CubeMarcherGPU(LandSettings settings)
        {
            InitializeMarcher(settings);
            InitializeResources();
            InitializeShader();
        }

        protected void InitializeMarcher(LandSettings settings)
        {
            _cubesPerChunk1D = settings.CubesPerChunk1D;
            _pointsPerChunk1D = settings.PointsPerChunk1D;
            _surfaceValue = settings.SurfaceValue;
            _seed = settings.Seed;
            _shader = settings.Shader;
            _threadGroups = Mathf.CeilToInt(_cubesPerChunk1D / (float)CubeNumthreads);
            _pointsBufferSize = (_cubesPerChunk1D + 1) * (_cubesPerChunk1D + 1) * (_cubesPerChunk1D + 1);
            _meshBufferSize = 15 * _cubesPerChunk1D * _cubesPerChunk1D * _cubesPerChunk1D;

            _generateMeshKernel = _shader.FindKernel("GenerateMesh");
            _generatePointsKernel = _shader.FindKernel("GeneratePoints");
        }
        protected void InitializeResources()
        {
            _edgeIndex2PointIndexesTexture = new Texture2D(2, 12, TextureFormat.RFloat, false);
            float[] edgeIndex2PointIndexes =
            {
                0, 1,
                1, 2,
                2, 3,
                3, 0,
                4, 5,
                5, 6,
                6, 7,
                7, 4,
                0, 4,
                1, 5,
                2, 6,
                3, 7
            };
            _edgeIndex2PointIndexesTexture.SetPixelData(edgeIndex2PointIndexes, 0);
            _edgeIndex2PointIndexesTexture.Apply();

            _pointsHash2EdgesHashTexture = new Texture2D(256, 1, TextureFormat.RFloat, false);
            float[] pointsHash2EdgesHash =
            {
                0x000, 0x109, 0x203, 0x30a, 0x406, 0x50f, 0x605, 0x70c, 0x80c, 0x905, 0xa0f, 0xb06, 0xc0a, 0xd03, 0xe09, 0xf00,
                0x190, 0x099, 0x393, 0x29a, 0x596, 0x49f, 0x795, 0x69c, 0x99c, 0x895, 0xb9f, 0xa96, 0xd9a, 0xc93, 0xf99, 0xe90,
                0x230, 0x339, 0x033, 0x13a, 0x636, 0x73f, 0x435, 0x53c, 0xa3c, 0xb35, 0x83f, 0x936, 0xe3a, 0xf33, 0xc39, 0xd30,
                0x3a0, 0x2a9, 0x1a3, 0x0aa, 0x7a6, 0x6af, 0x5a5, 0x4ac, 0xbac, 0xaa5, 0x9af, 0x8a6, 0xfaa, 0xea3, 0xda9, 0xca0,
                0x460, 0x569, 0x663, 0x76a, 0x066, 0x16f, 0x265, 0x36c, 0xc6c, 0xd65, 0xe6f, 0xf66, 0x86a, 0x963, 0xa69, 0xb60,
                0x5f0, 0x4f9, 0x7f3, 0x6fa, 0x1f6, 0x0ff, 0x3f5, 0x2fc, 0xdfc, 0xcf5, 0xfff, 0xef6, 0x9fa, 0x8f3, 0xbf9, 0xaf0,
                0x650, 0x759, 0x453, 0x55a, 0x256, 0x35f, 0x055, 0x15c, 0xe5c, 0xf55, 0xc5f, 0xd56, 0xa5a, 0xb53, 0x859, 0x950,
                0x7c0, 0x6c9, 0x5c3, 0x4ca, 0x3c6, 0x2cf, 0x1c5, 0x0cc, 0xfcc, 0xec5, 0xdcf, 0xcc6, 0xbca, 0xac3, 0x9c9, 0x8c0,
                0x8c0, 0x9c9, 0xac3, 0xbca, 0xcc6, 0xdcf, 0xec5, 0xfcc, 0x0cc, 0x1c5, 0x2cf, 0x3c6, 0x4ca, 0x5c3, 0x6c9, 0x7c0,
                0x950, 0x859, 0xb53, 0xa5a, 0xd56, 0xc5f, 0xf55, 0xe5c, 0x15c, 0x055, 0x35f, 0x256, 0x55a, 0x453, 0x759, 0x650,
                0xaf0, 0xbf9, 0x8f3, 0x9fa, 0xef6, 0xfff, 0xcf5, 0xdfc, 0x2fc, 0x3f5, 0x0ff, 0x1f6, 0x6fa, 0x7f3, 0x4f9, 0x5f0,
                0xb60, 0xa69, 0x963, 0x86a, 0xf66, 0xe6f, 0xd65, 0xc6c, 0x36c, 0x265, 0x16f, 0x066, 0x76a, 0x663, 0x569, 0x460,
                0xca0, 0xda9, 0xea3, 0xfaa, 0x8a6, 0x9af, 0xaa5, 0xbac, 0x4ac, 0x5a5, 0x6af, 0x7a6, 0x0aa, 0x1a3, 0x2a9, 0x3a0,
                0xd30, 0xc39, 0xf33, 0xe3a, 0x936, 0x83f, 0xb35, 0xa3c, 0x53c, 0x435, 0x73f, 0x636, 0x13a, 0x033, 0x339, 0x230,
                0xe90, 0xf99, 0xc93, 0xd9a, 0xa96, 0xb9f, 0x895, 0x99c, 0x69c, 0x795, 0x49f, 0x596, 0x29a, 0x393, 0x099, 0x190,
                0xf00, 0xe09, 0xd03, 0xc0a, 0xb06, 0xa0f, 0x905, 0x80c, 0x70c, 0x605, 0x50f, 0x406, 0x30a, 0x203, 0x109, 0x000
            };
            _pointsHash2EdgesHashTexture.SetPixelData(pointsHash2EdgesHash, 0);
            _pointsHash2EdgesHashTexture.Apply();

            _pointsHash2EdgesIndexesTexture = new Texture2D(16, 256, TextureFormat.RFloat, false);
            float[] pointsHash2EdgesIndexes =
            {
                20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 8, 3, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 1, 9, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                1, 8, 3, 9, 8, 1, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                1, 2, 10, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 8, 3, 1, 2, 10, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                9, 2, 10, 0, 2, 9, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                2, 8, 3, 2, 10, 8, 10, 9, 8, 20, 20, 20, 20, 20, 20, 20,
                3, 11, 2, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 11, 2, 8, 11, 0, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                1, 9, 0, 2, 3, 11, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                1, 11, 2, 1, 9, 11, 9, 8, 11, 20, 20, 20, 20, 20, 20, 20,
                3, 10, 1, 11, 10, 3, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 10, 1, 0, 8, 10, 8, 11, 10, 20, 20, 20, 20, 20, 20, 20,
                3, 9, 0, 3, 11, 9, 11, 10, 9, 20, 20, 20, 20, 20, 20, 20,
                9, 8, 10, 10, 8, 11, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                4, 7, 8, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                4, 3, 0, 7, 3, 4, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 1, 9, 8, 4, 7, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                4, 1, 9, 4, 7, 1, 7, 3, 1, 20, 20, 20, 20, 20, 20, 20,
                1, 2, 10, 8, 4, 7, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                3, 4, 7, 3, 0, 4, 1, 2, 10, 20, 20, 20, 20, 20, 20, 20,
                9, 2, 10, 9, 0, 2, 8, 4, 7, 20, 20, 20, 20, 20, 20, 20,
                2, 10, 9, 2, 9, 7, 2, 7, 3, 7, 9, 4, 20, 20, 20, 20,
                8, 4, 7, 3, 11, 2, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                11, 4, 7, 11, 2, 4, 2, 0, 4, 20, 20, 20, 20, 20, 20, 20,
                9, 0, 1, 8, 4, 7, 2, 3, 11, 20, 20, 20, 20, 20, 20, 20,
                4, 7, 11, 9, 4, 11, 9, 11, 2, 9, 2, 1, 20, 20, 20, 20,
                3, 10, 1, 3, 11, 10, 7, 8, 4, 20, 20, 20, 20, 20, 20, 20,
                1, 11, 10, 1, 4, 11, 1, 0, 4, 7, 11, 4, 20, 20, 20, 20,
                4, 7, 8, 9, 0, 11, 9, 11, 10, 11, 0, 3, 20, 20, 20, 20,
                4, 7, 11, 4, 11, 9, 9, 11, 10, 20, 20, 20, 20, 20, 20, 20,
                9, 5, 4, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                9, 5, 4, 0, 8, 3, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 5, 4, 1, 5, 0, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                8, 5, 4, 8, 3, 5, 3, 1, 5, 20, 20, 20, 20, 20, 20, 20,
                1, 2, 10, 9, 5, 4, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                3, 0, 8, 1, 2, 10, 4, 9, 5, 20, 20, 20, 20, 20, 20, 20,
                5, 2, 10, 5, 4, 2, 4, 0, 2, 20, 20, 20, 20, 20, 20, 20,
                2, 10, 5, 3, 2, 5, 3, 5, 4, 3, 4, 8, 20, 20, 20, 20,
                9, 5, 4, 2, 3, 11, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 11, 2, 0, 8, 11, 4, 9, 5, 20, 20, 20, 20, 20, 20, 20,
                0, 5, 4, 0, 1, 5, 2, 3, 11, 20, 20, 20, 20, 20, 20, 20,
                2, 1, 5, 2, 5, 8, 2, 8, 11, 4, 8, 5, 20, 20, 20, 20,
                10, 3, 11, 10, 1, 3, 9, 5, 4, 20, 20, 20, 20, 20, 20, 20,
                4, 9, 5, 0, 8, 1, 8, 10, 1, 8, 11, 10, 20, 20, 20, 20,
                5, 4, 0, 5, 0, 11, 5, 11, 10, 11, 0, 3, 20, 20, 20, 20,
                5, 4, 8, 5, 8, 10, 10, 8, 11, 20, 20, 20, 20, 20, 20, 20,
                9, 7, 8, 5, 7, 9, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                9, 3, 0, 9, 5, 3, 5, 7, 3, 20, 20, 20, 20, 20, 20, 20,
                0, 7, 8, 0, 1, 7, 1, 5, 7, 20, 20, 20, 20, 20, 20, 20,
                1, 5, 3, 3, 5, 7, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                9, 7, 8, 9, 5, 7, 10, 1, 2, 20, 20, 20, 20, 20, 20, 20,
                10, 1, 2, 9, 5, 0, 5, 3, 0, 5, 7, 3, 20, 20, 20, 20,
                8, 0, 2, 8, 2, 5, 8, 5, 7, 10, 5, 2, 20, 20, 20, 20,
                2, 10, 5, 2, 5, 3, 3, 5, 7, 20, 20, 20, 20, 20, 20, 20,
                7, 9, 5, 7, 8, 9, 3, 11, 2, 20, 20, 20, 20, 20, 20, 20,
                9, 5, 7, 9, 7, 2, 9, 2, 0, 2, 7, 11, 20, 20, 20, 20,
                2, 3, 11, 0, 1, 8, 1, 7, 8, 1, 5, 7, 20, 20, 20, 20,
                11, 2, 1, 11, 1, 7, 7, 1, 5, 20, 20, 20, 20, 20, 20, 20,
                9, 5, 8, 8, 5, 7, 10, 1, 3, 10, 3, 11, 20, 20, 20, 20,
                5, 7, 0, 5, 0, 9, 7, 11, 0, 1, 0, 10, 11, 10, 0, 20,
                11, 10, 0, 11, 0, 3, 10, 5, 0, 8, 0, 7, 5, 7, 0, 20,
                11, 10, 5, 7, 11, 5, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                10, 6, 5, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 8, 3, 5, 10, 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                9, 0, 1, 5, 10, 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                1, 8, 3, 1, 9, 8, 5, 10, 6, 20, 20, 20, 20, 20, 20, 20,
                1, 6, 5, 2, 6, 1, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                1, 6, 5, 1, 2, 6, 3, 0, 8, 20, 20, 20, 20, 20, 20, 20,
                9, 6, 5, 9, 0, 6, 0, 2, 6, 20, 20, 20, 20, 20, 20, 20,
                5, 9, 8, 5, 8, 2, 5, 2, 6, 3, 2, 8, 20, 20, 20, 20,
                2, 3, 11, 10, 6, 5, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                11, 0, 8, 11, 2, 0, 10, 6, 5, 20, 20, 20, 20, 20, 20, 20,
                0, 1, 9, 2, 3, 11, 5, 10, 6, 20, 20, 20, 20, 20, 20, 20,
                5, 10, 6, 1, 9, 2, 9, 11, 2, 9, 8, 11, 20, 20, 20, 20,
                6, 3, 11, 6, 5, 3, 5, 1, 3, 20, 20, 20, 20, 20, 20, 20,
                0, 8, 11, 0, 11, 5, 0, 5, 1, 5, 11, 6, 20, 20, 20, 20,
                3, 11, 6, 0, 3, 6, 0, 6, 5, 0, 5, 9, 20, 20, 20, 20,
                6, 5, 9, 6, 9, 11, 11, 9, 8, 20, 20, 20, 20, 20, 20, 20,
                5, 10, 6, 4, 7, 8, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                4, 3, 0, 4, 7, 3, 6, 5, 10, 20, 20, 20, 20, 20, 20, 20,
                1, 9, 0, 5, 10, 6, 8, 4, 7, 20, 20, 20, 20, 20, 20, 20,
                10, 6, 5, 1, 9, 7, 1, 7, 3, 7, 9, 4, 20, 20, 20, 20,
                6, 1, 2, 6, 5, 1, 4, 7, 8, 20, 20, 20, 20, 20, 20, 20,
                1, 2, 5, 5, 2, 6, 3, 0, 4, 3, 4, 7, 20, 20, 20, 20,
                8, 4, 7, 9, 0, 5, 0, 6, 5, 0, 2, 6, 20, 20, 20, 20,
                7, 3, 9, 7, 9, 4, 3, 2, 9, 5, 9, 6, 2, 6, 9, 20,
                3, 11, 2, 7, 8, 4, 10, 6, 5, 20, 20, 20, 20, 20, 20, 20,
                5, 10, 6, 4, 7, 2, 4, 2, 0, 2, 7, 11, 20, 20, 20, 20,
                0, 1, 9, 4, 7, 8, 2, 3, 11, 5, 10, 6, 20, 20, 20, 20,
                9, 2, 1, 9, 11, 2, 9, 4, 11, 7, 11, 4, 5, 10, 6, 20,
                8, 4, 7, 3, 11, 5, 3, 5, 1, 5, 11, 6, 20, 20, 20, 20,
                5, 1, 11, 5, 11, 6, 1, 0, 11, 7, 11, 4, 0, 4, 11, 20,
                0, 5, 9, 0, 6, 5, 0, 3, 6, 11, 6, 3, 8, 4, 7, 20,
                6, 5, 9, 6, 9, 11, 4, 7, 9, 7, 11, 9, 20, 20, 20, 20,
                10, 4, 9, 6, 4, 10, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                4, 10, 6, 4, 9, 10, 0, 8, 3, 20, 20, 20, 20, 20, 20, 20,
                10, 0, 1, 10, 6, 0, 6, 4, 0, 20, 20, 20, 20, 20, 20, 20,
                8, 3, 1, 8, 1, 6, 8, 6, 4, 6, 1, 10, 20, 20, 20, 20,
                1, 4, 9, 1, 2, 4, 2, 6, 4, 20, 20, 20, 20, 20, 20, 20,
                3, 0, 8, 1, 2, 9, 2, 4, 9, 2, 6, 4, 20, 20, 20, 20,
                0, 2, 4, 4, 2, 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                8, 3, 2, 8, 2, 4, 4, 2, 6, 20, 20, 20, 20, 20, 20, 20,
                10, 4, 9, 10, 6, 4, 11, 2, 3, 20, 20, 20, 20, 20, 20, 20,
                0, 8, 2, 2, 8, 11, 4, 9, 10, 4, 10, 6, 20, 20, 20, 20,
                3, 11, 2, 0, 1, 6, 0, 6, 4, 6, 1, 10, 20, 20, 20, 20,
                6, 4, 1, 6, 1, 10, 4, 8, 1, 2, 1, 11, 8, 11, 1, 20,
                9, 6, 4, 9, 3, 6, 9, 1, 3, 11, 6, 3, 20, 20, 20, 20,
                8, 11, 1, 8, 1, 0, 11, 6, 1, 9, 1, 4, 6, 4, 1, 20,
                3, 11, 6, 3, 6, 0, 0, 6, 4, 20, 20, 20, 20, 20, 20, 20,
                6, 4, 8, 11, 6, 8, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                7, 10, 6, 7, 8, 10, 8, 9, 10, 20, 20, 20, 20, 20, 20, 20,
                0, 7, 3, 0, 10, 7, 0, 9, 10, 6, 7, 10, 20, 20, 20, 20,
                10, 6, 7, 1, 10, 7, 1, 7, 8, 1, 8, 0, 20, 20, 20, 20,
                10, 6, 7, 10, 7, 1, 1, 7, 3, 20, 20, 20, 20, 20, 20, 20,
                1, 2, 6, 1, 6, 8, 1, 8, 9, 8, 6, 7, 20, 20, 20, 20,
                2, 6, 9, 2, 9, 1, 6, 7, 9, 0, 9, 3, 7, 3, 9, 20,
                7, 8, 0, 7, 0, 6, 6, 0, 2, 20, 20, 20, 20, 20, 20, 20,
                7, 3, 2, 6, 7, 2, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                2, 3, 11, 10, 6, 8, 10, 8, 9, 8, 6, 7, 20, 20, 20, 20,
                2, 0, 7, 2, 7, 11, 0, 9, 7, 6, 7, 10, 9, 10, 7, 20,
                1, 8, 0, 1, 7, 8, 1, 10, 7, 6, 7, 10, 2, 3, 11, 20,
                11, 2, 1, 11, 1, 7, 10, 6, 1, 6, 7, 1, 20, 20, 20, 20,
                8, 9, 6, 8, 6, 7, 9, 1, 6, 11, 6, 3, 1, 3, 6, 20,
                0, 9, 1, 11, 6, 7, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                7, 8, 0, 7, 0, 6, 3, 11, 0, 11, 6, 0, 20, 20, 20, 20,
                7, 11, 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                7, 6, 11, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                3, 0, 8, 11, 7, 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 1, 9, 11, 7, 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                8, 1, 9, 8, 3, 1, 11, 7, 6, 20, 20, 20, 20, 20, 20, 20,
                10, 1, 2, 6, 11, 7, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                1, 2, 10, 3, 0, 8, 6, 11, 7, 20, 20, 20, 20, 20, 20, 20,
                2, 9, 0, 2, 10, 9, 6, 11, 7, 20, 20, 20, 20, 20, 20, 20,
                6, 11, 7, 2, 10, 3, 10, 8, 3, 10, 9, 8, 20, 20, 20, 20,
                7, 2, 3, 6, 2, 7, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                7, 0, 8, 7, 6, 0, 6, 2, 0, 20, 20, 20, 20, 20, 20, 20,
                2, 7, 6, 2, 3, 7, 0, 1, 9, 20, 20, 20, 20, 20, 20, 20,
                1, 6, 2, 1, 8, 6, 1, 9, 8, 8, 7, 6, 20, 20, 20, 20,
                10, 7, 6, 10, 1, 7, 1, 3, 7, 20, 20, 20, 20, 20, 20, 20,
                10, 7, 6, 1, 7, 10, 1, 8, 7, 1, 0, 8, 20, 20, 20, 20,
                0, 3, 7, 0, 7, 10, 0, 10, 9, 6, 10, 7, 20, 20, 20, 20,
                7, 6, 10, 7, 10, 8, 8, 10, 9, 20, 20, 20, 20, 20, 20, 20,
                6, 8, 4, 11, 8, 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                3, 6, 11, 3, 0, 6, 0, 4, 6, 20, 20, 20, 20, 20, 20, 20,
                8, 6, 11, 8, 4, 6, 9, 0, 1, 20, 20, 20, 20, 20, 20, 20,
                9, 4, 6, 9, 6, 3, 9, 3, 1, 11, 3, 6, 20, 20, 20, 20,
                6, 8, 4, 6, 11, 8, 2, 10, 1, 20, 20, 20, 20, 20, 20, 20,
                1, 2, 10, 3, 0, 11, 0, 6, 11, 0, 4, 6, 20, 20, 20, 20,
                4, 11, 8, 4, 6, 11, 0, 2, 9, 2, 10, 9, 20, 20, 20, 20,
                10, 9, 3, 10, 3, 2, 9, 4, 3, 11, 3, 6, 4, 6, 3, 20,
                8, 2, 3, 8, 4, 2, 4, 6, 2, 20, 20, 20, 20, 20, 20, 20,
                0, 4, 2, 4, 6, 2, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                1, 9, 0, 2, 3, 4, 2, 4, 6, 4, 3, 8, 20, 20, 20, 20,
                1, 9, 4, 1, 4, 2, 2, 4, 6, 20, 20, 20, 20, 20, 20, 20,
                8, 1, 3, 8, 6, 1, 8, 4, 6, 6, 10, 1, 20, 20, 20, 20,
                10, 1, 0, 10, 0, 6, 6, 0, 4, 20, 20, 20, 20, 20, 20, 20,
                4, 6, 3, 4, 3, 8, 6, 10, 3, 0, 3, 9, 10, 9, 3, 20,
                10, 9, 4, 6, 10, 4, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                4, 9, 5, 7, 6, 11, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 8, 3, 4, 9, 5, 11, 7, 6, 20, 20, 20, 20, 20, 20, 20,
                5, 0, 1, 5, 4, 0, 7, 6, 11, 20, 20, 20, 20, 20, 20, 20,
                11, 7, 6, 8, 3, 4, 3, 5, 4, 3, 1, 5, 20, 20, 20, 20,
                9, 5, 4, 10, 1, 2, 7, 6, 11, 20, 20, 20, 20, 20, 20, 20,
                6, 11, 7, 1, 2, 10, 0, 8, 3, 4, 9, 5, 20, 20, 20, 20,
                7, 6, 11, 5, 4, 10, 4, 2, 10, 4, 0, 2, 20, 20, 20, 20,
                3, 4, 8, 3, 5, 4, 3, 2, 5, 10, 5, 2, 11, 7, 6, 20,
                7, 2, 3, 7, 6, 2, 5, 4, 9, 20, 20, 20, 20, 20, 20, 20,
                9, 5, 4, 0, 8, 6, 0, 6, 2, 6, 8, 7, 20, 20, 20, 20,
                3, 6, 2, 3, 7, 6, 1, 5, 0, 5, 4, 0, 20, 20, 20, 20,
                6, 2, 8, 6, 8, 7, 2, 1, 8, 4, 8, 5, 1, 5, 8, 20,
                9, 5, 4, 10, 1, 6, 1, 7, 6, 1, 3, 7, 20, 20, 20, 20,
                1, 6, 10, 1, 7, 6, 1, 0, 7, 8, 7, 0, 9, 5, 4, 20,
                4, 0, 10, 4, 10, 5, 0, 3, 10, 6, 10, 7, 3, 7, 10, 20,
                7, 6, 10, 7, 10, 8, 5, 4, 10, 4, 8, 10, 20, 20, 20, 20,
                6, 9, 5, 6, 11, 9, 11, 8, 9, 20, 20, 20, 20, 20, 20, 20,
                3, 6, 11, 0, 6, 3, 0, 5, 6, 0, 9, 5, 20, 20, 20, 20,
                0, 11, 8, 0, 5, 11, 0, 1, 5, 5, 6, 11, 20, 20, 20, 20,
                6, 11, 3, 6, 3, 5, 5, 3, 1, 20, 20, 20, 20, 20, 20, 20,
                1, 2, 10, 9, 5, 11, 9, 11, 8, 11, 5, 6, 20, 20, 20, 20,
                0, 11, 3, 0, 6, 11, 0, 9, 6, 5, 6, 9, 1, 2, 10, 20,
                11, 8, 5, 11, 5, 6, 8, 0, 5, 10, 5, 2, 0, 2, 5, 20,
                6, 11, 3, 6, 3, 5, 2, 10, 3, 10, 5, 3, 20, 20, 20, 20,
                5, 8, 9, 5, 2, 8, 5, 6, 2, 3, 8, 2, 20, 20, 20, 20,
                9, 5, 6, 9, 6, 0, 0, 6, 2, 20, 20, 20, 20, 20, 20, 20,
                1, 5, 8, 1, 8, 0, 5, 6, 8, 3, 8, 2, 6, 2, 8, 20,
                1, 5, 6, 2, 1, 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                1, 3, 6, 1, 6, 10, 3, 8, 6, 5, 6, 9, 8, 9, 6, 20,
                10, 1, 0, 10, 0, 6, 9, 5, 0, 5, 6, 0, 20, 20, 20, 20,
                0, 3, 8, 5, 6, 10, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                10, 5, 6, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                11, 5, 10, 7, 5, 11, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                11, 5, 10, 11, 7, 5, 8, 3, 0, 20, 20, 20, 20, 20, 20, 20,
                5, 11, 7, 5, 10, 11, 1, 9, 0, 20, 20, 20, 20, 20, 20, 20,
                10, 7, 5, 10, 11, 7, 9, 8, 1, 8, 3, 1, 20, 20, 20, 20,
                11, 1, 2, 11, 7, 1, 7, 5, 1, 20, 20, 20, 20, 20, 20, 20,
                0, 8, 3, 1, 2, 7, 1, 7, 5, 7, 2, 11, 20, 20, 20, 20,
                9, 7, 5, 9, 2, 7, 9, 0, 2, 2, 11, 7, 20, 20, 20, 20,
                7, 5, 2, 7, 2, 11, 5, 9, 2, 3, 2, 8, 9, 8, 2, 20,
                2, 5, 10, 2, 3, 5, 3, 7, 5, 20, 20, 20, 20, 20, 20, 20,
                8, 2, 0, 8, 5, 2, 8, 7, 5, 10, 2, 5, 20, 20, 20, 20,
                9, 0, 1, 5, 10, 3, 5, 3, 7, 3, 10, 2, 20, 20, 20, 20,
                9, 8, 2, 9, 2, 1, 8, 7, 2, 10, 2, 5, 7, 5, 2, 20,
                1, 3, 5, 3, 7, 5, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 8, 7, 0, 7, 1, 1, 7, 5, 20, 20, 20, 20, 20, 20, 20,
                9, 0, 3, 9, 3, 5, 5, 3, 7, 20, 20, 20, 20, 20, 20, 20,
                9, 8, 7, 5, 9, 7, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                5, 8, 4, 5, 10, 8, 10, 11, 8, 20, 20, 20, 20, 20, 20, 20,
                5, 0, 4, 5, 11, 0, 5, 10, 11, 11, 3, 0, 20, 20, 20, 20,
                0, 1, 9, 8, 4, 10, 8, 10, 11, 10, 4, 5, 20, 20, 20, 20,
                10, 11, 4, 10, 4, 5, 11, 3, 4, 9, 4, 1, 3, 1, 4, 20,
                2, 5, 1, 2, 8, 5, 2, 11, 8, 4, 5, 8, 20, 20, 20, 20,
                0, 4, 11, 0, 11, 3, 4, 5, 11, 2, 11, 1, 5, 1, 11, 20,
                0, 2, 5, 0, 5, 9, 2, 11, 5, 4, 5, 8, 11, 8, 5, 20,
                9, 4, 5, 2, 11, 3, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                2, 5, 10, 3, 5, 2, 3, 4, 5, 3, 8, 4, 20, 20, 20, 20,
                5, 10, 2, 5, 2, 4, 4, 2, 0, 20, 20, 20, 20, 20, 20, 20,
                3, 10, 2, 3, 5, 10, 3, 8, 5, 4, 5, 8, 0, 1, 9, 20,
                5, 10, 2, 5, 2, 4, 1, 9, 2, 9, 4, 2, 20, 20, 20, 20,
                8, 4, 5, 8, 5, 3, 3, 5, 1, 20, 20, 20, 20, 20, 20, 20,
                0, 4, 5, 1, 0, 5, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                8, 4, 5, 8, 5, 3, 9, 0, 5, 0, 3, 5, 20, 20, 20, 20,
                9, 4, 5, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                4, 11, 7, 4, 9, 11, 9, 10, 11, 20, 20, 20, 20, 20, 20, 20,
                0, 8, 3, 4, 9, 7, 9, 11, 7, 9, 10, 11, 20, 20, 20, 20,
                1, 10, 11, 1, 11, 4, 1, 4, 0, 7, 4, 11, 20, 20, 20, 20,
                3, 1, 4, 3, 4, 8, 1, 10, 4, 7, 4, 11, 10, 11, 4, 20,
                4, 11, 7, 9, 11, 4, 9, 2, 11, 9, 1, 2, 20, 20, 20, 20,
                9, 7, 4, 9, 11, 7, 9, 1, 11, 2, 11, 1, 0, 8, 3, 20,
                11, 7, 4, 11, 4, 2, 2, 4, 0, 20, 20, 20, 20, 20, 20, 20,
                11, 7, 4, 11, 4, 2, 8, 3, 4, 3, 2, 4, 20, 20, 20, 20,
                2, 9, 10, 2, 7, 9, 2, 3, 7, 7, 4, 9, 20, 20, 20, 20,
                9, 10, 7, 9, 7, 4, 10, 2, 7, 8, 7, 0, 2, 0, 7, 20,
                3, 7, 10, 3, 10, 2, 7, 4, 10, 1, 10, 0, 4, 0, 10, 20,
                1, 10, 2, 8, 7, 4, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                4, 9, 1, 4, 1, 7, 7, 1, 3, 20, 20, 20, 20, 20, 20, 20,
                4, 9, 1, 4, 1, 7, 0, 8, 1, 8, 7, 1, 20, 20, 20, 20,
                4, 0, 3, 7, 4, 3, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                4, 8, 7, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                9, 10, 8, 10, 11, 8, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                3, 0, 9, 3, 9, 11, 11, 9, 10, 20, 20, 20, 20, 20, 20, 20,
                0, 1, 10, 0, 10, 8, 8, 10, 11, 20, 20, 20, 20, 20, 20, 20,
                3, 1, 10, 11, 3, 10, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                1, 2, 11, 1, 11, 9, 9, 11, 8, 20, 20, 20, 20, 20, 20, 20,
                3, 0, 9, 3, 9, 11, 1, 2, 9, 2, 11, 9, 20, 20, 20, 20,
                0, 2, 11, 8, 0, 11, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                3, 2, 11, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                2, 3, 8, 2, 8, 10, 10, 8, 9, 20, 20, 20, 20, 20, 20, 20,
                9, 10, 2, 0, 9, 2, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                2, 3, 8, 2, 8, 10, 0, 1, 8, 1, 10, 8, 20, 20, 20, 20,
                1, 10, 2, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                1, 3, 8, 9, 1, 8, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 9, 1, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                0, 3, 8, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20
            };
            _pointsHash2EdgesIndexesTexture.SetPixelData(pointsHash2EdgesIndexes, 0);
            _pointsHash2EdgesIndexesTexture.Apply();

            _pointsBuffer = new ComputeBuffer(_pointsBufferSize, sizeof(float) * 4);
            _meshV3T1Buffer = new ComputeBuffer(_meshBufferSize, sizeof(float) * 4);

            _meshV3T1Values = new Vector4[_meshBufferSize];
        }
        protected void InitializeShader()
        {
            _shader.SetInt(_pointsPerChunk1DId, _pointsPerChunk1D);
            _shader.SetInt(_cubesPerChunk1DId, _cubesPerChunk1D);

            _shader.SetFloat(_surfaceValueId, _surfaceValue);
            _shader.SetFloat(_seedId, _seed);

            _shader.SetTexture(_generateMeshKernel, _edgeIndex2PointIndexesTextureId, _edgeIndex2PointIndexesTexture);
            _shader.SetTexture(_generateMeshKernel, _pointsHash2EdgesHashTextureId, _pointsHash2EdgesHashTexture);
            _shader.SetTexture(_generateMeshKernel, _pointsHash2EdgesIndexesTextureId, _pointsHash2EdgesIndexesTexture);

            _shader.SetBuffer(_generatePointsKernel, _pointsBufferId, _pointsBuffer);
            _shader.SetBuffer(_generateMeshKernel, _pointsBufferId, _pointsBuffer);
            _shader.SetBuffer(_generateMeshKernel, _meshV3T1BufferId, _meshV3T1Buffer);
        }

        public override void Dispose()
        {
            Object.Destroy(_edgeIndex2PointIndexesTexture);
            Object.Destroy(_pointsHash2EdgesHashTexture);
            Object.Destroy(_pointsHash2EdgesIndexesTexture);
            _pointsBuffer.Dispose();
            _meshV3T1Buffer.Dispose();
        }

        public override MeshData GenerateMeshData(Vector3Int chunkPosition, int cubeSize)
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample("CubeMarcher.GenerateMesh");
#endif
            DispatchPointsKernel(chunkPosition, cubeSize);
            MeshData meshData = DispatchCubesKernel();
#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif
            return meshData;
        }

        public override MarchPoint[] GeneratePoints(Vector3Int chunkPosition, int cubeSize)
        {
            DispatchPointsKernel(chunkPosition, cubeSize);

            MarchPoint[] points = new MarchPoint[_pointsBufferSize];
            _pointsBuffer.GetData(points);

            return points;
        }
        public override MeshData GenerateMeshData(MarchPoint[] points)
        {
            _pointsBuffer.SetData(points);

            return DispatchCubesKernel();
        }

        protected void DispatchPointsKernel(Vector3Int chunkPosition, int cubeSize)
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample("CubeMarcher.DispatchPointsKernel");
#endif
            _shader.SetVector(_chunkPositionId, (Vector3)chunkPosition);
            _shader.SetInt(_cubeSizeId, cubeSize);

            _shader.Dispatch(_generatePointsKernel, _threadGroups, _threadGroups, _threadGroups);
#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif
        }
        protected MeshData DispatchCubesKernel()
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample("CubeMarcher.DispatchCubesKernel");
#endif
            _shader.Dispatch(_generateMeshKernel, _threadGroups, _threadGroups, _threadGroups);

#if BIOSEARCHER_PROFILING
            Profiler.BeginSample("CubeMarcher.ReadMeshBuffer");
#endif
            _meshV3T1Buffer.GetData(_meshV3T1Values);
#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif
            MeshData meshData = ToMeshData(_meshV3T1Values);

#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif
            return meshData;
        }
    }
}
