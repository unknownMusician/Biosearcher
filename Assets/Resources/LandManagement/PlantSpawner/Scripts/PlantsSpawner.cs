using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.LandManagement
{
    public class PlantsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _plantPrefabs;
        private static PlantsSpawner s_spawner;

        private List<GameObject> t_plants;

        private void Awake()
        {
            s_spawner = this;

            t_plants = (List<GameObject>)Spawn(new List<Ray> {
                new Ray(new Vector3(0, 0, 0), new Vector3(100, 100, 100))
            });
            //Dispose(t_plants);
        }

        public static IEnumerable<GameObject> Spawn(IEnumerable<Ray> groundNormals)
        {
            List<GameObject> plants = new List<GameObject>();
            foreach (Ray ray in groundNormals)
            {
                plants.Add(Instantiate(s_spawner._plantPrefabs[0], ray.origin, Quaternion.FromToRotation(Vector3.up, ray.direction), s_spawner.transform));
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
