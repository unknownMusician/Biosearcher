using AreYouFruits.Common.ComponentGeneration;
using Biosearcher.CubeMarching;
using UnityEngine;

namespace Biosearcher.WorldGeneration
{
    [HasComponent(false)]
    public class LandWeightProvider : IProbabilityProvider<Vector3>
    {
        public float Height;
        public NoiseInfo[] NoiseInfos;

        public LandWeightProvider(float height, NoiseInfo[] noiseInfos)
        {
            Height = height;
            NoiseInfos = noiseInfos;
        }

        public float Get(ref Vector3 point)
        {
            float value = Height * 2 - Mathf.Clamp(point.y, 0.0f, Height * 2);
            value /= Height * 2;

            for (int i = 0; i < NoiseInfos.Length; i++)
            {
                NoiseInfo noiseInfo = NoiseInfos[i];

                if (noiseInfo.Strength != 0)
                {
                    float noise = (Noise.SimpleGradient(point.x, point.z, noiseInfo.Scale) * 2 - 1);
                    value += noise * noiseInfo.Strength;
                }
            }

            return Mathf.Clamp01(value);
        }
    }
}
