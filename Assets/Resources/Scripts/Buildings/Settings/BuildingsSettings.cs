using Biosearcher.Buildings.Settings.Structs;
using UnityEngine;

namespace Biosearcher.Buildings.Settings
{
    [CreateAssetMenu(fileName = "Buildings Settings", menuName = "Buildings Settings", order = 52)]
    public class BuildingsSettings : ScriptableObject
    {
        // public static readonly string filePath = "/sdfsdf/sdfsdg/sdfsad/fsad/fa/dg";

        [SerializeField] private CoalGeneratorSettings coalGeneratorSettings;
        [SerializeField] private GreenHouseSettings greenHouseSettings;
        
        public CoalGeneratorSettings CoalGeneratorSettings => coalGeneratorSettings;
        public GreenHouseSettings GreenHouseSettings => greenHouseSettings;
    }
}
