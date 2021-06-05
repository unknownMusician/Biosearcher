using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Buildings.Settings;
using Biosearcher.Buildings.Types.Interfaces;
using UnityEngine;

namespace Biosearcher.Buildings
{
    public class GreenHouse : MonoBehaviour, IResourceReceiver<Electricity>, IResourceReceiver<Water>
    {
        #region Properties

        [SerializeField] private BuildingsSettings buildingsSettings;

        private Electricity maxPossibleReceivedElectricity;
        private Electricity currentPossibleReceivedElectricity;
        private Water maxPossibleReceivedWater;
        private Water currentPossibleReceivedWater;

        Electricity IResourceReceiver<Electricity>.MaxPossibleReceived => maxPossibleReceivedElectricity;
        Electricity IResourceReceiver<Electricity>.CurrentPossibleReceived => currentPossibleReceivedElectricity;
        Water IResourceReceiver<Water>.MaxPossibleReceived => maxPossibleReceivedWater;
        Water IResourceReceiver<Water>.CurrentPossibleReceived => currentPossibleReceivedWater;

        #endregion

        #region Behaviour methods
        
        private void Awake()
        {
            LoadProperties();
            
            //TODO: logic
            currentPossibleReceivedElectricity = maxPossibleReceivedElectricity;
            currentPossibleReceivedWater = maxPossibleReceivedWater;
        }

        #endregion

        #region Methods

        private void LoadProperties()
        {
            var greenHouseSettings = buildingsSettings.GreenHouseSettings;

            maxPossibleReceivedElectricity = greenHouseSettings.MaxPossibleReceivedElectricity;
            maxPossibleReceivedWater = greenHouseSettings.MaxPossibleReceivedWater;
        }

        public void Receive(Electricity resource)
        {
            Debug.Log($"Received {resource.Value} energy");
            //TODO: logic
            return;
        }

        public void Receive(Water resource)
        {
            //TODO: logic
            return;
        }

        #endregion
    }
}
