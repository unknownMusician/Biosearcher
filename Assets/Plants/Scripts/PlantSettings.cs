using UnityEngine;

namespace Biosearcher.Test
{
    [CreateAssetMenu(fileName = "Default Plant", menuName = "Plant Settings")]
    public class PlantSettings : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private GameObject _plantPrefab;
        [SerializeField] private float _growthTime;          // in seconds

        public string Name => _name;
        public GameObject PlantPrefab => _plantPrefab;
        public float GrowthTime => _growthTime;
    }
}
