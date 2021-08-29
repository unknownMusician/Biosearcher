using Biosearcher.Buildings.Settings.Structs;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Buildings.Settings
{
    [CreateAssetMenu(fileName = "Buildings Settings", menuName = "Buildings Settings", order = 52)]
    public sealed class BuildingsSettings : ScriptableObject
    {
        [NeedsRefactor(Needs.Implementation)]
        public const string filePath = "/sdfsdf/sdfsdg/sdfsad/fsad/fa/dg";
        [Header("Common")]
        [SerializeField] private NetworkSettings _networkSettings;
        [Header("Buildings")]
        [SerializeField] private InfiniteEnergyGeneratorSettings _infiniteEnergyGeneratorSettings;
        [SerializeField] private GreenHouseSettings _greenHouseSettings;
        
        public NetworkSettings NetworkSettings => _networkSettings;
        public InfiniteEnergyGeneratorSettings InfiniteEnergyGeneratorSettings => _infiniteEnergyGeneratorSettings;
        public GreenHouseSettings GreenHouseSettings => _greenHouseSettings;
    }
}
