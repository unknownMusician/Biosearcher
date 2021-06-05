using Biosearcher.Buildings.Generators;
using UnityEngine;

namespace Biosearcher.Buildings.Network
{
    public class NetworkContentTest : MonoBehaviour
    {
        [SerializeField] private ElectricityNetwork network;
        [SerializeField] private CoalGenerator coalGenerator;
        [SerializeField] private GreenHouse greenHouse;

        private void Start()
        {
            network.producers.Add(coalGenerator);
            network.receivers.Add(greenHouse);
        }
    }
}
