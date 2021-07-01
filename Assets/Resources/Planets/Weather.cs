using Biosearcher.Common;
using Biosearcher.Planets.Orientation;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Planets
{
    public sealed class Weather
    {
        public static Weather Current => Planet.Current.Weather;

        private readonly Vector3 _planetCenter;
        private readonly Range<float> _temperatureRange;
        private readonly Range<float> _illuminationRange;
        private readonly Range<float> _humidityRange;
        private readonly Planet _planet;

        public Weather(Planet planet, Range<float> temperatureRange, Range<float> illuminationRange, Range<float> humidityRange)
        {
            _planet = planet;
            _planetCenter = planet.Center;
            _temperatureRange = temperatureRange;
            _illuminationRange = illuminationRange;
            _humidityRange = humidityRange;
        }

        [NeedsRefactor(Needs.Check)]
        public float GetHumidity(Vector3 position)
        {
            return _humidityRange.Lerp(LandManagement.CubeMarching.CPU.Noise.Gradient(position / 200));
        }

        [NeedsRefactor("Optimize raycast", Needs.Refactor)]
        public float GetIllumination(Vector3 position, float objectRadius = 3)
        {
            Vector3 localPosition = position - _planetCenter;
            Vector3 positionWithOffset = position + localPosition.normalized * objectRadius;

            float maxIlluminationDistance = 10;

            float timelessIlluminationLerp;
            if (Physics.Raycast(positionWithOffset, localPosition, out RaycastHit hit))
            {
                timelessIlluminationLerp = Mathf.Clamp01(Vector3.Distance(position, hit.point) / maxIlluminationDistance);
            }
            else
            {
                timelessIlluminationLerp = 1;
            }
            float timeLerp = GetTimeLerp(position);

            return _illuminationRange.Lerp(timelessIlluminationLerp * timeLerp);
        }

        [NeedsRefactor("Add Noise", Needs.Refactor)]
        public float GetTemperature(Vector3 position)
        {
            float positionLerp = 1 - Mathf.Abs(PlanetTransform.ToLatitude(position - _planetCenter)) / 90;
            float timeLerp = GetTimeLerp(position);
            const float positionSignificance = 0.8f;
            const float timeSignificance = 1 - positionSignificance;
            return _temperatureRange.Lerp(positionLerp * positionSignificance + timeLerp * timeSignificance);
        }

        private float GetTimeLerp(Vector3 position)
        {
            return 1 - Mathf.Abs(_planet.Time.Get(position).DayLerp * 2 - 1);
        }

        [NeedsRefactor]
        public static string HumidityToString(float humidity)
        {
            return string.Format("{0:f2} %", humidity);
        }

        [NeedsRefactor]
        public static string IlluminationToString(float illumination)
        {
            return string.Format("{0:f2} lux", illumination);
        }

        [NeedsRefactor]
        public static string TemperatureToString(float temperature)
        {
            return string.Format("{0:f2} °C", temperature);
        }
    }
}