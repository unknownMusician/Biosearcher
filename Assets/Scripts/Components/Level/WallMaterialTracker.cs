using UnityEngine;

namespace Biosearcher.Level
{
    [ExecuteAlways]
    public sealed class WallMaterialTracker : MonoBehaviour
    {
        [SerializeField] private Material _material;

        private void Update() => _material.SetVector("_PlayerPosition", transform.position);
    }
}