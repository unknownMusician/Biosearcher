using Biosearcher.Player.Interactions.Default;
using JetBrains.Annotations;
using UnityEngine;

namespace Biosearcher.Plants
{
    public class Seed : Insertable
    {
        public readonly GameObject GameObject;
        public readonly PlantInfo PlantInfo;
        
        public Seed(Rigidbody rigidbody, Collider collider, GameObject gameObject, PlantInfo plantInfo) : base(rigidbody, collider)
        {
            GameObject = gameObject;
            PlantInfo = plantInfo;
        }
    }
}
