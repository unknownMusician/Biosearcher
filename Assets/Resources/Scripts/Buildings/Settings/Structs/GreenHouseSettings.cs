using Biosearcher.Buildings.Resources.Structs;
using UnityEngine;

namespace Biosearcher.Buildings.Settings.Structs
{
    [System.Serializable]
    public struct GreenHouseSettings
    {
        [SerializeField] private Electricity maxPossibleReceivedElectricity;
        [SerializeField] private Water maxPossibleReceivedWater;
        [SerializeField] private int amountOfSlots;

        public Electricity MaxPossibleReceivedElectricity => maxPossibleReceivedElectricity;
        public Water MaxPossibleReceivedWater => maxPossibleReceivedWater;
        public int AmountOfSlots => amountOfSlots;
    }
}