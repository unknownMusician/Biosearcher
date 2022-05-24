using AreYouFruits.Common.ComponentGeneration;
using UnityEngine;

namespace Biosearcher.Plants
{
    [CreateAssetMenu(fileName = "Default Plant", menuName = "Plant Settings")]
    public sealed class PlantSettings : InfoHolder<PlantInfo> { }
}
