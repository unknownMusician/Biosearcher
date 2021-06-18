using System.Collections;
using UnityEngine;

namespace Biosearcher.Plants
{
    public class Plant : MonoBehaviour
    {
        #region Properties

        // todo
        private const float GrowthTicksPerSecond = 1;
        
        private PlantSettings _settings;
        private Slot _slot;
        
        private float _growthProgress;
        private float _corruptionProgress;

        private bool _isGrowing;

        public PlantSettings Settings => _settings;
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
            _settings = plantSettings;
        }

        private bool AreGrowthConditionsAcceptable(float humidity, float illumination, float temperature)
        {
            var humidityCondition = _settings.humidityRange.Contains(humidity);
            var illuminationCondition = _settings.illuminationRange.Contains(illumination);
            var temperatureCondition = _settings.temperatureRange.Contains(temperature);
            
            // todo
            // Debug.Log($"h : {humidity} | i : {illumination} | t : {temperature}");
            
            return humidityCondition && illuminationCondition && temperatureCondition;
        }
        
        private void Grow()
        {
            _growthProgress += GrowthTicksPerSecond / _settings.timeToGrow;
        }
        private void Corrupt()
        {
            _corruptionProgress += GrowthTicksPerSecond / _settings.timeToCorrupt;
        }
        
        private void Tick()
        {
            // todo: shouldn't it be after growth and corruption?
            if (_growthProgress >= 1 || _corruptionProgress >= 1)
            {
                EndGrowth();
                return;
            }

            if (AreGrowthConditionsAcceptable(_slot.CurrentHumidity, _slot.CurrentIllumination, _slot.CurrentTemperature))
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
            var waitForSeconds = new WaitForSeconds(1 / GrowthTicksPerSecond);
            while (_isGrowing)
            {
                Tick();
                yield return waitForSeconds;
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
