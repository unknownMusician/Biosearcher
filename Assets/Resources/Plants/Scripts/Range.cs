using UnityEngine;

namespace Biosearcher.Plants
{
    [System.Serializable]
    public struct Range
    {
        [SerializeField] private float min;
        [SerializeField] private float max;

        public float Min => min;
        public float Max => max;
        public float Average => (Min + Max) / 2;

        public bool Contains(float value)
        {
            return min <= value && value <= max;
        }
    }
}
