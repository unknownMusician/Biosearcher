using System.Collections;
using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Resources.Scripts;
using UnityEngine;

namespace Biosearcher.Plants
{
    public class Slot : MonoBehaviour
    {
        #region Properties
        
        private static readonly Electricity ElectricityToControlHumidity = new Electricity {energy = 1};
        private static readonly Electricity ElectricityToControlIllumination = new Electricity {energy = 1};
        private static readonly Electricity ElectricityToControlTemperature = new Electricity {energy = 1};

        private static readonly float ParametersPercentagePerSecond = 10;

        [SerializeField] private bool controlHumidity;
        [SerializeField] private bool controlIllumination;
        [SerializeField] private bool controlTemperature;
        
        private WeatherController weatherController;
        private Plant plant;
        private float t;

        private float currentHumidity;
        private float currentIllumination;
        private float currentTemperature;

        public float CurrentHumidity => currentHumidity;
        public float CurrentIllumination => currentIllumination; 
        public float CurrentTemperature => currentTemperature;
        
        public Electricity NeededElectricity
        {
            get
            {
                Electricity neededElectricity = default;

                if (plant == null)
                {
                    return neededElectricity;
                }
                
                if (controlHumidity) neededElectricity += ElectricityToControlHumidity;
                if (controlIllumination) neededElectricity += ElectricityToControlIllumination;
                if (controlTemperature) neededElectricity += ElectricityToControlTemperature;

                return neededElectricity;
            }
        }

        #endregion

        private void Start()
        {
            weatherController = WeatherController.Instance;
            ResetWeatherParameters();
        }

        private void ResetWeatherParameters()
        {
            t = 0;
            var position = transform.position;
            currentHumidity = weatherController.GetHumidity(position);
            currentIllumination = weatherController.GetHumidity(position);
            currentTemperature = weatherController.GetTemperature(position);
        }

        public void ChangePlant(Plant plant)
        {
            this.plant = plant;
            plant.ChangeSlot(this);
        }

        public void DecideWhatToDo(float percentage)
        {
            if (plant == null) return;

            if (!percentage.Equals(1))
            {
                t -= (ParametersPercentagePerSecond / 100) * (1 - percentage);
                if (t <= 0)
                {
                    t = 0;
                }
            }
            else
            {
                t += (ParametersPercentagePerSecond / 100);
                if (t >= 1)
                {
                    t = 1;
                }
            }

            if (controlHumidity)
            {
                var position = transform.position;
                currentHumidity = Mathf.Lerp(weatherController.GetHumidity(position), plant.PlantSettings.humidityRange.Average, t);
            }
            if (controlIllumination)
            {
                var position = transform.position;
                currentIllumination = Mathf.Lerp(weatherController.GetIllumination(position), plant.PlantSettings.illuminationRange.Average, t);
            }
            if (controlTemperature)
            {
                var position = transform.position;
                currentTemperature = Mathf.Lerp(weatherController.GetTemperature(position), plant.PlantSettings.temperatureRange.Average, t);
            }
            
            Debug.Log($"Humidity: {currentHumidity} | Illumination: {currentIllumination} | Temperature: {currentTemperature} | t: {t}");
        }
    }
}
