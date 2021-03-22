using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.Planet.Generation
{
    public static class GridGenerator
    {
        public static PointsChunk GeneratePointsChunk(Vector3Int chunkPosition, int chunkSize, int cubeSize)
        {
            int halfChunkSize = chunkSize / 2;

            int pointsArray1DSize = halfChunkSize * 2 + 1;
            var points = new Point[pointsArray1DSize, pointsArray1DSize, pointsArray1DSize];

            for (int z = -halfChunkSize, zIndex = 0; z <= halfChunkSize; z++, zIndex++)
            {
                for (int y = -halfChunkSize, yIndex = 0; y <= halfChunkSize; y++, yIndex++)
                {
                    for (int x = -halfChunkSize, xIndex = 0; x <= halfChunkSize; x++, xIndex++)
                    {
                        Vector3Int position = new Vector3Int(x, y, z) * cubeSize;
                        points[xIndex, yIndex, zIndex] = new Point(position, GenerateValue(position + chunkPosition));
                    }
                }
            }
            return new PointsChunk(points, pointsArray1DSize);
        }

        private static float GenerateValue(Vector3 position)
        {
            float result = 1;

            // todo
            result *= 1 - GradientNoise(position / 12f) / 8;
            result *= 1 - GradientNoise(position / 24f) / 4;
            result *= 1 - GradientNoise(position / 48f) / 2;

            // 100 - planet size (todo)
            result *= 1 - Mathf.Clamp01(position.magnitude / 100);

            return result;
        }

        private static float Noise(Vector3 position)
        {
            float noise = (Mathf.Sin(Vector3.Dot(position, new Vector3(12.9898f, 78.233f, 128.544f) * 2.0f) * position.magnitude) * 43758.5453f) % 1;
            return Mathf.Abs(noise);
        }

        private static float GradientNoise(Vector3 position)
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

        public static Cube[] ToCubes(PointsChunk pointsChunk)
        {
            var cubes = new List<Cube>();
            int cubesChunkSize = pointsChunk.Size1D - 1;

            for (int zIndex = 0; zIndex < cubesChunkSize; zIndex++)
            {
                for (int yIndex = 0; yIndex < cubesChunkSize; yIndex++)
                {
                    for (int xIndex = 0; xIndex < cubesChunkSize; xIndex++)
                    {
                        cubes.Add(GenerateCube(pointsChunk.Points, xIndex, yIndex, zIndex));
                    }
                }
            }
            return cubes.ToArray();
        }

        public static Cube GenerateCube(Point[,,] points, int xIndex, int yIndex, int zIndex)
        {
            var pointsList = new List<Point>();

            for (int y = 0; y < 2; y++)
            {
                for (int z = 0; z < 2; z++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        int localXIndex = z == 0 ? x : 1 - x;
                        int localYIndex = y;
                        int localZIndex = 1 - z;
                        pointsList.Add(points[xIndex + localXIndex, yIndex + localYIndex, zIndex + localZIndex]);
                    }
                }
            }
            return new Cube(pointsList.ToArray());
        }
    }
}