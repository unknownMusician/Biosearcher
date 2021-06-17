using Biosearcher.UserInterface.Sensors;
using System.Collections;
using System.Collections.Generic;
using Biosearcher.Weather;
using UnityEngine;

namespace Biosearcher.UserInterface
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] private float sensorsUpdateDelay;
        [Space]
        [SerializeField] private RectTransform sensorsContainer;
        [SerializeField] private Transform player;
        [Space]
        [SerializeField] private bool showPosition;
        [SerializeField] private bool showHumidity;
        [SerializeField] private bool showIllumination;
        [SerializeField] private bool showTemperature;
        [Space] 
        [SerializeField] private GameObject positionSensor;
        [SerializeField] private GameObject humiditySensor;
        [SerializeField] private GameObject illuminationSensor;
        [SerializeField] private GameObject temperatureSensor;

        private List<Sensor> _sensors;
        private WeatherController _weatherController;

        private void Awake()
        {
            _sensors = new List<Sensor>();
        }
        private void Start()
        {
            _weatherController = WeatherController.Instance;
            
            if (showPosition) InstallPositionSensor();
            if (showHumidity) InstallHumiditySensor();
            if (showIllumination) InstallIlluminationSensor();
            if (showTemperature) InstallTemperatureSensor();
            
            StartCoroutine(SensorsCycle());
        }

        private void InstallSensor(GameObject prefab)
        {
            var newSizeDelta = sensorsContainer.sizeDelta + new Vector2(0, 30);
            sensorsContainer.sizeDelta = newSizeDelta;

            var sensorObject = Instantiate(prefab, Vector3.zero, Quaternion.identity, sensorsContainer);
            var sensorComponent = sensorObject.GetComponent<Sensor>();
            sensorComponent.Initialize(_weatherController, player);
            _sensors.Add(sensorComponent);
        }
        public void InstallPositionSensor()
        {
            InstallSensor(positionSensor);
        }
        public void InstallHumiditySensor()
        {
            InstallSensor(humiditySensor);
        }
        public void InstallIlluminationSensor()
        {
            InstallSensor(illuminationSensor);
        }
        public void InstallTemperatureSensor()
        {
            InstallSensor(temperatureSensor);
        }
        
        private void UpdateSensors()
        {
            foreach (var sensor in _sensors)
            {
                sensor.UpdateData();
            }
        }
        private IEnumerator SensorsCycle()
        {
            while (true)
            {
                UpdateSensors();
                yield return new WaitForSeconds(sensorsUpdateDelay);
            }
        }
    }
}
