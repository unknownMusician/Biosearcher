using UnityEngine;

namespace Biosearcher.Resources.Scripts
{
    public class WeatherController : MonoBehaviour
    {
        public static WeatherController Instance;

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