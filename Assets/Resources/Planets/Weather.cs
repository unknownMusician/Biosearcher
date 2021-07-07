using Biosearcher.Common;
using Biosearcher.Common.Interfaces;
using Biosearcher.Planets.Orientation;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Planets
{
    public class Weather :
        IWeatherParameterProvider<Humidity>,
        IWeatherParameterProvider<Illumination>,
        IWeatherParameterProvider<Temperature>
    {
        public static Weather Current => Planet.Current.Weather;

        private readonly Vector3 _planetCenter;
        private readonly Range<float> _temperatureRange;
        private readonly Range<float> _illuminationRange;
        private readonly Range<float> _humidityRange;
        private readonly Planet _planet;

        internal Weather(Planet planet, Range<float> temperatureRange, Range<float> illuminationRange, Range<float> humidityRange)
        {
            _planet = planet;
            _planetCenter = planet.Center;
            _temperatureRange = temperatureRange;
            _illuminationRange = illuminationRange;
            _humidityRange = humidityRange;
        }

        [NeedsRefactor(Needs.Check)]
        public void Get(Vector3 position, out Humidity parameter)
        {
            parameter = new Humidity(_humidityRange.Lerp(LandManagement.CubeMarching.CPU.Noise.Gradient(position / 200)));
        }

        public void Get(Vector3 position, out Illumination parameter)
        {
            const float objectRadius = 3f;
            Get(position, out parameter, objectRadius);
        }
        [NeedsRefactor("Optimize raycast", Needs.Refactor)]
        public void Get(Vector3 position, out Illumination parameter, float objectRadius)
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

            parameter = new Illumination(_illuminationRange.Lerp(timelessIlluminationLerp * timeLerp));
        }

        [NeedsRefactor("Add Noise", Needs.Refactor)]
        public void Get(Vector3 position, out Temperature parameter)
        {
            float positionLerp = 1 - Mathf.Abs(PlanetTransform.ToLatitude(position - _planetCenter)) / 90;
            float timeLerp = GetTimeLerp(position);
            const float positionSignificance = 0.8f;
            const float timeSignificance = 1 - positionSignificance;
            parameter = new Temperature(_temperatureRange.Lerp(positionLerp * positionSignificance + timeLerp * timeSignificance));
        }

        public bool TryGet<TWeatherParameter>(Vector3 position, out TWeatherParameter parameter) where TWeatherParameter : IWeatherParameter<TWeatherParameter>
        {
            if (this is IWeatherParameterProvider<TWeatherParameter> paramWeather)
            {
                paramWeather.Get(position, out parameter);
                return true;
            }
            parameter = default;
            return false;
        }

        private float GetTimeLerp(Vector3 position)
        {
            return 1 - Mathf.Abs(_planet.Time.Get(position).DayLerp * 2 - 1);
        }
    }


    public interface IWeatherParameter<TWeatherParameter> :
        ILerpable<TWeatherParameter>, System.IComparable<TWeatherParameter>
    { }

    public interface IWeatherParameterProvider<TWeatherParameter> where TWeatherParameter : IWeatherParameter<TWeatherParameter>
    {
        void Get(Vector3 position, out TWeatherParameter parameter);
    }

    [System.Serializable]
    [NeedsRefactor("Custom editor", Needs.MakeOwnFile)]
    public struct Humidity : IWeatherParameter<Humidity>
    {
        [SerializeField] private float _humidity;

        public Humidity(float humidity) => _humidity = humidity;

        public Humidity Average(Humidity other) => Lerp(other, 0.5f);
        public Humidity Lerp(Humidity finValue, float t) => new Humidity(Mathf.Lerp(_humidity, finValue._humidity, t));
        public int CompareTo(Humidity other) => _humidity.CompareTo(other._humidity);

        public override string ToString() => string.Format("{0:f2} %", _humidity);
    }

    [NeedsRefactor("Custom editor", Needs.MakeOwnFile)]
    public struct Illumination : IWeatherParameter<Illumination>
    {
        [SerializeField] private float _illumination;

        public Illumination(float value) => _illumination = value;

        public Illumination Average(Illumination other) => Lerp(other, 0.5f);
        public Illumination Lerp(Illumination finValue, float t) => new Illumination(Mathf.Lerp(_illumination, finValue._illumination, t));
        public int CompareTo(Illumination other) => _illumination.CompareTo(other._illumination);

        public override string ToString() => string.Format("{0:f2} lux", _illumination);
    }

    [NeedsRefactor("Custom editor", Needs.MakeOwnFile)]
    public struct Temperature : IWeatherParameter<Temperature>
    {
        [SerializeField] private float _temperature;

        public Temperature(float value) => _temperature = value;

        public Temperature Average(Temperature other) => Lerp(other, 0.5f);
        public Temperature Lerp(Temperature finValue, float t) => new Temperature(Mathf.Lerp(_temperature, finValue._temperature, t));
        public int CompareTo(Temperature other) => _temperature.CompareTo(other._temperature);

        public override string ToString() => string.Format("{0:f2} °C", _temperature);
    }
}