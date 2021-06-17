namespace Biosearcher.UserInterface.Sensors
{
    public class TemperatureSensor : Sensor
    {
        public override void UpdateData()
        {
            text.text = $"{weatherController.GetTemperature(player.position)}";
        }
    }
}
