namespace Biosearcher.UserInterface.Sensors
{
    public class HumiditySensor : Sensor
    {
        public override void UpdateData()
        {
            text.text = $"{weatherController.GetHumidity(player.position)}";
        }
    }
}
