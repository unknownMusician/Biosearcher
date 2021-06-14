using UnityEngine;

namespace Biosearcher.Weather
{
    public class WeatherController : MonoBehaviour
    {
        public static WeatherController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public float GetHumidity(Vector3 position)
        {
            return 10;
        }
        public float GetIllumination(Vector3 position)
        {
            return 10;
        }
        public float GetTemperature(Vector3 position)
        {
            return 10;
        }
    }
}