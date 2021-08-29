using Biosearcher.Planets;
using UnityEngine;

namespace Biosearcher.Plants
{
    [CreateAssetMenu(fileName = "Plant Settings", menuName = "Plant Settings", order = 53)]
    public sealed class PlantSettings : ScriptableObject
    {
        [SerializeField] private string _plantName;
        [SerializeField] private GameObject _plantPrefab;
        [SerializeField] private GameObject _seedPrefab;

        [SerializeField] private float _timeToGrow;
        [SerializeField] private float _timeToCorrupt;

        [SerializeField] private WeatherRangeParameters _weatherParameters;
        [SerializeField] [Range(0.0f, 1.0f)] private float _probability;

        public string PlantName => _plantName;
        public GameObject PlantPrefab => _plantPrefab;
        public GameObject SeedPrefab => _seedPrefab;

        public float TimeToGrow => _timeToGrow;
        public float TimeToCorrupt => _timeToCorrupt;

        public WeatherRangeParameters WeatherParameters => _weatherParameters;
        public float Probability => _probability;
    }
}
