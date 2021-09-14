using Biosearcher.Level;
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
        [SerializeField] private Score _score;

        private Plant _plant;

        public bool TryAlign(ISeed seed)
        {
            if (_plant == null)
            {
                Align(seed.transform);
                return true;
            }
            return false;
        }

        public bool TryInsert(ISeed seed)
        {
            if (_plant != null)
            {
                return false;
            }

            GameObject plantObject = Instantiate(seed.PlantSettings.PlantPrefab);

            HandleInsertTransform(plantObject);
            HandleInsertPlant(plantObject, seed.PlantSettings);

            return true;
        }

        private void Align(Transform insertable)
        {
            insertable.transform.SetPositionAndRotation(transform.position + _alignLocalPosition, transform.rotation);
        }

        private void HandleInsertTransform(GameObject plantObject)
        {
            Align(plantObject.transform);
            plantObject.transform.SetParent(transform);
        }

        private void HandleInsertPlant(GameObject plantObject, IPlantSettings plantSettings)
        {
            _plant = plantObject.GetComponent<Plant>();
            _plant.OnGrowEnd += HandlePlantGrowed;
            _plant.TryStartGrow(plantSettings);
        }

        private void HandlePlantGrowed()
        {
            _score.AddScore(_plant.PlantSettings.Score);

            Destroy(_plant.gameObject);
        }
    }

    public sealed class Plant : MonoBehaviour
    {
        public event Action OnGrowEnd;

        private bool _isGrowing = false;
        public IPlantSettings PlantSettings { get; private set; }

        public void TryStartGrow(IPlantSettings plantSettings)
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
            float oneOverGrowTime = 1.0f / PlantSettings.GrowTime;
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
        int Score { get; }
    }

    public interface ISeed : IInsertable
    {
        IPlantSettings PlantSettings { get; }
        Transform transform { get; }
    }
}
