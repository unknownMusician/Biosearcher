using System;
using UnityEngine;

namespace Biosearcher.WorldGeneration
{
    public struct PropSpawnerInfo
    {
        public Vector3[] Vertices;
        public Vector3 Offset;

        public readonly void Deconstruct(out Vector3[] vertices, out Vector3 offset)
        {
            (vertices, offset) = (Vertices, Offset);
        }
    }
}
