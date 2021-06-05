using System.Collections;
using UnityEngine;

namespace Biosearcher.Plants
{
    public class Plant : MonoBehaviour
    {
        #region Properties

        private PlantSettings plantSettings;

        private float growthProgress;
        private float corruption;

        private bool isGrowing;

        private float ticksPerSecond;

        #endregion

        #region Behaviour methods

        private void Awake()
        {
            growthProgress = 0;
            corruption = 0;
            ticksPerSecond = 1;
        }

        #endregion

        #region Methods

        public void Initialize(PlantSettings plantSettings)
        {
            this.plantSettings = plantSettings;
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
        
        private void GrowthTick()
        {
            if (growthProgress.Equals(1))
            {
                EndGrowth();
                return;
            }
            // if temperature, humidity and illumination is ok
            growthProgress += ticksPerSecond / plantSettings.timeToGrow;
            Debug.Log($"Progress: {growthProgress}");
        }
        private IEnumerator GrowthCycle()
        {
            while (isGrowing)
            {
                GrowthTick();
                yield return new WaitForSeconds(1 / ticksPerSecond);
            }
        }

        #endregion
    }
}
