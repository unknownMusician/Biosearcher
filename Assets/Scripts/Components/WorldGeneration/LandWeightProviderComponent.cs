using AreYouFruits.Common.ComponentGeneration;
using Biosearcher.CubeMarching;
using UnityEngine;

namespace Biosearcher.WorldGeneration
{
    public class LandWeightProviderComponent : AbstractComponent<LandWeightProvider>
    {
#nullable disable
        [SerializeField] private float _height;
        [SerializeField] private NoiseInfo[] _noiseInfos;
#nullable enable

        protected override LandWeightProvider Create() => new LandWeightProvider(_height, _noiseInfos);
    }
}
