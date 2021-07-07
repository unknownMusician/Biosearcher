using Biosearcher.Common;
using Biosearcher.Planets;
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
        private Capsule _capsule;
        
        private float _growthProgress;
        private float _corruptionProgress;

        private bool _isGrowing;

        public PlantSettings Settings => _settings;
        public Capsule Capsule
        {
            set
            {
                EndGrowth();
                _capsule = value;
                StartGrowth();
            }
        }

        #endregion

        #region Methods

        public void Initialize(PlantSettings plantSettings)
        {
            _settings = plantSettings;
        }

        private bool AreGrowthConditionsAcceptable()
        {
            bool areAcceptable = true;

            areAcceptable &= _settings.humidityRange.Contains(_capsule.CurrentHumidity);
            areAcceptable &= _settings.illuminationRange.Contains(_capsule.CurrentIllumination);
            areAcceptable &= _settings.temperatureRange.Contains(_capsule.CurrentTemperature);
            
            return areAcceptable;
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

            if (AreGrowthConditionsAcceptable())
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
