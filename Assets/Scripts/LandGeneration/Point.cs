using System.Collections;
using UnityEngine;

namespace Biosearcher.LandGeneration
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
}