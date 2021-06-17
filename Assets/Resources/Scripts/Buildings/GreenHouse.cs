using Biosearcher.Buildings.Resources;
using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Buildings.Settings;
using Biosearcher.Buildings.Types.Interfaces;
using UnityEngine;

namespace Biosearcher.Buildings
{
    public sealed class GreenHouse : Building, IResourceReceiver<Electricity>, IResourceReceiver<Water>
    {
        #region Properties

        [SerializeField] private BuildingsSettings _buildingsSettings;

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

        #region Behaviour methods

        protected override void Awake()
        {
            base.Awake();
            LoadProperties();

            //TODO: logic
            _currentPossibleReceivedElectricity = _maxPossibleReceivedElectricity;
            _currentPossibleReceivedWater = _maxPossibleReceivedWater;
        }

        #endregion

        #region Methods

        private void LoadProperties()
        {
            _electricityNetwork = new Network<Electricity>(this);
            _waterNetwork = new Network<Water>(this);

            var greenHouseSettings = _buildingsSettings.GreenHouseSettings;

            _maxPossibleReceivedElectricity = greenHouseSettings.MaxPossibleReceivedElectricity;
            _maxPossibleReceivedWater = greenHouseSettings.MaxPossibleReceivedWater;
        }

        public void Receive(Electricity resource)
        {
            //TODO: logic
            return;
        }

        public void Receive(Water resource)
        {
            //TODO: logic
            return;
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

        #endregion
    }
}