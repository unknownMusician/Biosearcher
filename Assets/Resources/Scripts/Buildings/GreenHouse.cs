using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Buildings.Settings;
using Biosearcher.Buildings.Settings.Structs;
using Biosearcher.Buildings.Types.Interfaces;
using Biosearcher.Plants;
using UnityEngine;

namespace Biosearcher.Buildings
{
    public class GreenHouse : MonoBehaviour, IResourceReceiver<Electricity>, IResourceReceiver<Water>
    {
        [SerializeField] private Seed testSeed1;
        [SerializeField] private Seed testSeed2;
        [SerializeField] private Seed testSeed3;

        #region Properties

        [SerializeField] private BuildingsSettings buildingsSettings;
        [SerializeField] private Slot[] slots;

        private GreenHouseSettings _settings;

        private Electricity _maxPossibleReceivedElectricity;
        private Electricity _currentPossibleReceivedElectricity;
        private Water _maxPossibleReceivedWater;
        private Water _currentPossibleReceivedWater;

        Electricity IResourceReceiver<Electricity>.MaxPossibleReceived => _maxPossibleReceivedElectricity;
        Electricity IResourceReceiver<Electricity>.CurrentPossibleReceived => _currentPossibleReceivedElectricity;
        Water IResourceReceiver<Water>.MaxPossibleReceived => _maxPossibleReceivedWater;
        Water IResourceReceiver<Water>.CurrentPossibleReceived => _currentPossibleReceivedWater;

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            LoadSettings();

            _maxPossibleReceivedElectricity = _settings.MaxPossibleReceivedElectricity;
            _maxPossibleReceivedWater = _settings.MaxPossibleReceivedWater;
        }
        private void Start()
        {
            RecalculateNeededElectricity();
            RecalculateNeededWater();
        }

        #endregion

        #region Methods

        private void LoadSettings()
        {
            _settings = buildingsSettings.GreenHouseSettings;
        }

        private void RecalculateNeededElectricity()
        {
            _currentPossibleReceivedElectricity = default;
            foreach (var slot in slots)
            {
                if (slot == null) continue;

                _currentPossibleReceivedElectricity += slot.NeededElectricity;
            }
        }
        private void RecalculateNeededWater()
        {
            _currentPossibleReceivedWater = default;
            foreach (var slot in slots)
            {
                if (slot == null) continue;

                _currentPossibleReceivedWater += slot.NeededWater;
            }
        }

        public void Test1()
        {
            Plant(testSeed1, 0);
        }
        public void Test2()
        {
            Plant(testSeed2, 1);
        }
        public void Test3()
        {
            Plant(testSeed3, 2);
        }

        public void Plant(Seed seed, int slotNumber)
        {
            var position = Vector3.zero;
            var rotation = Quaternion.identity;
            var parent = slots[slotNumber].transform;
            var plant = seed.Plant(position, rotation, parent);
            slots[slotNumber].Plant = plant;
            RecalculateNeededElectricity();
            RecalculateNeededWater();
            Destroy(seed.gameObject);
        }
        public void ChangeSlot(Slot slot, int slotNumber)
        {
            slots[slotNumber] = slot;
            RecalculateNeededElectricity();
            RecalculateNeededWater();
        }

        public void Receive(Electricity resource)
        {
            var efficiency = (resource / _currentPossibleReceivedElectricity).Value;
            foreach (var slot in slots)
            {
                if (slot == null) continue;
                
                slot.RegulateIllumination(efficiency);
                slot.RegulateTemperature(efficiency);
            }
        }
        public void Receive(Water resource)
        {
            var efficiency = (resource / _currentPossibleReceivedWater).Value;
            foreach (var slot in slots)
            {
                if (slot == null) continue;
                
                slot.RegulateHumidity(efficiency);
            }
        }

        #endregion
    }
}
