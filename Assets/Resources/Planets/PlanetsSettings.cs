using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Planets
{
    [CreateAssetMenu(fileName = "Planets Settings", menuName = "Planet Settings", order = 53)]
    public class PlanetsSettings : ScriptableObject
    {
        [SerializeField] private PlanetSettings[] _planets;

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            PlanetsSettings settings = Resources.Load<PlanetsSettings>("Settings/Planets Settings");
            foreach (PlanetSettings planet in settings._planets)
            {
                Planet.Create(planet);
            }
        }
    }

    [System.Serializable]
    public class PlanetSettings
    {
        [NeedsRefactor("Sync with Coordinates")]
        [SerializeField] private Vector3 _rotationAxis = Vector3.up;
        [NeedsRefactor]
        [SerializeField] private bool _isCurrent = true;
        [NeedsRefactor(Needs.Implementation)]
        [SerializeField] private float _radius = 400.0f;
        [SerializeField] private WeatherRangeParameters _weatherParameters = new WeatherRangeParameters((0, 100), (0.01f, 100_000), (-40, 40));

        public Vector3 RotationAxis => _rotationAxis;
        public bool IsCurrent => _isCurrent;
        public float Radius => _radius;
        public WeatherRangeParameters WeatherParameters => _weatherParameters;
    }
}