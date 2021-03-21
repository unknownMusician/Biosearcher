using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.Land.Generation
{
    [RequireComponent(typeof(CubeMarcherGPU))]
    public class LandManager : MonoBehaviour
    {
        [SerializeField] protected Vector3Int size = Vector3Int.one * 3;
        [SerializeField] protected float surfaceValue = 0.2f;
        [SerializeField] protected GameObject chunkPrefab;

        protected CubeMarcherGPU cubeMarcher;

        protected const int chunkSize = 6;
        protected const int cubeSize = 1;

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
                        //GenerateChunkGPU(new Vector3Int(x, y, z) * chunkSize);
                        GenerateChunk(new Vector3Int(x, y, z) * chunkSize);
                    }
                }
            }
        }

        // todo: put all generating info in one and only one place
        protected void GenerateChunkGPU(Vector3Int chunkPosition)
        {
            CubeMarcherGPU.MarchPoint[] points = cubeMarcher.GeneratePoints(chunkPosition, cubeSize);
            Mesh mesh = cubeMarcher.GenerateMesh(points, surfaceValue);

            CreateChunk(mesh, chunkPosition);
        }

        protected void GenerateChunk(Vector3Int chunkPosition)
        {
            PointsChunk points = GridGenerator.GeneratePointsChunk(chunkPosition, chunkSize, cubeSize);
            Cube[] cubes = GridGenerator.ToCubes(points);
            Mesh mesh = CubeMarcher.GenerateMesh(cubes, surfaceValue);

            CreateChunk(mesh, chunkPosition);
        }

        protected void CreateChunk(Mesh mesh, Vector3Int chunkPosition)
        {
            GameObject chunkObject = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity, transform);
            chunkObject.GetComponent<MeshFilter>().mesh = mesh; // todo: expensive
            chunkObject.AddComponent<MeshCollider>().sharedMesh = mesh;
        }

        public void TerraformAdd(Vector3 position, float radius)
        {
            // todo
        }
    }
}
