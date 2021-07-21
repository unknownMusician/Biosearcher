using Biosearcher.Common;
using Biosearcher.Refactoring;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher.LandManagement
{
    public class PlantsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _plantPrefabs;
        private static PlantsSpawner s_spawner;

        //private List<GameObject> t_plants;

        private void Awake()
        {
            s_spawner = this;

            //t_plants = (List<GameObject>)Spawn(new List<Ray> {
            //    new Ray(new Vector3(0, 0, 0), new Vector3(100, 100, 100))
            //});
            //Dispose(t_plants);
        }

        [NeedsRefactor("Implement probability")]
        public static IEnumerable<GameObject> Spawn(IEnumerable<Ray> groundNormals)
        {
            List<GameObject> plants = new List<GameObject>();
            foreach (Ray ray in groundNormals)
            {
                // here
                // Random.value < neededProbability
                // or
                // Noise.Gradient(ray.origin) < neededProbability
                if (Random.value < 0.3f)
                {
                    plants.Add(Instantiate(s_spawner._plantPrefabs[0], ray.origin, Quaternion.FromToRotation(Vector3.up, ray.direction), s_spawner.transform));
                }
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
