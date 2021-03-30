using System.Collections;
using UnityEngine;

namespace Biosearcher.Planet.Generation
{
    public struct MarchCube
    {
        public MarchPoint[] Points { get; }

        public MarchCube(MarchPoint[] points) => Points = points;
    }
}