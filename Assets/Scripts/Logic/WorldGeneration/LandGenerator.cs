using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AreYouFruits.Common.ComponentGeneration;
using Biosearcher.CubeMarching;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Biosearcher.WorldGeneration
{
    [HasComponent(true)]
    public class LandGenerator : IDisposable
    {
        private readonly ICubeMarcher _cubeMarcher;
        private readonly IChunkGenerator _chunkGenerator;
        private readonly Transform _parent;
        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();

        public Vector3Int ChunkCount;
        public Vector3 ChunkOffset;

        public LandGenerator(
            ICubeMarcher cubeMarcher, IChunkGenerator chunkGenerator, Transform parent, Vector3Int chunkCount
        )
        {
            _cubeMarcher = cubeMarcher;
            _chunkGenerator = chunkGenerator;
            _parent = parent;
            ChunkCount = chunkCount;
        }

        public async void Generate()
        {
            foreach (Transform child in _parent)
            {
                Object.Destroy(child.gameObject);
            }

            // todo
            int totalChunkCount = ChunkCount.x * ChunkCount.y * ChunkCount.z;
            int generatedChunksCount = 0;
            var watch = new Stopwatch();
            watch.Start();

            ChunkOffset -= _cubeMarcher.Cubes1DCount * _cubeMarcher.SingleCube1DSize / 2 * (Vector3)ChunkCount;

            for (int x = 0; x < ChunkCount.x && !_cancellationSource.IsCancellationRequested; x++)
            {
                for (int y = 0; y < ChunkCount.y && !_cancellationSource.IsCancellationRequested; y++)
                {
                    for (int z = 0; z < ChunkCount.z && !_cancellationSource.IsCancellationRequested; z++)
                    {
                        Vector3 chunkPosition = _parent.position
                          + ChunkOffset
                          + _cubeMarcher.Cubes1DCount * _cubeMarcher.SingleCube1DSize * new Vector3(x, y, z);

                        _chunkGenerator.GenerateChunk(chunkPosition);

                        generatedChunksCount++;
                        
                        Debug.Log($"Generating: {Mathf.Floor(generatedChunksCount * 100.0f / totalChunkCount)} %"
                          + $"({generatedChunksCount} / {totalChunkCount} chunks)");
                        
                        //await Task.Yield();
                    }
                }
            }

            watch.Stop();

            Debug.Log(
                "Generation Done: "
              + $"{ChunkCount.x * ChunkCount.y * ChunkCount.z} chunks - {watch.Elapsed.TotalMilliseconds} ms"
            );
        }

        public void Dispose() => _cancellationSource.Cancel();
    }
}
