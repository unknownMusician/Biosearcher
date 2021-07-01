using Biosearcher.Common;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Planets
{
    public sealed class Time
    {
        public static Time Current => Planet.Current.Time;

        private readonly MainStarNew _mainStar;
        private readonly Vector3 _planetCenter;
        private readonly Vector3 _planetRotationAxis;
        private readonly Planet _planet;

        public Time(Planet planet)
        {
            _planet = planet;
            _mainStar = planet.Star;
            _planetCenter = planet.Center;
            _planetRotationAxis = planet.RotationAxis;
        }

        [NeedsRefactor]
        public Moment Get(Vector3 position)
        {
            Vector3 starLocalPosition = _mainStar.RotationStartingVector - _planetCenter;
            Vector3 getterLocalPosition = position - _planetCenter;

            float setterStarAngle = -Vector3.SignedAngle(
                Vector3.ProjectOnPlane(starLocalPosition, _planetRotationAxis),
                Vector3.ProjectOnPlane(getterLocalPosition, _planetRotationAxis),
                _planetRotationAxis);

            setterStarAngle += 180 + _mainStar.RotationAngle;
            setterStarAngle.MakeCycleDegrees();

            return Moment.Create(setterStarAngle / 360);
        }

        [NeedsRefactor("Implement custom dayLength")]
        public readonly struct Moment
        {
            public readonly int Hours;
            public readonly int Minutes;
            public readonly int Seconds;
            public readonly float DayLerp;

            private Moment(int hours, int minutes, int seconds, float dayLerp)
            {
                Hours = hours;
                Minutes = minutes;
                Seconds = seconds;
                DayLerp = dayLerp;
            }

            internal static Moment Create(float dayLerp)
            {
                int hours = Mathf.FloorToInt(dayLerp * 24);
                int minutes = Mathf.FloorToInt(dayLerp * (24 * 60)) % 60;
                int seconds = Mathf.FloorToInt(dayLerp * (24 * 60 * 60)) % 60;

                return new Moment(hours, minutes, seconds, dayLerp);
            }

            internal static Moment Create(int hours, int minutes, int seconds)
            {
                float dayLerp = hours * (1 / 24f) + minutes * (1 / (24f * 60f)) + seconds * (1 / (24f * 60f * 60f));
                return new Moment(hours, minutes, seconds, dayLerp);
            }

            public override string ToString()
            {
                return string.Format("{0:d2}:{1:d2}:{2:d2} ({2:d2} of a day)", Hours, Minutes, Seconds, DayLerp);
            }
        }
    }
}