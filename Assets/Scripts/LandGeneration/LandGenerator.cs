using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.LandGeneration
{
    [RequireComponent(typeof(MeshFilter))]
    public class LandGenerator : MonoBehaviour
    {
        [SerializeField] protected Vector3 position = Vector3.zero;
        [SerializeField] protected Vector3 size = Vector3.one * 3;
        [SerializeField] protected float cubeSize = 1;
        [SerializeField] protected float surfaceValue = 0.5f;
        [SerializeField] protected bool gizmos = true;
        [SerializeField] protected bool gizmosOnlyGround = true;

        protected Cube[] cubes = new Cube[0];

        protected void Start() => Generate();

        public void Generate()
        {
            cubes = GridGenerator.Generate(size, cubeSize);
            MeshData meshData = CubeMarcher.March(cubes, surfaceValue);
            SetMesh(meshData);
        }

        public void SetMesh(MeshData meshData)
        {
            Mesh mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh; // todo: expensive

            mesh.Clear();

            mesh.vertices = meshData.Vertices;
            mesh.triangles = meshData.Triangles;

            mesh.RecalculateNormals();
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(position, size * 2);

            if (!gizmos)
            {
                return;
            }

            foreach (Cube cube in cubes)
            {
                Point point = cube.Points[0];
                if (point.Value < surfaceValue && gizmosOnlyGround)
                {
                    continue;
                }
                Gizmos.color = new Color(point.Value, point.Value, point.Value);
                Gizmos.DrawSphere(point.Position, 0.1f);
            }
        }
    }
}
