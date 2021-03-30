using System.Collections;
using UnityEngine;

namespace Biosearcher.Planet.Generation
{
    public interface ICubeMarcher
    {
        MarchPoint[] GeneratePoints(Vector3Int chunkPosition, int cubeSize);
        Mesh GenerateMesh(MarchPoint[] points);
    }

    public struct MarchPoint
    {
        public Vector3 position;
        public float value;
    }
}