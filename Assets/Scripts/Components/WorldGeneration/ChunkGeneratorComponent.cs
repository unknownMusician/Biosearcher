using System.Linq;
using AreYouFruits.Common.ComponentGeneration;
using Biosearcher.CubeMarching;
using UnityEngine;

namespace Biosearcher.WorldGeneration
{
    public class ChunkGeneratorComponent : AbstractComponent<ChunkGenerator>
    {
#nullable disable
        [SerializeField] private GameObject _chunkPrefab;
        [SerializeField] private Transform _chunkParent;
        [SerializeField] private SerializedInterface<IComponent<ICubeMarcher>> _cubeMarcher;
        [SerializeField]
        private SerializedInterface<IComponent<IPropSpawner<PropSpawnerInfo, GameObject>>>[] _propSpawners;
#nullable enable

        protected override ChunkGenerator Create()
        {
            return new ChunkGenerator(
                _chunkPrefab,
                _chunkParent,
                _cubeMarcher.GetHeldItem(),
                _propSpawners.Select(spawner => spawner.GetHeldItem())
            );
        }
    }
}
