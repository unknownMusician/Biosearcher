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
        [SerializeField] private ElectricityNetwork electricityNetwork;

        private void Start()
        {
            electricityNetwork.producers.Add(coalGenerator);
            electricityNetwork.receivers.Add(greenHouse);
        }
    }
}
