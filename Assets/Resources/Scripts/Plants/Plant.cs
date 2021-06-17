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
            _plantSettings = plantSettings;
        }

        private bool CheckGrowthConditions(float humidity, float illumination, float temperature)
        {
            var humidityCondition = _plantSettings.humidityRange.Contains(humidity);
            var illuminationCondition = _plantSettings.illuminationRange.Contains(illumination);
            var temperatureCondition = _plantSettings.temperatureRange.Contains(temperature);
            
            // Debug.Log($"h : {humidity} | i : {illumination} | t : {temperature}");
            
            return humidityCondition && illuminationCondition && temperatureCondition;
        }
        
        private void Grow()
        {
            _growthProgress += GrowthTicksPerSecond / _plantSettings.timeToGrow;
        }
        private void Corrupt()
        {
            _corruptionProgress += GrowthTicksPerSecond / _plantSettings.timeToCorrupt;
        }
        
        private void Tick()
        {
            if (_growthProgress >= 1 || _corruptionProgress >= 1)
            {
                EndGrowth();
                return;
            }

            if (CheckGrowthConditions(_slot.CurrentHumidity, _slot.CurrentIllumination, _slot.CurrentTemperature))
            {
                Grow();
            }
            else
            {
                Corrupt();
            }
        }
        
        private IEnumerator GrowthCycle()
        {
            var tickDelay = new WaitForSeconds(1 / GrowthTicksPerSecond);
            while (_isGrowing)
            {
                Tick();
                yield return tickDelay;
            }
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
        
        #endregion
    }
}
