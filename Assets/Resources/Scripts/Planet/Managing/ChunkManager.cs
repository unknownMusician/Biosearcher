using Biosearcher.HDRP;
using Biosearcher.Planet.Generation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] protected int generatingFrequency = 1;

        protected ICubeMarcher cubeMarcher;
        protected ChunkHolder mainChunk;
        protected QueueWorker<(Vector3Int, int, ChunkHolder), (Mesh, GameObject)> generateWorker;

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
            generateWorker = new QueueWorker<(Vector3Int, int, ChunkHolder), (Mesh, GameObject)>(this, GenerateChunkJob, generatingFrequency);
        }

        protected void OnDestroy()
        {
            generateWorker.Dispose();
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

        protected internal GeometryHolder GenerateChunk(Vector3Int chunkPosition, int cubeSize, ChunkHolder parent)
        {
            var input = (chunkPosition, cubeSize, parent);
            var geometry = new GeometryCredit() { onCancel = () => generateWorker.TryRemoveRequest(input) };
            var geometryHolder = new GeometryHolder() { Geometry = geometry };
            generateWorker.MakeRequest(input, output => OnWorkerJobDone(output, geometryHolder));
            return geometryHolder;
        }

        protected void OnWorkerJobDone((Mesh mesh, GameObject chunkObject) output, GeometryHolder geometryHolder)
        {
            geometryHolder.Geometry = new Geometry() { chunkObject = output.chunkObject, mesh = output.mesh };
        }

        protected (Mesh, GameObject) GenerateChunkJob((Vector3Int chunkPosition, int cubeSize, ChunkHolder parent) input)
        {
            //float todoTimeStart = Time.realtimeSinceStartup;
            MarchPoint[] points = cubeMarcher.GeneratePoints(input.chunkPosition, input.cubeSize);
            //Debug.Log($"Generating points: {(Time.realtimeSinceStartup - todoTimeStart) * 1000} ms");
            //todoTimeStart = Time.realtimeSinceStartup;
            Mesh generatedMesh = cubeMarcher.GenerateMesh(points);
            //Debug.Log($"Generating Mesh: {(Time.realtimeSinceStartup - todoTimeStart) * 1000} ms");

            GameObject generatedChunkObject = CreateChunk(generatedMesh, input.chunkPosition, input.parent);
            return (generatedMesh, generatedChunkObject);
        }

        protected GameObject CreateChunk(Mesh mesh, Vector3Int chunkPosition, ChunkHolder parent)
        {
            //float todoTimeStart = Time.realtimeSinceStartup;
            GameObject generatedChunkObject = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity, transform);
            generatedChunkObject.GetComponent<MeshFilter>().mesh = mesh;
            generatedChunkObject.GetComponent<MeshCollider>().sharedMesh = mesh;
            //Debug.Log($"Instantiating Chunks: {(Time.realtimeSinceStartup - todoTimeStart) * 1000} ms");
            return generatedChunkObject;
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