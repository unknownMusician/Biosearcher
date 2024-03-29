﻿using Biosearcher.Buildings.Resources;
using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Buildings.Settings;
using Biosearcher.Buildings.Types.Interfaces;
using Biosearcher.Plants;

namespace Biosearcher.Buildings.GreenHouses
{
    public class ElectroWaterGreenHouse : GreenHouse, IResourceReceiver<Electricity>, IResourceReceiver<Water>
    {
        #region Properties

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

        protected override void PreRecalculateNeededResourcesForAllCapsules()
        {
            _currentPossibleReceivedElectricity = default;
            _currentPossibleReceivedWater = default;
        }

        protected override void RecalculateNeededResources(Capsule capsule)
        {
            _currentPossibleReceivedElectricity += capsule.NeededElectricity;
            _currentPossibleReceivedWater += capsule.NeededWater;
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

        public void Receive(Electricity resource)
        {
            var efficiency = resource / _currentPossibleReceivedElectricity;
            foreach (Capsule capsule in _capsules)
            {
                if (capsule != null)
                {
                    capsule.RegulateIllumination(efficiency);
                    capsule.RegulateTemperature(efficiency);
                }
            }
        }
        public void Receive(Water resource)
        {
            var efficiency = resource / _currentPossibleReceivedWater;
            foreach (Capsule capsule in _capsules)
            {
                if (capsule != null)
                {
                    capsule.RegulateHumidity(efficiency);
                }
            }
        }

        #endregion
    }
}
