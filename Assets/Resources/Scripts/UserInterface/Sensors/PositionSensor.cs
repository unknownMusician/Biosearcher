using System;
using Biosearcher.Planet.Orientation;
using UnityEngine;

namespace Biosearcher.UserInterface.Sensors
{
    public class PositionSensor : Sensor
    {
        public override void UpdateData()
        {
            var playerPlanetTransform = player.GetComponent<PlanetTransform>().Coordinates;
            text.text = $"h = {Math.Round(playerPlanetTransform.height, 2)}, " +
                        $"lat = {Math.Round(playerPlanetTransform.latitude, 2)}, " +
                        $"lon = {Math.Round(playerPlanetTransform.longitude, 2)}";
        }
    }
}
