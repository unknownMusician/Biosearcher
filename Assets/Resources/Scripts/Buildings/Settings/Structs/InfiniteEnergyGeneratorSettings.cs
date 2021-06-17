using Biosearcher.Buildings.Resources.Structs;
using UnityEngine;

namespace Biosearcher.Buildings.Settings.Structs
{
    [System.Serializable]
    public struct InfiniteEnergyGeneratorSettings
    {
        [SerializeField] private Electricity maxPossibleEnergyProduced;
        [SerializeField] private float neededCoal;

        public Electricity MaxPossibleEnergyProduced => maxPossibleEnergyProduced;
        public float NeededCoal => neededCoal;
    }
}
