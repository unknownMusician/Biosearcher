using System.Collections;
using UnityEngine;

namespace Biosearcher.Planet.Generation
{
    public interface ICubeMarcher
    {
        MarchPoint[] GeneratePoints(Vector3Int chunkPosition, int cubeSize);
        Mesh GenerateMesh(MarchPoint[] points, float surfaceValue);
    }

    public static class CubeMarcherConfig
    {
        public static readonly int cubesChunkSize = 6;
        public static readonly int pointsChunkSize = cubesChunkSize + 1;
    }

    public struct MarchPoint
    {
        public Vector3 position;
        public float value;
    }
}