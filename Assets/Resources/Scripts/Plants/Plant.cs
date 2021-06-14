using System.Collections;
using UnityEngine;

namespace Biosearcher.Plants
{
    public class Plant : MonoBehaviour
    {
        #region Properties

        private const float GrowthTicksPerSecond = 1;
        
        private PlantSettings _plantSettings;
        private Slot _slot;
        
        private float _growthProgress;
        private float _corruptionProgress;

        private bool _isGrowing;

        public PlantSettings PlantSettings => _plantSettings;

        public Slot Slot
        {
            set
            {
                EndGrowth();
                _slot = value;
                StartGrowth();
            }
        }

        #endregion

        #region Methods

        public void Initialize(PlantSettings plantSettings)
        {
            this._plantSettings = plantSettings;
        }

        private void StartGrowth()
        {
            _isGrowing = true;
            StartCoroutine(GrowthCycle());
        }
        private void EndGrowth()
        {
            _isGrowing = false;
        }

        private bool CheckGrowthConditions(float humidity, float illumination, float temperature)
        {
            var humidityCondition = _plantSettings.humidityRange.Contains(humidity);
            var illuminationCondition = _plantSettings.illuminationRange.Contains(illumination);
            var temperatureCondition = _plantSettings.temperatureRange.Contains(temperature);
            
            Debug.Log($"h : {humidity} | i : {illumination} | t : {temperature}");
            
            return humidityCondition && illuminationCondition && temperatureCondition;
        }
        private void GrowthTick()
        {
            if (_growthProgress >= 1)
            {
                EndGrowth();
                return;
            }
            if (_corruptionProgress >= 1)
            {
                EndGrowth();
                return;
            }
            
            if (CheckGrowthConditions(_slot.CurrentHumidity, _slot.CurrentIllumination, _slot.CurrentTemperature))
            {
                _growthProgress += GrowthTicksPerSecond / _plantSettings.timeToGrow;
            }
            else
            {
                _corruptionProgress += GrowthTicksPerSecond / _plantSettings.timeToCorrupt;
            }
        }
        private IEnumerator GrowthCycle()
        {
            while (_isGrowing)
            {
                GrowthTick();
                yield return new WaitForSeconds(1 / GrowthTicksPerSecond);
            }
        }

        #endregion
    }
}
