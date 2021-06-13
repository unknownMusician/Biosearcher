using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Buildings.Settings;
using Biosearcher.Buildings.Settings.Structs;
using Biosearcher.Buildings.Types.Interfaces;
using Biosearcher.Plants;
using UnityEngine;

//TODO: убрать паблики!!!

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

        private GreenHouseSettings settings;

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

            maxPossibleReceivedElectricity = settings.MaxPossibleReceivedElectricity;
            maxPossibleReceivedWater = settings.MaxPossibleReceivedWater;
            currentPossibleReceivedElectricity = default;
            currentPossibleReceivedWater = default;
        }
        private void Start()
        {
            RecalculateNeededElectricity();
        }

        #endregion

        #region Methods

        private void LoadSettings()
        {
            settings = buildingsSettings.GreenHouseSettings;
        }

        private void RecalculateNeededElectricity()
        {
            currentPossibleReceivedElectricity = default;
            foreach (var slot in slots)
            {
                if (slot == null)
                {
                    continue;
                }
                currentPossibleReceivedElectricity += slot.NeededElectricity;
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
            slots[slotNumber].ChangePlant(plant);
            RecalculateNeededElectricity();
            Destroy(seed.gameObject);
        }
        public void ChangeSlot(Slot slot, int slotNumber)
        {
            slots[slotNumber] = slot;
            RecalculateNeededElectricity();
        }

        public void Receive(Electricity resource)
        {
            var percentage = (resource / currentPossibleReceivedElectricity).Value;
            foreach (var slot in slots)
            {
                if (slot == null)
                {
                    continue;
                }
                
                slot.DecideWhatToDo(percentage);
            }
        }
        public void Receive(Water resource)
        {
            //TODO: logic
            return;
        }

        #endregion
    }
}
