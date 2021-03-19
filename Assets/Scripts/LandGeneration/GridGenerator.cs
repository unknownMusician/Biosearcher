using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.LandGeneration
{
    public static class GridGenerator
    {
        public static PointsChunk GeneratePointsChunk(Vector3Int chunkPosition, int size)
        {
            int cubeSize = 1;
            int halfSize = size / 2;

            int pointsArray1DSize = halfSize * 2 + 1;
            var points = new Point[pointsArray1DSize, pointsArray1DSize, pointsArray1DSize];

            for (int z = -halfSize, zIndex = 0; z <= halfSize; z += cubeSize, zIndex++)
            {
                for (int y = -halfSize, yIndex = 0; y <= halfSize; y += cubeSize, yIndex++)
                {
                    for (int x = -halfSize, xIndex = 0; x <= halfSize; x += cubeSize, xIndex++)
                    {
                        Vector3Int position = new Vector3Int(x, y, z);
                        points[xIndex, yIndex, zIndex] = new Point(position, GenerateValue(position + chunkPosition));
                    }
                }
            }
            return new PointsChunk(points, pointsArray1DSize);
        }

        private static float GenerateValue(Vector3 position)
        {
            float result = 1;

            // result *= ScaledNoise(position, 2);
            // result *= ScaledNoise(position, 4);
            // result *= ScaledNoise(position, 8);
            // result *= ScaledNoise(position, 16);
            // result *= ScaledNoise(position, 32);
            // result *= Mathf.Sqrt(Mathf.Sqrt(Mathf.Sqrt(Mathf.Sqrt(ScaledNoise(position, 64)))));
            // result *= ScaledNoise(position, 128);

            result *= 1 - Mathf.Clamp01(position.magnitude / 32);

            // result *= 1 - Mathf.Clamp(position.y / 2, 0, 1);

            return result;
        }

        private static float ScaledNoise(Vector3 vector, float scale)
        {
            Vector3 value = (vector / scale);
            value = new Vector3(value.x % 1, value.y % 1, value.z % 1);
            return Mathf.Clamp01(Perlin.Noise(value));
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

            for (int y = 0; y < 2; y++) // todo
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