//#define GENERATOR_PROFILING

using UnityEngine;
#if GENERATOR_PROFILING
using UnityEngine.Profiling;
#endif

namespace Biosearcher.LandManagement.CubeMarching.CPU
{
    internal abstract class Generator
    {
        protected ConstantBuffer _constantInputOutput;
        protected Common _common;

        protected internal Generator(ref ConstantBuffer constantInputOutput)
        {
            _constantInputOutput = constantInputOutput;
            _common = new Common(ref _constantInputOutput);
        }
        protected internal void GeneratePoints(ref TempPointsBuffer tempBuffer)
        {
#if GENERATOR_PROFILING
            Profiler.BeginSample("GeneratePoints");
#endif
            int x, y, z;
            int pointsPerChunk = _constantInputOutput.pointsPerChunk;
            for (z = 0; z < pointsPerChunk; z++)
            {
                for (y = 0; y < pointsPerChunk; y++)
                {
                    for (x = 0; x < pointsPerChunk; x++)
                    {
                        GeneratePoint(new Vector3Int(x, y, z), ref tempBuffer);
                    }
                }
            }
#if GENERATOR_PROFILING
            Profiler.EndSample();
#endif
        }
        protected void GeneratePoint(Vector3Int threadId, ref TempPointsBuffer tempBuffer)
        {
#if GENERATOR_PROFILING
            Profiler.BeginSample("GenerateSinglePoint");
#endif
            if (threadId.x >= _constantInputOutput.pointsPerChunk || threadId.y >= _constantInputOutput.pointsPerChunk || threadId.z >= _constantInputOutput.pointsPerChunk)
            {
#if GENERATOR_PROFILING
                Profiler.EndSample();
#endif
                return;
            }

            Vector3Int position = (threadId - Vector3Int.one * (_constantInputOutput.pointsPerChunk - 1) / 2) * tempBuffer.cubeSize;

            // todo
            tempBuffer.points[_common.MatrixId2ArrayId(threadId, _constantInputOutput.pointsPerChunk)] =
                new MarchPoint
                {
                    position = position,
                    value = GenerateValue(position + tempBuffer.chunkPosition)
                };
#if GENERATOR_PROFILING
            Profiler.EndSample();
#endif
        }
        protected abstract float GenerateValue(Vector3 position);

        protected internal void GenerateMesh(ref TempMeshBuffer tempBuffer)
        {
#if GENERATOR_PROFILING
            Profiler.BeginSample("GenerateMesh");
#endif
            int x, y, z;
            int cubesPerChunk = _constantInputOutput.cubesPerChunk;
            tempBuffer.counter = 0;
            for (z = 0; z < cubesPerChunk; z++)
            {
                for (y = 0; y < cubesPerChunk; y++)
                {
                    for (x = 0; x < cubesPerChunk; x++)
                    {
                        GenerateTriangles(new Vector3Int(x, y, z), ref tempBuffer);
                    }
                }
            }
#if GENERATOR_PROFILING
            Profiler.EndSample();
#endif
        }
        protected internal void GenerateTriangles(Vector3Int threadId, ref TempMeshBuffer tempBuffer)
        {
            if (threadId.x >= _constantInputOutput.cubesPerChunk || threadId.y >= _constantInputOutput.cubesPerChunk || threadId.z >= _constantInputOutput.cubesPerChunk)
            {
                return;
            }

            _common.March(
                _common.GenerateCube(tempBuffer.points, threadId),
                _constantInputOutput.surfaceValue,
                _common.MatrixId2ArrayId(threadId, _constantInputOutput.cubesPerChunk), 
                ref tempBuffer);
        }

    }

