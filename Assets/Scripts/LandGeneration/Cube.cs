using System.Collections;
using UnityEngine;

namespace Biosearcher.LandGeneration
{
    public struct Cube
    {
        public Point[] Points { get; }

        public Cube(Point[] points) => Points = points;
    }

    public class CubesChunk // todo: should exist?
    {
        public int Size1D { get; }
        public Cube[,,] Cubes { get; }

        public CubesChunk(Cube[,,] cubes, int size1D)
        {
            Cubes = cubes;
            Size1D = size1D;
        }
    }
}