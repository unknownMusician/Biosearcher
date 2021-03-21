using System.Collections;
using UnityEngine;

namespace Biosearcher.Land.Generation
{
    public struct Point
    {
        public Vector3 Position { get; }
        public float Value { get; }

        public Point(Vector3 position, float value)
        {
            Position = position;
            Value = value;
        }
    }

    public class PointsChunk
    {
        public int Size1D { get; }
        public Point[,,] Points { get; }

        public PointsChunk(Point[,,] points, int size1D)
        {
            Points = points;
            Size1D = size1D;
        }
    }
}