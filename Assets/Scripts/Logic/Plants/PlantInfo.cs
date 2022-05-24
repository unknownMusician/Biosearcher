using System;
using UnityEngine;

namespace Biosearcher.Plants
{
    [Serializable]
    public struct PlantInfo
    {
        public string Name;
        public GameObject PlantPrefab;
        public float GrowthTime;
        public int Score;
    }
}
