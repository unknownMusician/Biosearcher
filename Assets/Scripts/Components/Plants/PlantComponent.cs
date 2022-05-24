using AreYouFruits.Common.ComponentGeneration;
using UnityEngine;

namespace Biosearcher.Plants
{
    public sealed class PlantComponent : AbstractComponent<Plant>
    {
#nullable disable
        [SerializeField] private Animator _animator;
#nullable enable

        protected override Plant Create() => new Plant(_animator);
    }
}
