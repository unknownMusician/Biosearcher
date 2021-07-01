using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Plants
{
    [System.Serializable]
    [NeedsRefactor("Make less specific (there are calls from other namespaces)")]
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
