using Biosearcher.Planets;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Weather
{
    [NeedsRefactor("Add generics", Needs.Remove)]
    public static class WeatherController
    {
        [NeedsRefactor(Needs.Remove)]
        public static float GetHumidity(Vector3 position)
        {
            return Planets.Weather.Current.GetHumidity(position);
        }
        [NeedsRefactor(Needs.Remove)]
        public static float GetIllumination(Vector3 position)
        {
            return Planets.Weather.Current.GetIllumination(position);
        }
        [NeedsRefactor(Needs.Remove)]
        public static float GetTemperature(Vector3 position)
        {
            return Planets.Weather.Current.GetTemperature(position);
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
