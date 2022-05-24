using System;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.CubeMarching
{
    public interface ICubeMarcher
    {
        public int Points1DCount { get; }
        public int Cubes1DCount { get; }
        public float SingleCube1DSize { get; }
        public float CriticalWeight { get; }

        public Vector3[] CreatePointsBuffer();
        public float[] CreateWeightsBuffer();
        public Vector3[] CreateVerticesBuffer();

        public bool TryGenerateMesh(
            Vector3 chunkOffset, Span<Vector3> points, Span<float> weights, Span<Vector3> verticesBuffer,
            out Mesh? mesh, out Vector3[] vertices
        );
    }
}
