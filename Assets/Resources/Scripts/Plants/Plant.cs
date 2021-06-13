using System.Collections;
using UnityEngine;

namespace Biosearcher.Plants
{
    public class Plant : MonoBehaviour
    {
        #region Properties

        private const float GrowthTicksPerSecond = 1;
        
        private PlantSettings plantSettings;
        private Slot slot;

        public PlantSettings PlantSettings => plantSettings;

        private float growthProgress;
        private float corruptionProgress;

        private bool isGrowing;

        #endregion

        #region Methods

        public void Initialize(PlantSettings plantSettings)
        {
            this.plantSettings = plantSettings;
        }

        public void ChangeSlot(Slot slot)
        {
            EndGrowth();
            this.slot = slot;
            StartGrowth();
        }

        private void StartGrowth()
        {
            Debug.Log("Growth started!");
            isGrowing = true;
            StartCoroutine(GrowthCycle());
        }
        private void EndGrowth()
        {
            Debug.Log("Growth ended!");
            isGrowing = false;
        }

        private bool CheckGrowthConditions(float humidity, float illumination, float temperature)
        {
            var humidityCondition = plantSettings.humidityRange.Contains(humidity);
            var illuminationCondition = plantSettings.illuminationRange.Contains(illumination);
            var temperatureCondition = plantSettings.temperatureRange.Contains(temperature);
            
            return humidityCondition && illuminationCondition && temperatureCondition;
        }
        private void GrowthTick()
        {
            if (growthProgress.Equals(1))
            {
                EndGrowth();
                return;
            }
            
            if (!CheckGrowthConditions(slot.CurrentHumidity, slot.CurrentIllumination, slot.CurrentTemperature))
            {
                corruptionProgress += GrowthTicksPerSecond / plantSettings.timeToCorrupt;
                Debug.Log($"Corruption: {corruptionProgress}");
                return;
            }
            
            growthProgress += GrowthTicksPerSecond / plantSettings.timeToGrow;
            Debug.Log($"Progress: {growthProgress}");
        }
        private IEnumerator GrowthCycle()
        {
            while (isGrowing)
            {
                GrowthTick();
                yield return new WaitForSeconds(1 / GrowthTicksPerSecond);
            }
        }

        #endregion
    }
}
