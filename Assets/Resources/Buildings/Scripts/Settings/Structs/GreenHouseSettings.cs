using Biosearcher.Buildings.Resources.Structs;
using UnityEngine;

namespace Biosearcher.Buildings.Settings.Structs
{
    [System.Serializable]
    public struct GreenHouseSettings
    {
        [SerializeField] private Electricity _maxPossibleReceivedElectricity;
        [SerializeField] private Water _maxPossibleReceivedWater;
        [SerializeField] private int _amountOfSlots;

        public Electricity MaxPossibleReceivedElectricity => _maxPossibleReceivedElectricity;
        public Water MaxPossibleReceivedWater => _maxPossibleReceivedWater;
        public int AmountOfSlots => _amountOfSlots;
    }
}