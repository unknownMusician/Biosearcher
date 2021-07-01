using System;
using Biosearcher.Buildings.GreenHouses;
using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Planets.Orientation;
using Biosearcher.Player;
using Biosearcher.Refactoring;
using Biosearcher.Weather;
using UnityEngine;

namespace Biosearcher.Plants
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public sealed class Capsule : MonoBehaviour, IInsertFriendly, IInsertable
    {
        #region Properties

        private static readonly Electricity ElectricityToControlIllumination = new Electricity { energy = 1 };
        private static readonly Electricity ElectricityToControlTemperature = new Electricity { energy = 1 };
        private static readonly Water WaterToControlHumidity = new Water { volume = 1 };

        private const float HumidityPercentagePerSecond = 0.1f;
        private const float IlluminationPercentagePerSecond = 0.1f;
        private const float TemperaturePercentagePerSecond = 0.1f;

        [SerializeField] private float _minDistanceToGround;
        [SerializeField] private LayerMask _groundMask;
        [Space]
        [SerializeField] private bool _controlHumidity;
        [SerializeField] private bool _controlIllumination;
        [SerializeField] private bool _controlTemperature;

        private WeatherRegulator _humidityRegulator;
        private WeatherRegulator _illuminationRegulator;
        private WeatherRegulator _temperatureRegulator;

        private Plant _plant;
        private Collider _collider;
        private Rigidbody _rigidbody;
        private GreenHouse _greenHouse;
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
        public GreenHouse GreenHouse
        {
            get => _greenHouse;
            set => _greenHouse = value;
        }
        
        public float CurrentHumidity => _humidityRegulator.CurrentValue;
        public float CurrentIllumination => _illuminationRegulator.CurrentValue;
        public float CurrentTemperature => _temperatureRegulator.CurrentValue;

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            _humidityRegulator = new WeatherRegulator(HumidityPercentagePerSecond, _controlHumidity);
            _illuminationRegulator = new WeatherRegulator(IlluminationPercentagePerSecond, _controlIllumination);
            _temperatureRegulator = new WeatherRegulator(TemperaturePercentagePerSecond, _controlTemperature);

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
            var position = transform.position;
            _humidityRegulator.Reset(WeatherController.GetHumidity(position));
            _illuminationRegulator.Reset(WeatherController.GetIllumination(position));
            _temperatureRegulator.Reset(WeatherController.GetTemperature(position));
        }

        [NeedsRefactor(Needs.Remove)]
        private void RegulateParameter(WeatherRegulator regulator, float outsideValue, float goalValue, float efficiency)
        {
            regulator.Regulate(outsideValue, goalValue, efficiency);
        }
        public void RegulateHumidity(float efficiency)
        {
            if (_plant != null)
            {
                RegulateParameter(_humidityRegulator, WeatherController.GetHumidity(transform.position), _plant.Settings.humidityRange.Average, efficiency);
            }
        }
        public void RegulateIllumination(float efficiency)
        {
            if (_plant != null)
            {
                RegulateParameter(_illuminationRegulator, WeatherController.GetIllumination(transform.position), _plant.Settings.illuminationRange.Average, efficiency);
            }
        }
        public void RegulateTemperature(float efficiency)
        {
            if (_plant != null)
            {
                RegulateParameter(_temperatureRegulator, WeatherController.GetTemperature(transform.position), _plant.Settings.temperatureRange.Average, efficiency);
            }
        }

        #endregion

        public Type[] GetInsertableType()
        {
            return new Type[] {typeof(Seed)};
        }
        public Vector3 GetAlignmentPosition()
        {
            return transform.position + _planetTransform.ToUniverse(0.5f * Vector3.up);
        }
        public void Insert(IInsertable insertable)
        {
            _greenHouse.Plant((Seed) insertable, this);
        }
        
        public void Drop()
        {
            if (Physics.OverlapSphere(transform.position, _minDistanceToGround, _groundMask).Length == 0)
            {
                _rigidbody.isKinematic = false;
            }
            _collider.enabled = true;
        }
        public void Grab()
        {
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
        }
    }
}
