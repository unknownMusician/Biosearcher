using Biosearcher.Common;
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
        public Vector3 rotationAxis = Vector3.up;
        [NeedsRefactor]
        public bool isCurrent = true;
        public Range<float> temperatureRange = new Range<float>(-40, 40);
        public Range<float> illuminationRange = new Range<float>(0.01f, 100_000);
        public Range<float> humidityRange = new Range<float>(0, 100);
    }
}