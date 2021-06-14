using UnityEngine;

namespace Biosearcher.Plants
{
    [System.Serializable]
    public struct Range
    {
        [SerializeField] private float leftBorder;
        [SerializeField] private float rightBorder;

        public float LeftBorder => leftBorder;
        public float RightBorder => rightBorder;
        public float Average => (LeftBorder + RightBorder) / 2;

        public bool Contains(float value)
        {
            return leftBorder <= value && value <= rightBorder;
        }
    }
}