    internal sealed class PlanetGenerator : Generator
    {
        internal PlanetGenerator(ref ConstantBuffer constantInputOutput) : base(ref constantInputOutput) { }
        protected override float GenerateValue(Vector3 position)
        {
#if GENERATOR_PROFILING
            Profiler.BeginSample("GenerateValue");
#endif
            float result = 1;
            Vector3 normalizedPosition = position.normalized;

            // 400 - planet size (todo)
            float planetRadius = position.magnitude / 400;
            //float planetRadius = length(position*2) - 400;

            //planetRadius += GradientNoise(normalizedPosition / (cubesPerChunk * (1 << 5))) / 1.5f;

            // Mountains
            float preMountainNoise = 1;
            preMountainNoise *= Noise.Gradient(normalizedPosition * (_constantInputOutput.cubesPerChunk * (1 << 2)));
            preMountainNoise *= Noise.Gradient(normalizedPosition * (_constantInputOutput.cubesPerChunk * (1 << 3)));
            float mountainMask = Mathf.SmoothStep(0.3f, 0.1f, Noise.Gradient(normalizedPosition * (_constantInputOutput.cubesPerChunk / (1 << 1))));
            planetRadius -= (Noise.ToMountain(preMountainNoise) + 0.5f) * mountainMask * 0.15f;

            // Hills
            planetRadius -= Noise.Gradient(normalizedPosition * (_constantInputOutput.cubesPerChunk * (1 << 0))) * (1 - mountainMask) * 0.02f;
            // todo
            //result *= 1 - GradientNoise(position / (cubesPerChunk * (1 << 1))) / 16;
            //result *= 1 - GradientNoise(position / (cubesPerChunk * (1 << 2))) / 8;
            //result *= 1 - GradientNoise(position / (cubesPerChunk * (1 << 3))) / 4;
            //result *= 1 - GradientNoise(position / (cubesPerChunk * (1 << 4))) / 2;
            //result *= 1 - GradientNoise(position / (cubesPerChunk * (1 << 5))) / 1.5f;

            result *= 1 - Mathf.Clamp01(planetRadius);

#if GENERATOR_PROFILING
            Profiler.EndSample();
#endif
            return result;
        }
    }

    internal sealed class FlatGenerator : Generator
    {
        internal FlatGenerator(ref ConstantBuffer constantInputOutput) : base(ref constantInputOutput) { }
        protected override float GenerateValue(Vector3 position)
        {
#if GENERATOR_PROFILING
            Profiler.BeginSample("GenerateValue");
#endif
            float result = 1;
            Vector3 normalizedPosition = new Vector3(position.x, 0, position.z);

            // 400 - planet size (todo)
            float planetRadius = position.y / 400;

            // Mountains
            float preMountainNoise = 1;
            preMountainNoise *= 1 - Noise.Gradient(normalizedPosition * _constantInputOutput.cubesPerChunk * 0.005f);
            preMountainNoise *= 1 - Noise.Gradient(normalizedPosition * _constantInputOutput.cubesPerChunk * 0.01f) * 0.5f;
            //preMountainNoise *= 1 - GradientNoise(normalizedPosition * cubesPerChunk * 0.02) * 0.2;
            //preMountainNoise *= 1 - GradientNoise(normalizedPosition * cubesPerChunk * 0.05) * 0.1;
            float preMountainMask = 1;
            preMountainMask *= 1 - Noise.Gradient(normalizedPosition * _constantInputOutput.cubesPerChunk * 0.0005f);
            preMountainMask *= 1 - Noise.Gradient(normalizedPosition * _constantInputOutput.cubesPerChunk * 0.0008f);
            preMountainMask *= 1 - Noise.Gradient(normalizedPosition * _constantInputOutput.cubesPerChunk * 0.001f) * 0.75f;
            preMountainMask *= 1 - Noise.Gradient(normalizedPosition * _constantInputOutput.cubesPerChunk * 0.002f) * 0.5f;
            float mountainMask = Mathf.SmoothStep(0.1f, 0.8f, preMountainMask);
            //float mountainMask = GradientNoise(normalizedPosition * cubesPerChunk * 0.0005);
            //planetRadius -= (Noise2Mountain(preMountainNoise) + 0.5) * mountainMask * 0.3;
            planetRadius -= (preMountainNoise + 0.5f) * mountainMask * 0.3f;
            //planetRadius -= preMountainNoise * mountainMask * 0.2;

            // Hills
            planetRadius -= Noise.Gradient(position * _constantInputOutput.cubesPerChunk * 0.02f) /* * (1 - mountainMask)*/ * 0.002f;
            planetRadius -= Noise.Gradient(position * _constantInputOutput.cubesPerChunk * 0.01f) /* * (1 - mountainMask)*/ * 0.005f;
            planetRadius -= Noise.Gradient(position * _constantInputOutput.cubesPerChunk * 0.005f) /* * (1 - mountainMask)*/ * 0.01f;

            result *= 1 - Mathf.Clamp01(planetRadius);

#if GENERATOR_PROFILING
            Profiler.EndSample();
#endif
            return result;
        }
    }
}