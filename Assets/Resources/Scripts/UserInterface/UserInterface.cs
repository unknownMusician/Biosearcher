using System;
using System.Collections;
using System.Collections.Generic;
using Biosearcher.Weather;
using UnityEngine;

namespace Biosearcher.UserInterface
{
    public class UserInterface : MonoBehaviour
    {
        private WeatherController _weatherController;

        private void Start()
        {
            _weatherController = WeatherController.Instance;
        }
    }
}
