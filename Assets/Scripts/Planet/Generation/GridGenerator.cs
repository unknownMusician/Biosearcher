using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.Planet.Generation
{
    public class GridGenerator
    {
        public MarchPoint[] GeneratePoints(Vector3Int chunkPosition, int cubeSize)
        {
            int halfChunkSize = CubeMarcherConfig.cubesChunkSize / 2;

            int pointsArray1DSize = halfChunkSize * 2 + 1;
            var points = new MarchPoint[pointsArray1DSize * pointsArray1DSize * pointsArray1DSize];

            for (int z = -halfChunkSize, zIndex = 0; z <= halfChunkSize; z++, zIndex++)
            {
                for (int y = -halfChunkSize, yIndex = 0; y <= halfChunkSize; y++, yIndex++)
                {
                    for (int x = -halfChunkSize, xIndex = 0; x <= halfChunkSize; x++, xIndex++)
                    {
                        Vector3Int position = new Vector3Int(x, y, z) * cubeSize;
                        int pointIndex = MatrixId2ArrayId(xIndex, yIndex, zIndex, CubeMarcherConfig.pointsChunkSize);
                        points[pointIndex] = new MarchPoint()
                        {
                            position = position,
                            value = GenerateValue(position + chunkPosition)
                        };
                    }
                }
            }
            return points;
        }

        protected float GenerateValue(Vector3 position)
        {
            float result = 1;

            // todo
            result *= 1 - GradientNoise(position / (6 * (1 << 1))) / 16;
            result *= 1 - GradientNoise(position / (6 * (1 << 2))) / 8;
            result *= 1 - GradientNoise(position / (6 * (1 << 3))) / 4;
            result *= 1 - GradientNoise(position / (6 * (1 << 4))) / 2;
            result *= 1 - GradientNoise(position / (6 * (1 << 5))) / 1.5f;

            // 100 - planet size (todo)
            result *= 1 - Mathf.Clamp01(position.magnitude / 400);

            return result;
        }

        protected float Noise(Vector3 position)
        {
            float noise = (Mathf.Sin(Vector3.Dot(position, new Vector3(12.9898f, 78.233f, 128.544f) * 2.0f) * position.magnitude) * 43758.5453f) % 1;
            return Mathf.Abs(noise);
        }

        protected float GradientNoise(Vector3 position)
        {
            Vector3Int wholePart = new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));
            // todo
            Vector3 fractPart = new Vector3(position.x % 1, position.y % 1, position.z % 1);
            // todo
            fractPart += new Vector3(position.x < 0 && fractPart.x != 0 ? 1 : 0, position.y < 0 && fractPart.y != 0 ? 1 : 0, position.z < 0 && fractPart.z != 0 ? 1 : 0);
            float[] noisesZ = new float[2];
            for (int z = 0; z < 2; z++)
            {
                float[] noisesY = new float[2];
                for (int y = 0; y < 2; y++)
                {
                    float[] noisesX = new float[2];
                    for (int x = 0; x < 2; x++)
                    {
                        noisesX[x] = Noise(wholePart + new Vector3(x, y, z));
                    }
                    noisesY[y] = Mathf.Lerp(noisesX[0], noisesX[1], fractPart.x);
                }
                noisesZ[z] = Mathf.Lerp(noisesY[0], noisesY[1], fractPart.y);
            }
            return Mathf.Lerp(noisesZ[0], noisesZ[1], fractPart.z);
        }

        public MarchCube[] ToCubes(MarchPoint[] points)
        {
            var cubes = new List<MarchCube>();
            int cubesChunkSize = CubeMarcherConfig.cubesChunkSize;

            for (int zIndex = 0; zIndex < cubesChunkSize; zIndex++)
            {
                for (int yIndex = 0; yIndex < cubesChunkSize; yIndex++)
                {
                    for (int xIndex = 0; xIndex < cubesChunkSize; xIndex++)
                    {
                        cubes.Add(GenerateCube(points, xIndex, yIndex, zIndex));
                    }
                }
            }
            return cubes.ToArray();
        }

        public MarchCube GenerateCube(MarchPoint[] points, int xIndex, int yIndex, int zIndex)
        {
            var pointsList = new List<MarchPoint>();

            for (int y = 0; y < 2; y++)
            {
                for (int z = 0; z < 2; z++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        int localXIndex = z == 0 ? x : 1 - x;
                        int localYIndex = y;
                        int localZIndex = 1 - z;
                        int pointIndex = MatrixId2ArrayId(xIndex + localXIndex, yIndex + localYIndex, zIndex + localZIndex, CubeMarcherConfig.pointsChunkSize);
                        pointsList.Add(points[pointIndex]);
                    }
                }
            }
            return new MarchCube(pointsList.ToArray());
        }

        public int MatrixId2ArrayId(int x, int y, int z, int chunkSize)
        {
            return x + y * chunkSize + z * chunkSize * chunkSize;
        }
    }
}