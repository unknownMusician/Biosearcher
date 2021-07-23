using Biosearcher.Common;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Planets
{
    public sealed class Planet
    {
        public static Planet Current { get; internal set; }

        public MainStar Star => MainStar.Instance;
        [NeedsRefactor]
        public Vector3 Center => Vector3.zero;
        public Vector3 RotationAxis { get; }

        public Time Time { get; private set; }
        public Weather Weather { get; private set; }

        private Planet(Vector3 rotationAxis)
        {
            RotationAxis = rotationAxis;
        }

        private void Initialize(Time time, Weather weather)
        {
            Time = time;
            Weather = weather;
        }

        internal static Planet Create(PlanetSettings settings)
            => Create(settings.rotationAxis, settings.isCurrent, settings.temperatureRange, settings.illuminationRange, settings.humidityRange);
        internal static Planet Create(Vector3 rotationAxis, bool setAsCurrent, Range<float> temperatureRange, Range<float> illuminationRange, Range<float> humidityRange)
        {
            var planet = new Planet(rotationAxis);

            var time = new Time(planet);
            var weather = new Weather(planet, temperatureRange, illuminationRange, humidityRange);

            planet.Initialize(time, weather);

            if (setAsCurrent)
            {
                Current = planet;
            }

            return planet;
        }
    }
}