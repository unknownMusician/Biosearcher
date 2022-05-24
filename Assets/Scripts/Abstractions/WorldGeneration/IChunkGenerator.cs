using UnityEngine;

namespace Biosearcher.WorldGeneration
{
    public interface IChunkGenerator
    {
        public GameObject[] GenerateChunk(Vector3 offset);
    }
}