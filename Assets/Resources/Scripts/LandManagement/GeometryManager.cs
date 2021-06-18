using Biosearcher.LandManagement.Chunks;
using Biosearcher.LandManagement.CubeMarching;
using UnityEngine;

namespace Biosearcher.LandManagement
{
    public class GeometryManager : System.IDisposable
    {
        protected QueueWorker<(Vector3Int, int), Geometry> generateWorker;
        protected LandSettings settings;
        protected Land land;
        protected CubeMarcher cubeMarcher;

        public GeometryManager(Land land, LandSettings settings, CubeMarcher cubeMarcher)
        {
            this.settings = settings;
            this.land = land;
            this.cubeMarcher = cubeMarcher;
            generateWorker = new QueueWorker<(Vector3Int, int), Geometry>(GenerateChunkJob, settings.GeneratingFrequency);
        }

        protected internal void Create(ChunkWithGeometry chunk)
        {
            var input = (chunk.Position, chunk.HierarchySize);
            generateWorker.MakeRequest(input, output => OnWorkerJobDone(output, chunk));
        }

        protected internal void Clear(Geometry geometry, ChunkWithGeometry chunk)
        {
            Object.Destroy(geometry.chunkMesh);
            Object.Destroy(geometry.chunkObject);
        }

        protected Geometry GenerateChunkJob((Vector3Int chunkPosition, int hierarchySize) input)
        {
            MarchPoint[] points = cubeMarcher.GeneratePoints(input.chunkPosition, 1 << input.hierarchySize);
            Mesh generatedMesh = cubeMarcher.GenerateMesh(points);

            GameObject chunkPrefab = settings.ChunkPrefab;
            return new Geometry() { chunkObject = chunkPrefab, chunkMesh = generatedMesh };
        }

        protected internal Geometry InstantiateChunk(Geometry geometry, ChunkWithGeometry chunk)
        {
            GameObject generatedChunkObject = GameObject.Instantiate(settings.ChunkPrefab, chunk.Position, Quaternion.identity, land.transform);
            generatedChunkObject.GetComponent<MeshFilter>().mesh = geometry.chunkMesh;
            generatedChunkObject.GetComponent<MeshCollider>().sharedMesh = geometry.chunkMesh;
            return new Geometry() { chunkMesh = geometry.chunkMesh, chunkObject = generatedChunkObject };
        }

        protected void OnWorkerJobDone(Geometry output, ChunkWithGeometry chunk) => chunk.Initialize(output);

        public void Dispose() => generateWorker.Dispose();
    }
}