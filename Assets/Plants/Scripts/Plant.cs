using Biosearcher.Refactoring;
using System;
using System.Collections;
using UnityEngine;

namespace Biosearcher.Plants
{
    public sealed class Plant : MonoBehaviour
    {
        public event Action OnGrowEnd;

        private bool _isGrowing = false;
        public PlantSettings PlantSettings { get; private set; }

        public void TryStartGrow(PlantSettings plantSettings)
        {
            if (!_isGrowing)
            {
                PlantSettings = plantSettings;
                StartCoroutine(Growing());
            }
        }

        [NeedsRefactor("Get growTime from plantSettings")]
        private IEnumerator Growing()
        {
            _isGrowing = true;
            float oneOverGrowTime = 1.0f / PlantSettings.GrowthTime;
            float lerp = 0.0f;

            while (lerp < 1.0f)
            {
                HandleGrowth(lerp);

                lerp += Time.deltaTime * oneOverGrowTime;
                yield return null;
            }

            HandleGrowth(1.0f);

            _isGrowing = false;

            OnGrowEnd?.Invoke();
        }

        private void HandleGrowth(float lerp) => transform.localScale = lerp * Vector3.one;
    }
}
