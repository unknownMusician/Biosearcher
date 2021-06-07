using System.Collections.Generic;
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
        #region Properties

        [SerializeField] private BuildingsSettings buildingsSettings;
        [SerializeField] private Transform[] slotsObjects;

        private GreenHouseSettings settings;
        private Plant[] slots;

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
            LoadSettings();
            
            //TODO: logic
            slots = new Plant[settings.AmountOfSlots];

            maxPossibleReceivedElectricity = settings.MaxPossibleReceivedElectricity;
            maxPossibleReceivedWater = settings.MaxPossibleReceivedWater;
            currentPossibleReceivedElectricity = maxPossibleReceivedElectricity;
            currentPossibleReceivedWater = maxPossibleReceivedWater;
        }

        #endregion

        #region Methods

        private void LoadSettings()
        {
            settings = buildingsSettings.GreenHouseSettings;
        }

        public void Plant(Seed seed, int slotNumber)
        {
            var position = Vector3.zero;
            var rotation = Quaternion.identity;
            var parent = slotsObjects[slotNumber];
            slots[slotNumber] = seed.Plant(position, rotation, parent);
            Destroy(seed.gameObject);
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
