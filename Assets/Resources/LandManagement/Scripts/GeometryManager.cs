using Biosearcher.LandManagement.Chunks;
using Biosearcher.LandManagement.CubeMarching;
using UnityEngine;

namespace Biosearcher.LandManagement
{
    public class GeometryManager : System.IDisposable
    {
        protected QueueWorker<(Vector3Int, int), Geometry> _generateWorker;
        protected LandSettings _settings;
        protected Land _land;
        protected CubeMarcher _cubeMarcher;

        public GeometryManager(Land land, LandSettings settings, CubeMarcher cubeMarcher)
        {
            _settings = settings;
            _land = land;
            _cubeMarcher = cubeMarcher;
            _generateWorker = new QueueWorker<(Vector3Int, int), Geometry>(GenerateChunkJob, settings.GeneratingFrequency);
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
        }

        protected Geometry GenerateChunkJob((Vector3Int chunkPosition, int hierarchySize) input)
        {
            Mesh generatedMesh = _cubeMarcher.GenerateMesh(input.chunkPosition, 1 << input.hierarchySize);
            GameObject chunkPrefab = _settings.ChunkPrefab;

            return new Geometry() { chunkObject = chunkPrefab, chunkMesh = generatedMesh };
        }

        protected internal Geometry InstantiateChunk(Geometry geometry, ChunkWithGeometry chunk)
        {
            GameObject generatedChunkObject = Object.Instantiate(_settings.ChunkPrefab, chunk.Position, Quaternion.identity, _land.transform);
            generatedChunkObject.GetComponent<MeshFilter>().mesh = geometry.chunkMesh;
            generatedChunkObject.GetComponent<MeshCollider>().sharedMesh = geometry.chunkMesh;
            return new Geometry() { chunkMesh = geometry.chunkMesh, chunkObject = generatedChunkObject };
        }

        protected void OnWorkerJobDone(Geometry output, ChunkWithGeometry chunk) => chunk.Initialize(output);

        public void Dispose() => _generateWorker.Dispose();
    }
}