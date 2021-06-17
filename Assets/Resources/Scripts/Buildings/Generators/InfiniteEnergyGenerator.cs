using Biosearcher.Buildings.Resources;
using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Buildings.Settings;
using Biosearcher.Buildings.Types.Interfaces;
using UnityEngine;

namespace Biosearcher.Buildings.Generators
{
    // todo: remove
    public class InfiniteEnergyGenerator : Building, IResourceProducer<Electricity>
    {
        #region Properties

        [SerializeField] protected BuildingsSettings _buildingsSettings;

        protected Network<Electricity> _electricityNetwork;

        public Electricity MaxPossibleProduced { get; protected set; }
        public Electricity CurrentPossibleProduced => MaxPossibleProduced;

        Network<Electricity> IResourceMover<Electricity>.Network
        {
            get => _electricityNetwork;
            set => _electricityNetwork = value;
        }

        #endregion
        #region Behaviour methods

        protected override void Awake()
        {
            base.Awake();
            LoadProperties();
        }

        #endregion

        #region Methods

        protected void LoadProperties()
        {
            _electricityNetwork = new Network<Electricity>(this);

            MaxPossibleProduced = _buildingsSettings.InfiniteEnergyGeneratorSettings.MaxPossibleEnergyProduced;
        }

        public Electricity Produce() => CurrentPossibleProduced;

        protected override void TryConnect() => TryConnect(this);
        protected override void TryDisconnect() => TryDisconnect(this);

        #endregion
    }
}