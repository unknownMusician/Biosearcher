using Biosearcher.Planet.Generation;
using System.Collections;
using UnityEngine;

namespace Biosearcher.Planet.Managing
{
    [RequireComponent(typeof(CubeMarcherGPU))]
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField] protected Transform trigger;
        [SerializeField] protected int size;
        [SerializeField] protected float surfaceValue = 0.2f;
        [SerializeField] protected GameObject chunkPrefab;
        [SerializeField] protected Vector3Int planetPosition;
        [SerializeField] protected Vector3 rotationAxis;
        [SerializeField] protected float gravityScale = 9.8f; // todo: move

        protected CubeMarcherGPU cubeMarcher;
        protected ChunkHolder mainChunk;

        protected const int chunkSize = 6;

        protected internal Transform Trigger => trigger;
        protected internal float SurfaceValue => surfaceValue;
        protected internal GameObject ChunkPrefab => chunkPrefab;
        public Vector3 RotationAxis => rotationAxis;
        public Vector3Int PlanetPosition => planetPosition;
        public float GravityScale => gravityScale;

        protected void Awake() => cubeMarcher = GetComponent<CubeMarcherGPU>();

        protected void Start()
        {
            mainChunk = new ChunkHolder(null, this);
            mainChunk.Initialize(Chunk.Create(planetPosition, size, mainChunk, trigger.position));
        }

        protected void OnDrawGizmos()
        {
            mainChunk?.Chunk.DrawGizmos();
        }

        protected internal void GenerateChunkGPU(Vector3Int chunkPosition, int cubeSize, ChunkHolder parent, out Mesh generatedMesh, out GameObject generatedChunkObject)
        {
            CubeMarcherGPU.MarchPoint[] points = cubeMarcher.GeneratePoints(chunkPosition, cubeSize);
            generatedMesh = cubeMarcher.GenerateMesh(points, surfaceValue);

            CreateChunk(generatedMesh, chunkPosition, parent, out generatedChunkObject);
        }

        protected internal void GenerateChunk(Vector3Int chunkPosition, int cubeSize, ChunkHolder parent, out Mesh generatedMesh, out GameObject generatedChunkObject)
        {
            PointsChunk points = GridGenerator.GeneratePointsChunk(chunkPosition, chunkSize, cubeSize);
            Cube[] cubes = GridGenerator.ToCubes(points);
            generatedMesh = CubeMarcher.GenerateMesh(cubes, surfaceValue);

            CreateChunk(generatedMesh, chunkPosition, parent, out generatedChunkObject);
        }

        protected void CreateChunk(Mesh mesh, Vector3Int chunkPosition, ChunkHolder parent, out GameObject generatedChunkObject)
        {
            generatedChunkObject = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity, transform);
            generatedChunkObject.GetComponent<MeshFilter>().mesh = mesh;
            generatedChunkObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        public void TerraformAdd(Vector3 position, float radius)
        {
            // todo
        }
    }
}