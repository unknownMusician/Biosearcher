using System.Collections.Generic;
using UnityEngine;
#if BIOSEARCHER_PROFILING
using UnityEngine.Profiling;
#endif

namespace Biosearcher.LandManagement.CubeMarching
{
    public abstract class CubeMarcher : System.IDisposable
    {
        public Mesh GenerateMesh(Vector3Int chunkPosition, int cubeSize)
        {
            return ToMesh(GenerateMeshData(chunkPosition, cubeSize));
        }

        public abstract MarchPoint[] GeneratePoints(Vector3Int chunkPosition, int cubeSize);
        public Mesh GenerateMesh(MarchPoint[] points)
        {
            return ToMesh(GenerateMeshData(points));
        }
        public abstract MeshData GenerateMeshData(Vector3Int chunkPosition, int cubeSize);
        public abstract MeshData GenerateMeshData(MarchPoint[] points);

        protected MeshData ToMeshData(Vector4[] meshV3T1)
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample("ToMeshData");
#endif

            List<Vector3> newVertices = new List<Vector3>();
            List<int> newTriangles = new List<int>();
            int i, j;
            int counter = 0;
            Vector4 facePart;
            Vector3[] faceVertices = new Vector3[3];
            int[] faceTriangles = new int[3];
            for (i = 0; i < meshV3T1.Length; i += 3)
            {
                for (j = 0; j < 3; j++)
                {
                    facePart = meshV3T1[i + j];
                    if (facePart.w == -1)
                    {
                        break;
                    }
                    faceVertices[j] = facePart;
                    faceTriangles[j] = counter + j;
                }
                if (j < 3)
                {
                    continue;
                }
#if BIOSEARCHER_PROFILING
                Profiler.BeginSample("AddToList");
#endif
                newVertices.AddRange(faceVertices);
                newTriangles.AddRange(faceTriangles);
#if BIOSEARCHER_PROFILING
                Profiler.EndSample();
#endif
                counter += 3;
            }

            var meshData = new MeshData(newVertices.ToArray(), newTriangles.ToArray());
#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif
            return meshData;
        }
        protected Mesh ToMesh(Vector3[] cleanVertices, int[] cleanTriangles)
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample("MeshInstantiatingAndAssigning");
#endif
            var mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = cleanVertices;
            mesh.triangles = cleanTriangles;
            mesh.RecalculateNormals();
#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif
            return mesh;
        }
        public Mesh ToMesh(MeshData meshData) => ToMesh(meshData.Vertices, meshData.Triangles);

        public abstract void Dispose();
    }
}