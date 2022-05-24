using System;
using System.Collections.Generic;
using System.Linq;
using AreYouFruits.Common.ComponentGeneration;
using Biosearcher.CubeMarching;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace Biosearcher.WorldGeneration
{
    [HasComponent(true)]
    public class ChunkGenerator : IChunkGenerator
    {
        private readonly GameObject _chunkPrefab;
        private readonly Transform _chunkParent;

        private readonly ICubeMarcher _cubeMarcher;
        private readonly IEnumerable<IPropSpawner<PropSpawnerInfo, GameObject>> _propSpawners;

        private Vector3[] _points;
        private float[] _weights;
        private Vector3[] _vertices;

        public ChunkGenerator(
            GameObject chunkPrefab, Transform chunkParent, ICubeMarcher cubeMarcher,
            IEnumerable<IPropSpawner<PropSpawnerInfo, GameObject>> propSpawners
        )
        {
            _chunkPrefab = chunkPrefab;
            _chunkParent = chunkParent;
            _cubeMarcher = cubeMarcher;
            _propSpawners = propSpawners;

            _points = _cubeMarcher.CreatePointsBuffer();
            _weights = _cubeMarcher.CreateWeightsBuffer();
            _vertices = _cubeMarcher.CreateVerticesBuffer();
        }

        public GameObject[] GenerateChunk(Vector3 offset)
        {
            Profiler.BeginSample($"{GetType().Name}.{nameof(GenerateChunk)}");

            if (!TryGenerateChunkLand(offset, out GameObject? chunkLand, out Vector3[] vertices))
            {
                Profiler.EndSample();

                return Array.Empty<GameObject>();
            }

            var gameObjects = new List<GameObject> { chunkLand! };

            var propSpawnerInfo = new PropSpawnerInfo { Offset = offset, Vertices = vertices };

            foreach (IPropSpawner<PropSpawnerInfo, GameObject> propSpawner in _propSpawners)
            {
                propSpawner.Spawn(gameObjects, propSpawnerInfo);
            }

            GameObject[] result = gameObjects.ToArray();

            Profiler.EndSample();
            
            return result;
        }

        public bool TryGenerateChunkLand(Vector3 offset, out GameObject? chunkLand, out Vector3[] vertices)
        {
            Profiler.BeginSample($"{GetType().Name}.{nameof(TryGenerateChunkLand)}");
            
            if (!_cubeMarcher.TryGenerateMesh(offset, _points, _weights, _vertices, out Mesh? mesh, out vertices))
            {
                chunkLand = null;

                Profiler.EndSample();

                return false;
            }
            
            chunkLand = Object.Instantiate(_chunkPrefab, _chunkParent);

            chunkLand.transform.position = offset;
                
            chunkLand.GetComponent<MeshFilter>().mesh = mesh;
            chunkLand.GetComponent<MeshCollider>().sharedMesh = mesh;

            Profiler.EndSample();

            return true;
        }
    }
}
