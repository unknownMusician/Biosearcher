using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.LandGeneration
{
    public static class GridGenerator
    {
        public static Cube[] Generate(Vector3 size, float squareSize)
        {
            Vector3 bounds = GetBounds(size, squareSize);
            return GenerateCubes(bounds, squareSize);
        }

        private static Vector3 GetBounds(Vector3 size, float squareSize)
        {
            Vector3 bounds;
            bounds.x = size.x - size.x % squareSize;
            bounds.y = size.y - size.y % squareSize;
            bounds.z = size.z - size.z % squareSize;
            return bounds;
        }

        private static Cube[] GenerateCubes(Vector3 bounds, float cubeSize)
        {
            var cubes = new List<Cube>();

            for (float x = -bounds.x; x < bounds.x; x += cubeSize)
            {
                for (float y = -bounds.y; y < bounds.y; y += cubeSize)
                {
                    for (float z = -bounds.z; z < bounds.z; z += cubeSize)
                    {
                        Vector3 position = new Vector3(x, y, z);
                        cubes.Add(GenerateCube(position, cubeSize, bounds));
                    }
                }
            }
            return cubes.ToArray();
        }

        private static Cube GenerateCube(Vector3 startPoint, float cubeSize, Vector3 bounds)
        {
            var pointsList = new List<Point>();

            for (int y = 0; y < 2; y++)
            {
                for (int z = 0; z < 2; z++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        Vector3 point = new Vector3(startPoint.x + (z == 0 ? x : 1 - x) * cubeSize, startPoint.y + (1 - y) * cubeSize, startPoint.z + z * cubeSize);
                        pointsList.Add(new Point(point, GenerateValue(point, bounds)));
                    }
                }
            }
            return new Cube(pointsList.ToArray());
        }

        private static float GenerateValue(Vector3 position, Vector3 bounds)
        {
            Vector3 value = (position + bounds).DivideBy(bounds);

            float result = Perlin.Noise(value);
            result *= 1 - Mathf.Clamp(position.y/10, 0, 1);

            return result;
        }
    }
}