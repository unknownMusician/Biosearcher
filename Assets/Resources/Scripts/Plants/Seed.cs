using UnityEngine;

namespace Biosearcher.Plants
{
    public class Seed : MonoBehaviour
    {
        [SerializeField] private PlantSettings plantSettings;

        public void Plant()
        {
            var prefab = (GameObject) Resources.Load("Prefabs/Plants/Plant");
            var position = Vector3.zero;
            var rotation = Quaternion.identity;
            var plant = Instantiate(prefab, position, rotation);
            plant.GetComponent<Plant>().Initialize(plantSettings);
            Destroy(gameObject);
        }
    }
}
