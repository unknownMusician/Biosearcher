using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.LandManagement
{
    public sealed class PlantsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _plantPrefabs;
        [SerializeField] private Settings.LandSettings _settings;
        private static PlantsSpawner s_spawner;

        private void Awake()
        {
            s_spawner = this;
        }
        public static IEnumerable<GameObject> Spawn(IEnumerable<Ray> groundNormals)
        {
            List<GameObject> plants = new List<GameObject>();
            GameObject current;
            foreach (Ray ray in groundNormals)
            {
                if (Random.value > s_spawner._settings.PlantGenProbability)
                {
                    continue;
                }
                current = Instantiate(
                    s_spawner._plantPrefabs[0],
                    ray.origin,
                    Quaternion.FromToRotation(Vector3.up, ray.direction),
                    s_spawner.transform);
                current.transform.Rotate(ray.direction, Random.Range(-180f, 180f), Space.World);
                plants.Add(current);
            }
            return plants;
        }
        public static void Dispose(IEnumerable<GameObject> spawnedObjects)
        {
            foreach (GameObject plant in spawnedObjects)
            {
                Destroy(plant);
            }
        }
    }
}
