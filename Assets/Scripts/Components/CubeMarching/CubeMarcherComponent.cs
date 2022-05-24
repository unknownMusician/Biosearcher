using AreYouFruits.Common.ComponentGeneration;
using Biosearcher.WorldGeneration;
using UnityEngine;

namespace Biosearcher.CubeMarching
{
    public sealed class CubeMarcherComponent : AbstractComponent<CubeMarcher>
    {
#nullable disable
        [SerializeField]
        private SerializedInterface<IComponent<IProbabilityProvider<Vector3>>> _pointWeightProvider;
        [SerializeField] private int _points1DCount = 8;
        [SerializeField] private float _singleCube1DSize = 1.0f;
        [SerializeField] private float _criticalWeight = 0.5f;
#nullable enable

        protected override CubeMarcher Create()
        {
            return new CubeMarcher(
                _pointWeightProvider.GetHeldItem(),
                _points1DCount,
                _singleCube1DSize,
                _criticalWeight
            );
        }
    }
}
