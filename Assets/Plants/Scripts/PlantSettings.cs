using UnityEngine;

namespace Biosearcher.Plants
{
    [CreateAssetMenu(fileName = "Default Plant", menuName = "Plant Settings")]
    public class PlantSettings : ScriptableObject
    {
        [SerializeField] private string _name = "Plant";
        [SerializeField] private GameObject _plantPrefab;
        [SerializeField, Tooltip("In seconds.")] private float _growthTime = 10.0f;          // in seconds
        [SerializeField] private int _score = 10;          // in seconds

        public string Name => _name;
        public GameObject PlantPrefab => _plantPrefab;
        public float GrowthTime => _growthTime;
        public int Score => _score;
    }
}
