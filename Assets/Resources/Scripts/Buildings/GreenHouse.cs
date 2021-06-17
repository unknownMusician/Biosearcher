using Biosearcher.Buildings.Resources;
using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Buildings.Settings;
using Biosearcher.Buildings.Types.Interfaces;
using Biosearcher.Plants;
using UnityEngine;

namespace Biosearcher.Buildings
{
    // refactor: make 2 classes (classic abstract GreenHouse class and actual ElectricityGreenHouse class)
    public sealed class GreenHouse : Building, IResourceReceiver<Electricity>, IResourceReceiver<Water>
    {
        #region Properties

        [SerializeField] private Slot[] slots;

        private Electricity _maxPossibleReceivedElectricity;
        private Electricity _currentPossibleReceivedElectricity;
        private Water _maxPossibleReceivedWater;
        private Water _currentPossibleReceivedWater;

        private Network<Electricity> _electricityNetwork;
        private Network<Water> _waterNetwork;

        Electricity IResourceReceiver<Electricity>.MaxPossibleReceived => _maxPossibleReceivedElectricity;
        Electricity IResourceReceiver<Electricity>.CurrentPossibleReceived => _currentPossibleReceivedElectricity;
        Water IResourceReceiver<Water>.MaxPossibleReceived => _maxPossibleReceivedWater;
        Water IResourceReceiver<Water>.CurrentPossibleReceived => _currentPossibleReceivedWater;

        Network<Electricity> IResourceMover<Electricity>.Network
        {
            get => _electricityNetwork;
            set => _electricityNetwork = value;
        }
        Network<Water> IResourceMover<Water>.Network
        {
            get => _waterNetwork;
            set => _waterNetwork = value;
        }

        #endregion
        private void Start()
        {
            RecalculateNeededElectricity();
            RecalculateNeededWater();
        }

        #region Methods

        protected override void LoadBuildingParameters(BuildingsSettings buildingsSettings)
        {
            base.LoadBuildingParameters(buildingsSettings);

            var greenHouseSettings = buildingsSettings.GreenHouseSettings;

            _currentPossibleReceivedElectricity = _maxPossibleReceivedElectricity = greenHouseSettings.MaxPossibleReceivedElectricity;
            _currentPossibleReceivedWater = _maxPossibleReceivedWater = greenHouseSettings.MaxPossibleReceivedWater;

            _electricityNetwork = new Network<Electricity>(this, _cyclesPerSecond);
            _waterNetwork = new Network<Water>(this, _cyclesPerSecond);
        }

        protected override void TryConnect()
        {
            TryConnect<Electricity>(this);
            TryConnect<Water>(this);
        }
        protected override void TryDisconnect()
        {
            TryDisconnect<Electricity>(this);
            TryDisconnect<Water>(this);
        }

        private void RecalculateNeededElectricity()
        {
            _currentPossibleReceivedElectricity = default;
            foreach (var slot in slots)
            {
                if (slot != null)
                {
                    _currentPossibleReceivedElectricity += slot.NeededElectricity;
                }
            }
        }
        private void RecalculateNeededWater()
        {
            _currentPossibleReceivedWater = default;
            foreach (var slot in slots)
            {
                if (slot != null)
                {
                    _currentPossibleReceivedWater += slot.NeededWater;
                }
            }
        }

        public void Plant(Seed seed, int slotNumber)
        {
            // todo
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
            var efficiency = (resource / _currentPossibleReceivedElectricity);
            foreach (var slot in slots)
            {
                if (slot != null)
                {
                    slot.RegulateIllumination(efficiency);
                    slot.RegulateTemperature(efficiency);
                }
            }
        }
        public void Receive(Water resource)
        {
            var efficiency = (resource / _currentPossibleReceivedWater);
            foreach (var slot in slots)
            {
                if (slot != null)
                {
                    slot.RegulateHumidity(efficiency);
                }
            }
        }

        #endregion
    }
}