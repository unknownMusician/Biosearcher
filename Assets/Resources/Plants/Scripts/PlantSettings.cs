using Biosearcher.Common;
using Biosearcher.Planets;
using UnityEngine;

namespace Biosearcher.Plants
{
    [CreateAssetMenu(fileName = "Plant Settings", menuName = "Plant Settings", order = 53)]
    public class PlantSettings : ScriptableObject
    {
        [SerializeField] public int id;
        [SerializeField] public string plantName;
        
        [SerializeField] public float timeToGrow;
        [SerializeField] public float timeToCorrupt;
        
        [SerializeField] public Range<Humidity> humidityRange;
        [SerializeField] public Range<Illumination> illuminationRange;
        [SerializeField] public Range<Temperature> temperatureRange;
    }
}
