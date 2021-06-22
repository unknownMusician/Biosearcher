//#define NOISE_PROFILING

using UnityEngine;
#if NOISE_PROFILING
using UnityEngine.Profiling;
#endif

namespace Biosearcher.LandManagement.CubeMarching.CPU
{
    internal static class Noise
    {
        internal static float Get(Vector3 position)
        {
#if NOISE_PROFILING
            Profiler.BeginSample("Noise.Get");
#endif
            float noise = Mathf.Abs((Mathf.Sin(Vector3.Dot(position, new Vector3(12.9898f, 78.233f, 128.544f) * 2.0f) * position.magnitude) * 43758.5453f) % 1);
#if NOISE_PROFILING
            Profiler.EndSample();
#endif
            return noise;
        }

        private static float Smooth(float x)
        {
            return x * x * x * (x * (x * 6 - 15) + 10);
        }

        internal static Vector3 Smooth(Vector3 v)
        {
            return new Vector3(Smooth(v.x), Smooth(v.y), Smooth(v.z));
        }

        internal static float Gradient(Vector3 position)
        {
#if NOISE_PROFILING
            Profiler.BeginSample("Noise.Gradient");
#endif
            Vector3Int wholePart = Vector3Int.FloorToInt(position);
            Vector3 fractPart = Smooth(position - wholePart);
            var noisesZ = new float[2];
            var noisesY = new float[2];
            var noisesX = new float[2];
            Vector3Int delta = default;

            for (delta.z = 0; delta.z < 2; delta.z++)
            {
                for (delta.y = 0; delta.y < 2; delta.y++)
                {
                    for (delta.x = 0; delta.x < 2; delta.x++)
                    {
                        noisesX[delta.x] = Get(wholePart + delta);
                    }
                    noisesY[delta.y] = Mathf.Lerp(noisesX[0], noisesX[1], fractPart.x);
                }
                noisesZ[delta.z] = Mathf.Lerp(noisesY[0], noisesY[1], fractPart.y);
            }
            float gradient = Mathf.Lerp(noisesZ[0], noisesZ[1], fractPart.z);

#if NOISE_PROFILING
            Profiler.EndSample();
#endif
            return gradient;
        }

        internal static float ToMountain(float g)
        {
            return g * g * g * g;
        }
    }
}
