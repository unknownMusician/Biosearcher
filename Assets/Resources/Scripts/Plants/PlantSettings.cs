using System;
using UnityEngine;

namespace Biosearcher.Plants
{
    [CreateAssetMenu(fileName = "Plant Settings", menuName = "Plant Settings", order = 53)]
    public class PlantSettings : ScriptableObject
    {
        [SerializeField] public int id;
        [SerializeField] public string name;
        [SerializeField] public float timeToGrow;
        [SerializeField] public Range temperatureRange;
        [SerializeField] public Range illuminationRange;
        [SerializeField] public Range humidityRange;
    }
}