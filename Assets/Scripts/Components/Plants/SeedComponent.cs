using AreYouFruits.Common.ComponentGeneration;
using UnityEngine;

namespace Biosearcher.Plants
{
    public sealed class SeedComponent : AbstractComponent<Seed>
    {
#nullable disable
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        
        [SerializeField] private SerializedInterface<IComponent<PlantInfo>> _plantInfo;
#nullable enable

        protected override Seed Create() => new Seed(_rigidbody, _collider, gameObject, _plantInfo.GetHeldItem());
    }
}
