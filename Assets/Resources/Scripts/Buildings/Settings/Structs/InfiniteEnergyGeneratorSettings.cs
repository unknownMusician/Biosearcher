using Biosearcher.Buildings.Resources.Structs;
using UnityEngine;

namespace Biosearcher.Buildings.Settings.Structs
{
    [System.Serializable]
    public struct InfiniteEnergyGeneratorSettings
    {
        [SerializeField] private Electricity _maxPossibleEnergyProduced;
        [SerializeField] private float _neededCoal;

        public Electricity MaxPossibleEnergyProduced => _maxPossibleEnergyProduced;
        public float NeededCoal => _neededCoal;
    }
}
