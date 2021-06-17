namespace Biosearcher.UserInterface.Sensors
{
    public class IlluminationSensor : Sensor
    {
        public override void UpdateData()
        {
            text.text = $"{weatherController.GetIllumination(player.position)}";
        }
    }
}
