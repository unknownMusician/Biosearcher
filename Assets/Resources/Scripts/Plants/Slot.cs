using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Weather;
using UnityEngine;

namespace Biosearcher.Plants
{
    public sealed class Slot : MonoBehaviour
    {
        #region Properties

        private static readonly Electricity ElectricityToControlIllumination = new Electricity { energy = 1 };
        private static readonly Electricity ElectricityToControlTemperature = new Electricity { energy = 1 };
        private static readonly Water WaterToControlHumidity = new Water { volume = 1 };

        private const float HumidityPercentagePerSecond = 0.1f;
        private const float IlluminationPercentagePerSecond = 0.1f;
        private const float TemperaturePercentagePerSecond = 0.1f;

        [SerializeField] private bool _controlHumidity;
        [SerializeField] private bool _controlIllumination;
        [SerializeField] private bool _controlTemperature;

        private WeatherRegulator _humidityRegulator;
        private WeatherRegulator _illuminationRegulator;
        private WeatherRegulator _temperatureRegulator;

        private WeatherController _weatherController;
        private Plant _plant;

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
                _plant.Slot = this;
            }
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
        }
        private void Start()
        {
            _weatherController = WeatherController.Instance;
            ResetWeatherParameters();
        }

        #endregion

        #region Methods

        private void ResetWeatherParameters()
        {
            var position = transform.position;
            _humidityRegulator.Reset(_weatherController.GetHumidity(position));
            _illuminationRegulator.Reset(_weatherController.GetIllumination(position));
            _temperatureRegulator.Reset(_weatherController.GetTemperature(position));
        }

        private void RegulateParameter(WeatherRegulator regulator, float outsideValue, float goalValue, float efficiency)
        {
            if (_plant != null)
            {
                regulator.Regulate(outsideValue, goalValue, efficiency);
            }
        }
        public void RegulateHumidity(float efficiency)
        {
            RegulateParameter(_humidityRegulator, _weatherController.GetHumidity(transform.position), _plant.Settings.humidityRange.Average, efficiency);
        }
        public void RegulateIllumination(float efficiency)
        {
            RegulateParameter(_illuminationRegulator, _weatherController.GetIllumination(transform.position), _plant.Settings.illuminationRange.Average, efficiency);
        }
        public void RegulateTemperature(float efficiency)
        {
            RegulateParameter(_temperatureRegulator, _weatherController.GetTemperature(transform.position), _plant.Settings.temperatureRange.Average, efficiency);
        }

        #endregion
    }
}
