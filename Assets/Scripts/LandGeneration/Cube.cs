using System.Collections;
using UnityEngine;

namespace Biosearcher.LandGeneration
{
    public struct Cube
    {
        public Point[] Points { get; }

        public Cube(Point[] points) => Points = points;
    }
}