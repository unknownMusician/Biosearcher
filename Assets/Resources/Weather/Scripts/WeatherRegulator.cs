using UnityEngine;

namespace Biosearcher.Weather
{
    public class WeatherRegulator<TWeatherParameter> where TWeatherParameter : Planets.IWeatherParameter<TWeatherParameter>
    {
        #region Properties

        private readonly float _percentagePerSecond;
        private readonly bool _control;
        private TWeatherParameter _goalParameter;

        private float _localWeatherLerp;

        public TWeatherParameter GetCurrentValue(Vector3 position)
        {
            if(!Planets.Weather.Current.TryGet(position, out TWeatherParameter outsideParameter))
            {
                throw new System.ArgumentException($"Parameter {outsideParameter.GetType().Name} is not supported by Weather.");
            }

            return outsideParameter.Lerp(_goalParameter, _localWeatherLerp);
        }

        #endregion

        public WeatherRegulator(float percentagePerSecond, bool control)
        {
            _percentagePerSecond = percentagePerSecond;
            _control = control;
            _localWeatherLerp = 0;
        }

        #region Methods

        public void Reset(TWeatherParameter goalParameter)
        {
            _localWeatherLerp = 0;
            _goalParameter = goalParameter;
        }

        public void Regulate(float efficiency)
        {
            if (!_control)
            {
                return;
            }

            if (Mathf.Approximately(efficiency, 1))
            {
                _localWeatherLerp += _percentagePerSecond;
            }
            else
            {
                _localWeatherLerp -= _percentagePerSecond * (1 - efficiency);
            }

            _localWeatherLerp = Mathf.Clamp01(_localWeatherLerp);
        }

        #endregion
    }
}