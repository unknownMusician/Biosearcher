using Biosearcher.Level;
using Biosearcher.Player.Interactions;
using UnityEngine;

namespace Biosearcher.Plants
{
    public class GreenHouse : MonoBehaviour, IInsertFriendly<Seed>
    {
        [SerializeField] private Vector3 _alignLocalPosition;
        [SerializeField] private Score _score;

        private Plant _plant;

        public bool TryAlign(Seed seed)
        {
            if (_plant == null)
            {
                Align(seed.transform);
                return true;
            }
            return false;
        }

        public bool TryInsert(Seed seed)
        {
            if (_plant != null)
            {
                return false;
            }

            GameObject plantObject = Instantiate((GameObject)seed.PlantSettings.PlantPrefab);

            Destroy(seed.gameObject);

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

        private void HandleInsertPlant(GameObject plantObject, PlantSettings plantSettings)
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
}
