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
        public float Radius { get; }

        public Time Time { get; private set; }
        public Weather Weather { get; private set; }

        private Planet(Vector3 rotationAxis, float radius)
        {
            RotationAxis = rotationAxis;
            Radius = radius;
        }

        private void Initialize(Time time, Weather weather)
        {
            Time = time;
            Weather = weather;
        }

        internal static Planet Create(PlanetSettings settings)
            => Create(settings.RotationAxis, settings.IsCurrent, settings.WeatherParameters, settings.Radius);
        internal static Planet Create(Vector3 rotationAxis, bool setAsCurrent, WeatherRangeParameters weatherParameters, float radius)
        {
            var planet = new Planet(rotationAxis, radius);

            var time = new Time(planet);
            var weather = new Weather(planet, weatherParameters);

            planet.Initialize(time, weather);

            if (setAsCurrent)
            {
                Current = planet;
            }

            return planet;
        }
    }
}