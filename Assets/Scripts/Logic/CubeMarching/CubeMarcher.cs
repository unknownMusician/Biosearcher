using System;
using System.Collections.Generic;
using AreYouFruits.Common.ComponentGeneration;
using Biosearcher.WorldGeneration;
using UnityEngine;
using UnityEngine.Profiling;
using static Biosearcher.CubeMarching.Utils;
using static Biosearcher.CubeMarching.Constants;

namespace Biosearcher.CubeMarching
{
    [HasComponent(true)]
    public class CubeMarcher : ICubeMarcher
    {
        public int Points1DCount { get; }
        public int Cubes1DCount { get; }
        public float SingleCube1DSize { get; }
        public float CriticalWeight { get; }

        private readonly IProbabilityProvider<Vector3> _pointWeightProvider;

        public CubeMarcher(
            IProbabilityProvider<Vector3> pointWeightProvider, int points1DCount = 8, float singleCube1DSize = 1.0f,
            float criticalWeight = 0.5f
        )
        {
            _pointWeightProvider = pointWeightProvider;
            SingleCube1DSize = singleCube1DSize;
            CriticalWeight = criticalWeight;
            Points1DCount = points1DCount;
            Cubes1DCount = Points1DCount - 1;
        }

        public float[] CreateWeightsBuffer() => new float[Points1DCount * Points1DCount * Points1DCount];
        public Vector3[] CreatePointsBuffer() => new Vector3[Points1DCount * Points1DCount * Points1DCount];
        public Vector3[] CreateVerticesBuffer() => new Vector3[Cubes1DCount * Cubes1DCount * Cubes1DCount * MaxTriangleVerticesInCube];

        protected virtual void GeneratePoints(Vector3 chunkOffset, Span<Vector3> points, Span<float> weights)
        {
            Profiler.BeginSample($"{GetType().Name}.{nameof(GeneratePoints)}");
            
            if (weights.Length != Points1DCount * Points1DCount * Points1DCount)
            {
                Profiler.EndSample();
                
                throw new ArgumentException();
            }

            for (int x = 0; x < Points1DCount; x++)
            {
                for (int y = 0; y < Points1DCount; y++)
                {
                    for (int z = 0; z < Points1DCount; z++)
                    {
                        Vector3 point = SingleCube1DSize * new Vector3(x, y, z);
                        Vector3 globalPoint = point + chunkOffset;

                        int index = ToIndex(Points1DCount, x, y, z);
                        points[index] = point;

                        weights[index] = _pointWeightProvider.Get(ref globalPoint);
                    }
                }
            }
            
            Profiler.EndSample();
        }
        
        protected virtual int GenerateVertices(Span<float> weights, Span<Vector3> vertices)
        {
            Profiler.BeginSample($"{GetType().Name}.{nameof(GenerateVertices)}");

            int verticesCount = 0;
            
            Span<Vector3> points = stackalloc Vector3[PointsInCube];
            Span<float> pointWeights = stackalloc float[PointsInCube];
            Span<Vector3> edges = stackalloc Vector3[EdgesInCube];

            for (int x = 0; x < Cubes1DCount; x++)
            {
                for (int y = 0; y < Cubes1DCount; y++)
                {
                    for (int z = 0; z < Cubes1DCount; z++)
                    {
                        int cubeIndex = 0;

                        for (int i = 0; i < PointsInCube; i++)
                        {
                            Vector3Int pointCubeOffset = PointsCubeOffset[i];

                            var position = new Vector3Int(
                                x + pointCubeOffset.x,
                                y + pointCubeOffset.y,
                                z + pointCubeOffset.z
                            );

                            int pointIndex = ToIndex(Points1DCount, position.x, position.y, position.z);

                            points[i] = SingleCube1DSize * (Vector3)position;

                            float weight = weights[pointIndex];

                            pointWeights[i] = weight;

                            if (weight < CriticalWeight)
                            {
                                cubeIndex |= 1 << i;
                            }
                        }
                        
                        int edgeIndexes = EdgeTable[cubeIndex];

                        for (int i = 0; i < EdgesInCube; i++)
                        {
                            if ((edgeIndexes & (1 << i)) != 0)
                            {
                                int pointIndex1 = EdgeIndexToPositionIndexes[i, 0];
                                int pointIndex2 = EdgeIndexToPositionIndexes[i, 1];

                                Vector3 point1 = points[pointIndex1];
                                Vector3 point2 = points[pointIndex2];

                                float weight1 = pointWeights[pointIndex1];
                                float weight2 = pointWeights[pointIndex2];

                                edges[i] = Interpolate(CriticalWeight, ref point1, ref point2, weight1, weight2);
                            }
                        }
                        
                        for (int i = 0; i < 16; i += 3)
                        {
                            int edgeIndex = TriTable[cubeIndex, i];

                            if (edgeIndex == -1)
                            {
                                break;
                            }

                            vertices[verticesCount++] = edges[edgeIndex];
                            vertices[verticesCount++] = edges[TriTable[cubeIndex, i + 1]];
                            vertices[verticesCount++] = edges[TriTable[cubeIndex, i + 2]];
                        }
                    }
                }
            }
            
            Profiler.EndSample();

            return verticesCount;
        }

        protected virtual Mesh ToMesh(Span<Vector3> verticesBuffer, out Vector3[] vertices)
        {
            Profiler.BeginSample($"{GetType().Name}.{nameof(ToMesh)}");
            
            int[] triangleIndexes = new int[verticesBuffer.Length];

            for (int i = 0; i < verticesBuffer.Length; i++)
            {
                triangleIndexes[i] = i;
            }

            vertices = verticesBuffer.ToArray();
            
            var mesh = new Mesh { vertices = vertices, triangles = triangleIndexes, };
            mesh.RecalculateNormals();

            Profiler.EndSample();
            
            return mesh;
        }

        public virtual bool TryGenerateMesh(
            Vector3 chunkOffset, Span<Vector3> points, Span<float> weights,
            Span<Vector3> verticesBuffer, out Mesh? mesh, out Vector3[] vertices
        )
        {
            Profiler.BeginSample($"{GetType().Name}.{nameof(TryGenerateMesh)}");
            
            GeneratePoints(chunkOffset, points, weights);
            int verticesCount = GenerateVertices(weights, verticesBuffer);

            verticesBuffer = verticesBuffer[..verticesCount];

            if (verticesCount <= 0)
            {
                mesh = null;
                vertices = Array.Empty<Vector3>();
                
                Profiler.EndSample();

                return false;
            }

            mesh = ToMesh(verticesBuffer, out vertices);
                
            Profiler.EndSample();

            return true;
        }
    }
}
