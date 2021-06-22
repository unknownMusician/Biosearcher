using UnityEngine;

namespace Biosearcher.LandManagement.CubeMarching
{
    public sealed class MeshData
    {
        public Vector3[] Vertices { get; private set; }
        public int[] Triangles { get; private set; }

        public MeshData(Vector3[] vertices, int[] triangles)
        {
            Vertices = vertices;
            Triangles = triangles;
        }
    }
}