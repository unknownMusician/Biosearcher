using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.LandGeneration
{
    [RequireComponent(typeof(CubeMarcherGPU))]
    public class LandManager : MonoBehaviour
    {
        [SerializeField] protected Vector3Int size = Vector3Int.one * 3;
        [SerializeField] protected float surfaceValue = 0.2f;
        [SerializeField] protected bool gizmos = true;
        [SerializeField] protected bool gizmosOnlyGround = true;
        [SerializeField] protected GameObject chunkPrefab;

        protected CubeMarcherGPU cubeMarcher;

        protected const int chunkSize = 6;
        protected const float cubeSize = 1; // todo: unused

        protected List<Cube> cubes = new List<Cube>();

        protected void Awake() => cubeMarcher = GetComponent<CubeMarcherGPU>();
        protected void Start() => Generate();

        public void Generate()
        {
            for (int z = 0, deltaZ = 0; deltaZ < size.z; deltaZ++, z += deltaZ * (int)Mathf.Pow(-1, deltaZ + 1))
            {
                for (int y = 0, deltaY = 0; deltaY < size.y; deltaY++, y += deltaY * (int)Mathf.Pow(-1, deltaY + 1))
                {
                    for (int x = 0, deltaX = 0; deltaX < size.x; deltaX++, x += deltaX * (int)Mathf.Pow(-1, deltaX + 1))
                    {
                        GenerateChunkGPU(new Vector3Int(x, y, z) * chunkSize);
                        //GenerateChunk(new Vector3Int(x, y, z) * chunkSize);
                    }
                }
            }
        }

        protected void GenerateChunkGPU(Vector3Int chunkPosition)
        {
            CubeMarcherGPU.MarchPoint[] points = cubeMarcher.GeneratePoints(chunkPosition);
            Mesh mesh = cubeMarcher.GenerateMesh(points, surfaceValue);

            CreateChunk(mesh, chunkPosition);
        }

        protected void GenerateChunk(Vector3Int chunkPosition)
        {
            PointsChunk points = GridGenerator.GeneratePointsChunk(chunkPosition, 6);
            Cube[] cubes = GridGenerator.ToCubes(points);
            Mesh mesh = CubeMarcher.GenerateMesh(cubes, surfaceValue);

            CreateChunk(mesh, chunkPosition);
        }

        protected void CreateChunk(Mesh mesh, Vector3Int chunkPosition)
        {
            GameObject chunkObject = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity, transform);
            chunkObject.GetComponent<MeshFilter>().mesh = mesh; // todo: expensive
        }

        //protected void OnDrawGizmos()
        //{
        //    //Gizmos.color = Color.black;
        //    //Gizmos.DrawWireCube(transform.position, size * 2); // todo

        //    if (!gizmos)
        //    {
        //        return;
        //    }

        //    foreach (Cube cube in cubes)
        //    {
        //        Point point = cube.Points[0];
        //        if (point.Value < surfaceValue && gizmosOnlyGround)
        //        {
        //            continue;
        //        }
        //        Gizmos.color = new Color(point.Value, point.Value, point.Value);
        //        Gizmos.DrawSphere(point.Position, 0.1f);
        //    }
        //}
    }
}
