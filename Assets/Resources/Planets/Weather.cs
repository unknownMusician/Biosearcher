using Biosearcher.Common;
using Biosearcher.Planets.Orientation;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Planets
{
    [NeedsRefactor("Dynamic to Static (Need planet to have certain radius)")]
    public sealed class Weather
    {
        public static Weather Current => Planet.Current.Weather;

        private readonly Vector3 _planetCenter;
        private readonly WeatherRangeParameters _weatherParameters;
        private readonly Planet _planet;

        public Weather(Planet planet, WeatherRangeParameters weatherParameters)
        {
            _planet = planet;
            _planetCenter = planet.Center;
            _weatherParameters = weatherParameters;
        }

        [NeedsRefactor("Combine common", Needs.Optimization)]
        public WeatherParameters GetAll(Vector3 position)
        {
            return new WeatherParameters(GetHumidity(position), GetIllumination(position), GetTemperature(position));
        }

        [NeedsRefactor(Needs.Check)]
        public float GetHumidity(Vector3 position)
        {
            return _weatherParameters.HumidityRange.Lerp(LandManagement.CubeMarching.CPU.Noise.Gradient(position / 200));
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

            return _weatherParameters.IlluminationRange.Lerp(timelessIlluminationLerp * timeLerp);
        }

        [NeedsRefactor("Add Noise", Needs.Refactor)]
        public float GetTemperature(Vector3 position)
        {
            float positionLerp = 1 - Mathf.Abs(PlanetTransform.ToLatitude(position - _planetCenter)) / 90;
            float timeLerp = GetTimeLerp(position);
            const float positionSignificance = 0.8f;
            const float timeSignificance = 1 - positionSignificance;
            return _weatherParameters.TemperatureRange.Lerp(positionLerp * positionSignificance + timeLerp * timeSignificance);
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

    [System.Serializable]
    public struct WeatherRangeParameters
    {
        [SerializeField] private Range<float> _humidityRange;
        [SerializeField] private Range<float> _illuminationRange;
        [SerializeField] private Range<float> _temperatureRange;

        public Range<float> HumidityRange => _humidityRange;
        public Range<float> IlluminationRange => _illuminationRange;
        public Range<float> TemperatureRange => _temperatureRange;

        public WeatherRangeParameters(Range<float> humidityRange, Range<float> illuminationRange, Range<float> temperatureRange)
        {
            _humidityRange = humidityRange;
            _illuminationRange = illuminationRange;
            _temperatureRange = temperatureRange;
        }

        public bool Contains(WeatherParameters parameters)
        {
            return
                _humidityRange.Contains(parameters.Humidity) &&
                _illuminationRange.Contains(parameters.Illumination) &&
                _temperatureRange.Contains(parameters.Temperature);
        }
    }

    public struct WeatherParameters
    {
        [SerializeField] private float _humidity;
        [SerializeField] private float _illumination;
        [SerializeField] private float _temperature;

        public float Humidity => _humidity;
        public float Illumination => _illumination;
        public float Temperature => _temperature;

        public WeatherParameters(float humidity, float illumination, float temperature)
        {
            _humidity = humidity;
            _illumination = illumination;
            _temperature = temperature;
        }
    }

    [NeedsRefactor(Needs.MakeOwnFile, Needs.Refactor)]
    public class WeatherRegulator
    {
        #region Properties

        private float _preparedness;
        private readonly float _percentagePerSecond;
        private readonly bool _control;
        private float _currentValue;

        public float CurrentValue => _currentValue;

        #endregion

        public WeatherRegulator(float percentagePerSecond, bool control)
        {
            _percentagePerSecond = percentagePerSecond;
            _control = control;
        }

        #region Methods

        public void Reset(float outsideValue)
        {
            _preparedness = 0;
            _currentValue = outsideValue;
        }

        public void Regulate(float outsideValue, float goalValue, float efficiency)
        {
            if (!_control)
            {
                return;
            }

            if (Mathf.Approximately(efficiency, 1))
            {
                _preparedness += _percentagePerSecond;
            }
            else
            {
                _preparedness -= _percentagePerSecond * (1 - efficiency);
            }

            _preparedness = Mathf.Clamp01(_preparedness);
            _currentValue = Mathf.Lerp(outsideValue, goalValue, _preparedness);
        }

        #endregion
    }




    //public abstract class ResourceAccount<TResource>
    //{
    //    internal abstract TResource Resource { get; set; }

    //    public ResourceAccount()
    //    {
    //        Resource = default;
    //    }
    //}

    //public class ReceiverResourceAccount<TResource> : ResourceAccount<TResource>
    //    where TResource : Buildings.Resources.Interfaces.IResource<TResource>, new()
    //{
    //    private TResource _resource;
    //    internal override TResource Resource
    //    {
    //        get => _resourceGetter();
    //        set => _resourceSetter(value);
    //    }

    //    private System.Action<TResource> _resourceSetter;
    //    private System.Func<TResource> _resourceGetter;

    //    public ReceiverResourceAccount() : base()
    //    {
    //        _resourceSetter = res => _resource = res;
    //        _resourceGetter = () => _resource;
    //    }

    //    public ReceiverResourceAccount(FullResourceAccount<TResource> account)
    //    {
    //        _resourceSetter = res => account.Resource = res;
    //        _resourceGetter = () => account.Resource;
    //    }
    //}

    //public sealed class FullResourceAccount<TResource> : ResourceAccount<TResource>
    //    where TResource : Buildings.Resources.Interfaces.IResource<TResource>, new()
    //{
    //    public ReceiverResourceAccount<TResource> GettableAccount { get; }
    //    internal override TResource Resource { get; set; }

    //    public FullResourceAccount() : base()
    //    {
    //        GettableAccount = new ReceiverResourceAccount<TResource>();
    //    }

    //    public void TransferTo(ReceiverResourceAccount<TResource> account, TResource amount)
    //    {
    //        TResource deltaResource = MathB.Min(Resource, amount);

    //        Resource = Resource.Subtract(deltaResource);
    //        account.Resource = account.Resource.Add(deltaResource);
    //    }
    //}

    public static class MathB
    {
        public static TComparable Max<TComparable>(TComparable l, TComparable r) where TComparable : System.IComparable<TComparable>
        {
            return l.CompareTo(r) > 0 ? l : r;
        }

        public static TComparable Min<TComparable>(TComparable l, TComparable r) where TComparable : System.IComparable<TComparable>
        {
            return l.CompareTo(r) < 0 ? l : r;
        }
    }

    //public interface IWeatherParameter
    //{
    //    public float Value { get; }
    //}

    //public struct Humidity : IWeatherParameter
    //{
    //    public float Value { get; }
    //    public Humidity(float value)=>Value = value;
    //}
    //public struct Illumination : IWeatherParameter
    //{
    //    public float Value { get; }
    //    public Illumination(float value)=>Value = value;
    //}
    //public struct Temperature : IWeatherParameter
    //{
    //    public float Value { get; }

    //    public Temperature(float value)
    //    {
    //        Value = value;

    //        new CapsuleHumidityWeather().Get(default, out Temperature temp);
    //    }
    //}

    //public interface IWeather { }

    //public class CapsuleHumidityWeather : IWeather
    //{
    //    public void Get(Vector3 position, out Humidity parameter)
    //    {
    //        parameter = default;
    //    }
    //}

    //public static class WeatherExtensions
    //{
    //    public static void Get<TWeatherParameter>(this IWeather weather, Vector3 position, out TWeatherParameter parameter) where TWeatherParameter : IWeatherParameter
    //    {
    //        parameter = default;
    //    }
    //}




    //public class Weather2
    //{
    //    public virtual void Get(Vector3 position, out Humidity parameter)
    //    {
    //        parameter = default;
    //    }
    //    public virtual void Get(Vector3 position, out Illumination parameter)
    //    {
    //        parameter = default;
    //    }
    //    public virtual void Get(Vector3 position, out Temperature parameter)
    //    {
    //        parameter = default;
    //    }
    //}

    //public sealed class CapsuleHumidityWeather2 : Weather2
    //{
    //    public override void Get(Vector3 position, out Humidity parameter)
    //    {
    //        parameter = default;
    //    }
    //}
}