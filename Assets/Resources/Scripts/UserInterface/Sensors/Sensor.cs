using Biosearcher.Weather;
using TMPro;
using UnityEngine;

namespace Biosearcher.UserInterface.Sensors
{
    public abstract class Sensor : MonoBehaviour
    {
        [SerializeField] protected TMP_Text text;
        
        protected WeatherController weatherController;
        protected Transform player;

        public void Initialize(WeatherController weatherController, Transform player)
        {
            this.weatherController = weatherController;
            this.player = player;
        }
        
        public abstract void UpdateData();
    }
}