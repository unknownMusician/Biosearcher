using Biosearcher.Player.Interactions;
using Biosearcher.Refactoring;
using System;
using System.Collections;
using UnityEngine;

namespace Biosearcher.Plants
{
    public class GreenHouse : MonoBehaviour, IInsertFriendly<ISeed>
    {
        [SerializeField] private Vector3 _alignLocalPosition;

        private Plant _plant;

        public bool TryAlign(ISeed seed)
        {
            seed.transform.SetPositionAndRotation(transform.position + _alignLocalPosition, transform.rotation);
            return true;
        }

        [NeedsRefactor("Get Plant GameObject from seed")]
        public bool TryInsert(ISeed seed)
        {
            if (_plant != null)
            {
                return false;
            }

            // todo
            GameObject plantPrefab = seed.PlantSettings.PlantPrefab;
            GameObject plantObject =Instantiate(plantPrefab);

            plantObject.transform.SetPositionAndRotation(transform.position + _alignLocalPosition, transform.rotation);
            plantObject.transform.SetParent(transform);

            _plant = plantObject.GetComponent<Plant>();
            _plant.TryStartGrow(seed.PlantSettings);

            return true;
        }
    }

    public sealed class Plant : MonoBehaviour
    {
        private bool _isGrowing = false;
        private IPlantSettings _plantSettings;

        public event Action OnGrowEnd;

        public void TryStartGrow(IPlantSettings plantSettings)
        {
            if (!_isGrowing)
            {
                _plantSettings = plantSettings;
                StartCoroutine(Growing());
            }
        }

        [NeedsRefactor("Get growTime from plantSettings")]
        private IEnumerator Growing()
        {
            _isGrowing = true;
            float oneOverGrowTime = 1.0f / _plantSettings.GrowTime;
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

    public interface IPlantSettings
    {
        float GrowTime { get; }
        GameObject PlantPrefab { get; }
    }

    public interface ISeed : IInsertable
    {
        IPlantSettings PlantSettings { get; }
        Transform transform { get; }
    }
}
