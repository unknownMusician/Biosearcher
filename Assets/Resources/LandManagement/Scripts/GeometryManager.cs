using Biosearcher.Common;
using Biosearcher.LandManagement.Chunks;
using Biosearcher.LandManagement.CubeMarching;
using Biosearcher.LandManagement.CubeMarching.CPU;
using Biosearcher.LandManagement.CubeMarching.GPU;
using Biosearcher.LandManagement.QueueWorkers;
using Biosearcher.LandManagement.Settings;
using Biosearcher.Refactoring;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Biosearcher.LandManagement
{
    public class GeometryManager : System.IDisposable
    {
        protected QueueWorker<(Vector3Int, int), MeshData> _generateWorker;
        protected LandSettings _settings;
        protected Land _land;
        protected CubeMarcher _cubeMarcher;

        public GeometryManager(Land land, LandSettings settings)
        {
            _settings = settings;
            _land = land;

            GetMarcherAndWorker(out _cubeMarcher, out _generateWorker);
        }

        protected void GetMarcherAndWorker(out CubeMarcher marcher, out QueueWorker<(Vector3Int, int), MeshData> worker)
        {
            switch (_settings.CubeMarchingType)
            {
                case CubeMarchingType.GPU:
                    marcher = new CubeMarcherGPU(_settings);
                    worker = new CoroutineQueueWorker<(Vector3Int, int), MeshData>(GenerateChunkJob, _settings.GeneratingFrequency);
                    break;
                case CubeMarchingType.CPU:
                    marcher = new CubeMarcherCPU(_settings);
                    worker = GetAsyncWorker();
                    break;
                default:
                    throw new System.ArgumentException();
            }
        }
        protected QueueWorker<(Vector3Int, int), MeshData> GetAsyncWorker()
        {
            return _settings.AsyncType switch
            {
                AsyncType.Jobs => new JobQueueWorker<(Vector3Int, int), MeshData>(GenerateChunkJob, _settings.GeneratingFrequency),
                AsyncType.Thread => new ThreadQueueWorker<(Vector3Int, int), MeshData>(GenerateChunkJob, _settings.GeneratingFrequency),
                _ => throw new System.ArgumentException()
            };
        }

        protected internal void Create(ChunkWithGeometry chunk)
        {
            var input = (chunk.Position, chunk.HierarchySize);
            _generateWorker.MakeRequest(input, output => OnWorkerJobDone(output, chunk));
        }

        protected internal void Clear(Geometry geometry, ChunkWithGeometry chunk)
        {
            Object.Destroy(geometry.chunkMesh);
            Object.Destroy(geometry.chunkObject);
            PlantsSpawner.Dispose(geometry.plants);
            geometry.normals = null;
        }

        protected MeshData GenerateChunkJob((Vector3Int chunkPosition, int hierarchySize) input)
        {
            return _cubeMarcher.GenerateMeshData(input.chunkPosition, Chunk.HierarchySize2CubeSize(input.hierarchySize));
        }

        [NeedsRefactor]
        protected internal void InstantiateChunk(ref Geometry geometry, ChunkWithGeometry chunk)
        {
            GameObject generatedChunkObject = Object.Instantiate(_settings.ChunkPrefab, chunk.Position, Quaternion.identity, _land.transform);
            generatedChunkObject.GetComponent<MeshFilter>().mesh = geometry.chunkMesh;
            generatedChunkObject.GetComponent<MeshCollider>().sharedMesh = geometry.chunkMesh;

            // remove LINQ
            IEnumerable<Ray> normals = geometry.normals.Select(normal => new Ray(normal.origin + chunk.Position, normal.direction));
            //normals.Foreach(normal => Debug.DrawRay(normal.origin, normal.direction, Color.white, 20f));
            IEnumerable<GameObject> plants = PlantsSpawner.Spawn(normals);

            geometry = new Geometry() { chunkMesh = geometry.chunkMesh, chunkObject = generatedChunkObject, normals = geometry.normals, plants = plants };
        }

        protected void OnWorkerJobDone(MeshData output, ChunkWithGeometry chunk)
        {
            Mesh generatedMesh = _cubeMarcher.ToMesh(output);
            GameObject chunkPrefab = _settings.ChunkPrefab;
            Ray[] normals = output.Normals;

            var geometry = new Geometry() { chunkObject = chunkPrefab, chunkMesh = generatedMesh, normals = normals };
            chunk.Initialize(geometry);
        }

        public void Dispose()
        {
            _generateWorker.Dispose();
            _cubeMarcher.Dispose();
        }
    }
}