using AreYouFruits.Common.ComponentGeneration;
using UnityEngine;

namespace Biosearcher.Plants
{
    public sealed class GreenHouseComponent : AbstractComponent<GreenHouse>
    {
#nullable disable
        [SerializeField] private Vector3 _alignLocalPosition;
#nullable enable

        protected override GreenHouse Create() => new GreenHouse(transform, _alignLocalPosition);
    }
}
