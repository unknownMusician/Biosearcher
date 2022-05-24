using AreYouFruits.Common.ComponentGeneration;
using UnityEngine;

namespace Biosearcher.WorldGeneration
{
    public class StoneSpawnerComponent : AbstractComponent<StoneSpawner>
    {
#nullable disable
        [SerializeField] private Transform _propParent;
        [SerializeField] private GameObject[] _propPrefabs;
        [SerializeField] private float _probability;
        [SerializeField] private float _surfaceAngle = 75.0f;
#nullable enable

        protected override StoneSpawner Create()
        {
            return new StoneSpawner(_propParent, _propPrefabs, _probability, _surfaceAngle);
        }
    }
}
