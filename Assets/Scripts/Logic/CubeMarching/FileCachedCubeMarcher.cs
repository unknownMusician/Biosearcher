using System;
using System.Collections.Generic;
using System.IO;
using AreYouFruits.Common.ComponentGeneration;
using Biosearcher.WorldGeneration;
using UnityEngine;
using UnityEngine.Profiling;

namespace Biosearcher.CubeMarching
{
    [HasComponent(false)]
    public class FileCachedCubeMarcher : CubeMarcher
    {
        const int BytesInFloat = sizeof(float);
        const int BytesInVertex = BytesInFloat * 3;
        const int BytesInPoint = BytesInVertex + BytesInFloat;

        public const string FolderName = "LandData";
        
        public FileCachedCubeMarcher(
            IProbabilityProvider<Vector3> pointWeightProvider, int points1DCount = 8, float singleCube1DSize = 1,
            float criticalWeight = 0.5f
        ) : base(pointWeightProvider, points1DCount, singleCube1DSize, criticalWeight) { }

        private void DeserializeVertex(Span<byte> inputData, out Vector3 vertex)
        {
            vertex = new Vector3
            {
                x = BitConverter.ToSingle(inputData[..BytesInFloat]),
                y = BitConverter.ToSingle(inputData[BytesInFloat..(BytesInFloat * 2)]),
                z = BitConverter.ToSingle(inputData[(BytesInFloat * 2)..(BytesInFloat * 3)]),
            };
        }
        
        private void DeserializePoint(Span<byte> inputData, out Vector3 point, out float weight)
        {
            DeserializeVertex(inputData[..(BytesInFloat * 3)], out point);

            weight = BitConverter.ToSingle(inputData[(BytesInFloat * 3)..]);
        }

        private void SerializeVertex(Span<byte> outputData, ref Vector3 point)
        {
            BitConverter.TryWriteBytes(outputData[..BytesInFloat], point.x);
            BitConverter.TryWriteBytes(outputData[BytesInFloat..(BytesInFloat * 2)], point.y);
            BitConverter.TryWriteBytes(outputData[(BytesInFloat * 2)..(BytesInFloat * 3)], point.z);
        }

        private void SerializePoint(Span<byte> outputData, ref Vector3 point, float weight)
        {
            SerializeVertex(outputData[..(BytesInFloat * 3)], ref point);
            BitConverter.TryWriteBytes(outputData[(BytesInFloat * 3)..], weight);
        }

        private void DeserializePoints(string filePath, Span<Vector3> points, Span<float> weights)
        {
            byte[] bytes = File.ReadAllBytes(filePath);
                
            int pointsCount = bytes.Length / BytesInPoint;

            for (int i = 0; i < pointsCount; i++)
            {
                int startIndex = i * BytesInPoint;

                DeserializePoint(
                    ((Span<byte>)bytes).Slice(startIndex, BytesInPoint),
                    out Vector3 vertex,
                    out float weight
                );

                points[i] = vertex;
                weights[i] = weight;
            }
        }

        private void SerializePoints(string filePath, string folderPath, Span<Vector3> points, Span<float> weights)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using FileStream file = File.Create(filePath);

            Span<byte> bytes = stackalloc byte[BytesInPoint];
                
            for (int i = 0; i < points.Length; i++)
            {
                SerializePoint(bytes, ref points[i], weights[i]);
                file.Write(bytes);
            }
        }
        
        private int DeserializeVertices(string filePath, Span<Vector3> vertices)
        {
            using FileStream file = File.OpenRead(filePath);

            Span<byte> buffer = stackalloc byte[BytesInVertex];

            int bytesRead;

            int verticesCount = 0;
            
            while ((bytesRead = file.Read(buffer)) > 0)
            {
                if (bytesRead < BytesInVertex)
                {
                    throw new Exception("Incorrect file data.");
                }

                DeserializeVertex(buffer, out Vector3 vertex);
                
                vertices[verticesCount++] = vertex;
            }

            return verticesCount;
        }

        private void SerializeVertices(string filePath, string folderPath, Span<Vector3> vertices)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using FileStream file = File.Create(filePath);

            Span<byte> bytes = stackalloc byte[BytesInVertex];
                
            for (int i = 0; i < vertices.Length; i++)
            {
                SerializeVertex(bytes, ref vertices[i]);
                file.Write(bytes);
            }
        }

        protected override void GeneratePoints(Vector3 chunkOffset, Span<Vector3> points, Span<float> weights)
        {
            Profiler.BeginSample($"{GetType().Name}.{nameof(GeneratePoints)}");

            string applicationPath = Path.Combine(Application.dataPath, "..");
            string folderPath = Path.Combine(applicationPath, FolderName);
            string filePath = Path.Combine(folderPath, $"points_{chunkOffset.x}_{chunkOffset.y}_{chunkOffset.z}.bin");

            if (File.Exists(filePath))
            {
                DeserializePoints(filePath, points, weights);
            }
            else
            {
                base.GeneratePoints(chunkOffset, points, weights);
                
                SerializePoints(filePath, folderPath, points, weights);
            }
            
            Profiler.EndSample();
        }

        protected int GenerateVertices(
            Vector3 chunkOffset, Span<Vector3> points, Span<float> weights, Span<Vector3> vertices
        )
        {
            Profiler.BeginSample($"{GetType().Name}.{nameof(GenerateVertices)}");

            string applicationPath = Path.Combine(Application.dataPath, "..");
            string folderPath = Path.Combine(applicationPath, FolderName);
            string filePath = Path.Combine(folderPath, $"triangles_{chunkOffset.x}_{chunkOffset.y}_{chunkOffset.z}.bin");

            int verticesCount = 0;
            
            if (File.Exists(filePath))
            {
                verticesCount = DeserializeVertices(filePath, vertices);
            }
            else
            {
                base.GeneratePoints(chunkOffset, points, weights);
                verticesCount = GenerateVertices(weights, vertices);
                
                SerializeVertices(filePath, folderPath, vertices[..verticesCount]);
            }
            
            Profiler.EndSample();

            return verticesCount;
        }

        public override bool TryGenerateMesh(
            Vector3 chunkOffset, Span<Vector3> points, Span<float> weights, Span<Vector3> verticesBuffer,
            out Mesh? mesh, out Vector3[] vertices
        )
        {
            Profiler.BeginSample($"{GetType().Name}.{nameof(TryGenerateMesh)}");
            
            int verticesCount = GenerateVertices(chunkOffset, points, weights, verticesBuffer);

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
