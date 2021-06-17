using Biosearcher.Buildings.Settings.Structs;
using UnityEngine;

namespace Biosearcher.Buildings.Settings
{
    [CreateAssetMenu(fileName = "Buildings Settings", menuName = "Buildings Settings", order = 52)]
    public sealed class BuildingsSettings : ScriptableObject
    {
        // todo
        // public static readonly string filePath = "/sdfsdf/sdfsdg/sdfsad/fsad/fa/dg";
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
