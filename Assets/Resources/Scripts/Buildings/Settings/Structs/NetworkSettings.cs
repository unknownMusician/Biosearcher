using System.Collections;
using UnityEngine;

namespace Biosearcher.Buildings.Settings.Structs
{
    [System.Serializable]
    public struct NetworkSettings
    {
        [SerializeField] private float _maxConnectRadius;
        [SerializeField] private float _cyclesPerSecond;
        
        public float MaxConnectRadius => _maxConnectRadius;
        public float CyclesPerSecond => _cyclesPerSecond;
    }
}