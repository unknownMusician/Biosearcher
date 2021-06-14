using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Buildings.Settings;
using Biosearcher.Buildings.Types.Interfaces;
using UnityEngine;

namespace Biosearcher.Buildings.Generators
{
    public class CoalGenerator : MonoBehaviour, IResourceProducer<Electricity>
    {
        #region Properties

        [SerializeField] private BuildingsSettings buildingsSettings;

        public Electricity MaxPossibleProduced { get; protected set; }
        public Electricity CurrentPossibleProduced { get; protected set; }

        public float NeededCoal { get; protected set; }
        public float CurrentCoal { get; set; }

        #endregion

        #region Behaviour methods

        private void Awake()
        {
            LoadProperties();

            CurrentCoal = 15f;
        }

        private void Start()
        {
            RecalculateProducing();
        }

        #endregion

        #region Methods

        private void LoadProperties()
        {
            var coalGeneratorParams = buildingsSettings.CoalGeneratorSettings;

            MaxPossibleProduced = coalGeneratorParams.MaxPossibleEnergyProduced;
            NeededCoal = coalGeneratorParams.NeededCoal;
        }

        private void RecalculateProducing()
        {
            if (CurrentCoal == 0)
            {
                CurrentPossibleProduced = default;
            }
            else if (CurrentCoal >= NeededCoal)
            {
                CurrentPossibleProduced = MaxPossibleProduced;
            }
            else
            {
                var currentToMaxRatio = new Electricity {energy = CurrentCoal / NeededCoal};
                CurrentPossibleProduced = MaxPossibleProduced.Multiply(currentToMaxRatio);
            }
        }

        private void ReduceCurrentCoal(float value)
        {
            if (CurrentCoal - NeededCoal < 0)
            {
                CurrentCoal = 0;
                return;
            }

            CurrentCoal -= value;
        }

        public Electricity Produce()
        {
            RecalculateProducing();
            ReduceCurrentCoal(NeededCoal);
            return CurrentPossibleProduced;
        }

        #endregion
    }
}