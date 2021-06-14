using Biosearcher.Buildings;
using Biosearcher.Buildings.Generators;
using Biosearcher.Buildings.Network;
using UnityEngine;

namespace Biosearcher.Plants
{
    public class TestGreenHouse : MonoBehaviour
    {
        [SerializeField] private CoalGenerator coalGenerator;
        [SerializeField] private GreenHouse greenHouse;
        [SerializeField] private TestEternalWaterGenerator testEternalWaterGenerator;
        [Space]
        [SerializeField] private ElectricityNetwork electricityNetwork;
        [SerializeField] private WaterNetwork waterNetwork;

        private void Start()
        {
            electricityNetwork.producers.Add(coalGenerator);
            electricityNetwork.receivers.Add(greenHouse);
            
            waterNetwork.producers.Add(testEternalWaterGenerator);
            waterNetwork.receivers.Add(greenHouse);
        }
    }
}
