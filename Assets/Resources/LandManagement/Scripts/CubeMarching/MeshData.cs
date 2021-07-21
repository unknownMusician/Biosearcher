using UnityEngine;

namespace Biosearcher.LandManagement.CubeMarching
{
    public sealed class MeshData
    {
        public Vector3[] Vertices { get; }
        public int[] Triangles { get; }
        public Ray[] Normals { get; }

        public MeshData(Vector3[] vertices, int[] triangles, Ray[] normals)
        {
            Vertices = vertices;
            Triangles = triangles;
            Normals = normals;
        }
    }
}