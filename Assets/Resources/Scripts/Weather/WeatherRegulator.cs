using UnityEngine;

namespace Biosearcher.Plants
{
    public class WeatherRegulator<TParameter>
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
            if (Mathf.Approximately(efficiency, 1))
            {
                _preparedness += _percentagePerSecond;
            }
            else
            {
                _preparedness -= _percentagePerSecond * (1 - efficiency);
            }
            _preparedness = Mathf.Clamp01(_preparedness);
            
            if (_control)
            {
                _currentValue = Mathf.Lerp(outsideValue, goalValue, _preparedness);
            }
        }

        #endregion
    }
}
