using UnityEngine;

namespace Biosearcher.Plants
{
    public class Seed : MonoBehaviour
    {
        [SerializeField] private PlantSettings plantSettings;

        public Plant Plant(Vector3 position, Quaternion rotation, Transform parent) 
        {
            var prefab = (GameObject) UnityEngine.Resources.Load("Prefabs/Plants/Plant");
            var plantObject = Instantiate(prefab, position, rotation, parent);
            var plant = plantObject.GetComponent<Plant>();
            plant.Initialize(plantSettings);
            
            return plant; 
        }
    }
}
