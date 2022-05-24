using AreYouFruits.Common.ComponentGeneration;
using Biosearcher.CubeMarching;
using UnityEngine;

namespace Biosearcher.WorldGeneration
{
    public class LandGeneratorComponent : AbstractComponent<LandGenerator>
    {
#nullable disable
        [SerializeField] private SerializedInterface<IComponent<ICubeMarcher>> _cubeMarcher;
        [SerializeField] private SerializedInterface<IComponent<IChunkGenerator>> _chunkGenerator;
        [SerializeField] private Transform _parent;
        [SerializeField] private Vector3Int _chunkCount;
#nullable enable

        protected override LandGenerator Create()
        {
            var item = new LandGenerator(
                _cubeMarcher.GetHeldItem(),
                _chunkGenerator.GetHeldItem(),
                _parent,
                _chunkCount
            );
            
            item.Generate();

            return item;
        }
    }
}
