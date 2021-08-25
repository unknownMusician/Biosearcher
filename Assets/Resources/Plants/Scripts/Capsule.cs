using System;
using Biosearcher.Buildings.GreenHouses;
using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Common;
using Biosearcher.Planets;
using Biosearcher.Planets.Orientation;
using Biosearcher.Player;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Plants
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public sealed class Capsule : MonoBehaviour, IInsertFriendly<Seed>, IInsertable
    {
        #region Properties

        private LayerMask _realMask;

        [NeedsRefactor] private static readonly Electricity ElectricityToControlIllumination = new Electricity(1);
        [NeedsRefactor] private static readonly Electricity ElectricityToControlTemperature = new Electricity(1);
        [NeedsRefactor] private static readonly Water WaterToControlHumidity = new Water(1);

        private const float HumidityPercentagePerSecond = 0.1f;
        private const float IlluminationPercentagePerSecond = 0.1f;
        private const float TemperaturePercentagePerSecond = 0.1f;

        [SerializeField] private float _minDistanceToGround;
        [SerializeField] private LayerMask _groundMask;
        [Space]
        [SerializeField] private bool _controlHumidity;
        [SerializeField] private bool _controlIllumination;
        [SerializeField] private bool _controlTemperature;

        private WeatherRegulator<Humidity> _humidityRegulator;
        private WeatherRegulator<Illumination> _illuminationRegulator;
        private WeatherRegulator<Temperature> _temperatureRegulator;

        private Plant _plant;
        private Collider _collider;
        private Rigidbody _rigidbody;
        private PlanetTransform _planetTransform;


        public Electricity NeededElectricity
        {
            get
            {
                Electricity neededElectricity = default;

                if (_plant == null)
                {
                    return neededElectricity;
                }

                if (_controlIllumination)
                {
                    neededElectricity += ElectricityToControlIllumination;
                }
                if (_controlTemperature)
                {
                    neededElectricity += ElectricityToControlTemperature;
                }

                return neededElectricity;
            }
        }
        public Water NeededWater
        {
            get
            {
                Water neededWater = default;

                if (_plant == null)
                {
                    return neededWater;
                }

                if (_controlHumidity)
                {
                    neededWater += WaterToControlHumidity;
                }

                return neededWater;
            }
        }

        public Plant Plant
        {
            get => _plant;
            set
            {
                _plant = value;
                _plant.Capsule = this;
            }
        }
        public GreenHouse GreenHouse { get; set; }

        public Humidity CurrentHumidity => _humidityRegulator.GetCurrentValue(transform.position);
        public Illumination CurrentIllumination => _illuminationRegulator.GetCurrentValue(transform.position);
        public Temperature CurrentTemperature => _temperatureRegulator.GetCurrentValue(transform.position);

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            _humidityRegulator = new WeatherRegulator<Humidity>(HumidityPercentagePerSecond, _controlHumidity);
            _illuminationRegulator = new WeatherRegulator<Illumination>(IlluminationPercentagePerSecond, _controlIllumination);
            _temperatureRegulator = new WeatherRegulator<Temperature>(TemperaturePercentagePerSecond, _controlTemperature);

            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
            _planetTransform = GetComponent<PlanetTransform>();
        }
        private void Start()
        {
            ResetWeatherParameters();
        }

        #endregion

        #region Methods

        private void ResetWeatherParameters()
        {
            _humidityRegulator.Reset(_plant.Settings.humidityRange.Average());
            _illuminationRegulator.Reset(_plant.Settings.illuminationRange.Average());
            _temperatureRegulator.Reset(_plant.Settings.temperatureRange.Average());
        }

        [NeedsRefactor(Needs.Remove)]
        private void RegulateParameterIfPlantIsNotNull<TWeatherParameter>(WeatherRegulator<TWeatherParameter> regulator, float efficiency)
            where TWeatherParameter : IWeatherParameter<TWeatherParameter>
        {
            if (_plant != null)
            {
                regulator.Regulate(efficiency);
            }
        }
        public void RegulateHumidity(float efficiency) => RegulateParameterIfPlantIsNotNull(_humidityRegulator, efficiency);
        public void RegulateIllumination(float efficiency) => RegulateParameterIfPlantIsNotNull(_illuminationRegulator, efficiency);
        public void RegulateTemperature(float efficiency) => RegulateParameterIfPlantIsNotNull(_temperatureRegulator, efficiency);

        public bool TryInsert(Seed insertable)
        {
            var capsuleTransform = transform;

            Vector3 position = capsuleTransform.position;
            Quaternion rotation = capsuleTransform.rotation;
            Plant = insertable.Plant(position, rotation, capsuleTransform);
            GreenHouse.PlantChanged();

            return true;
        }
        public bool TryAlign(Seed insertable)
        {
            insertable.transform.position = transform.position + _planetTransform.ToUniverse(0.5f * Vector3.up);
            return true;
        }

        public void HandleInsertableGrabbed(Seed insertable) { }

        public void HandleGrab()
        {
            this.HandleGrabDefault(out _realMask);

            _rigidbody.isKinematic = true;
            _collider.enabled = false;
        }
        public void HandleDrop()
        {
            this.HandleDropDefault(_realMask);

            if (Physics.OverlapSphere(transform.position, _minDistanceToGround, _groundMask).Length == 0)
            {
                _rigidbody.isKinematic = false;
            }
            _collider.enabled = true;
        }
        public void HandleInsert()
        {
            this.HandleInsertDefault(_realMask);
        }
        public bool TryInsertIn(IInsertFriendly insertFriendly) => this.TryInsertInGeneric(insertFriendly);
        public bool TryAlignWith(IInsertFriendly insertFriendly) => this.TryAlignWithGeneric(insertFriendly);

        #endregion
    }
}
