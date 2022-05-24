using System;
using UnityEngine;

namespace Biosearcher.Level
{
    public sealed class ColliderListener : MonoBehaviour
    {
        public event Action<Collision>? OnCollide;
        public event Action<Collider>? OnTrigger;

        private void OnCollisionEnter(Collision collision) => OnCollide?.Invoke(collision);
        private void OnTriggerEnter(Collider collider) => OnTrigger?.Invoke(collider);
    }
}
