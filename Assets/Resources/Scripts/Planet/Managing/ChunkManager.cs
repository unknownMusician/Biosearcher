using Biosearcher.HDRP;
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
        [SerializeField] protected GameObject chunkPrefab;
        [SerializeField] protected Vector3Int planetPosition;
        [SerializeField] protected Vector3 rotationAxis;
        [SerializeField] protected float gravityScale = 9.8f; // todo: move
        [SerializeField] protected CubeMarchType marchType;
        [SerializeField] protected MarchingCubesSettings settings;

        protected ICubeMarcher cubeMarcher;
        protected ChunkHolder mainChunk;

        protected internal Transform Trigger => trigger;
        protected internal GameObject ChunkPrefab => chunkPrefab;
        public Vector3 RotationAxis => rotationAxis;
        public Vector3Int PlanetPosition => planetPosition;
        public float GravityScale => gravityScale;

        protected void Awake()
        {
            if (marchType == CubeMarchType.GPU)
            {
                cubeMarcher = GetComponent<CubeMarcherGPU>();
            }
            else if (marchType == CubeMarchType.CPU)
            {
                cubeMarcher = new CubeMarcherCPU(settings);
            }
            SkyGameManager.planetPosition = planetPosition;
        }

        protected void Start()
        {
            mainChunk = new ChunkHolder(null, this);
            mainChunk.Initialize(Chunk.Create(planetPosition, size, mainChunk, trigger.position));
        }

        protected void OnDrawGizmos()
        {
            mainChunk?.Chunk.DrawGizmos();
        }

        protected internal void GenerateChunk(Vector3Int chunkPosition, int cubeSize, ChunkHolder parent, out Mesh generatedMesh, out GameObject generatedChunkObject)
        {
            float todoTimeStart = Time.realtimeSinceStartup;
            MarchPoint[] points = cubeMarcher.GeneratePoints(chunkPosition, cubeSize);
            Debug.Log($"Generating points: {(Time.realtimeSinceStartup - todoTimeStart) * 1000} ms");
            todoTimeStart = Time.realtimeSinceStartup;
            generatedMesh = cubeMarcher.GenerateMesh(points);
            Debug.Log($"Generating Mesh: {(Time.realtimeSinceStartup - todoTimeStart) * 1000} ms");

            CreateChunk(generatedMesh, chunkPosition, parent, out generatedChunkObject);
        }

        protected void CreateChunk(Mesh mesh, Vector3Int chunkPosition, ChunkHolder parent, out GameObject generatedChunkObject)
        {
            float todoTimeStart = Time.realtimeSinceStartup;
            generatedChunkObject = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity, transform);
            generatedChunkObject.GetComponent<MeshFilter>().mesh = mesh;
            generatedChunkObject.GetComponent<MeshCollider>().sharedMesh = mesh;
            Debug.Log($"Instantiating Chunks: {(Time.realtimeSinceStartup - todoTimeStart) * 1000} ms");
        }

        public void TerraformAdd(Vector3 position, float radius)
        {
            // todo
        }

        public enum CubeMarchType
        {
            CPU,
            GPU
        }
    }
}