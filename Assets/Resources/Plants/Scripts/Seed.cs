using UnityEngine;

namespace Biosearcher.Plants
{
    public class Seed : MonoBehaviour
    {
        [SerializeField] private PlantSettings _plantSettings;

        public Plant Plant(Vector3 position, Quaternion rotation, Transform parent) 
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Plants/Plant");
            var plantObject = Instantiate(prefab, position, rotation, parent);
            var plant = plantObject.GetComponent<Plant>();
            plant.Initialize(_plantSettings);

            return plant; 
        }
    }
}
