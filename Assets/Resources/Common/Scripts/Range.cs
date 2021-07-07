using Biosearcher.Common.Interfaces;
using Biosearcher.Refactoring;
using System;
using UnityEngine;

namespace Biosearcher.Common
{
    [Serializable]
    [NeedsRefactor("Custom Editor")]
    public struct Range<T>
    {
        [SerializeField] private T _min;
        [SerializeField] private T _max;

        public T Min => _min;
        public T Max => _max;

        public Range(T min, T max)
        {
            _min = min;
            _max = max;
        }
    }

    [NeedsRefactor(Needs.MakeOwnFile)]
    public static class RangeExtensions
    {
        public static bool Contains<TRangeable>(this Range<TRangeable> range, TRangeable value)
            where TRangeable : IComparable<TRangeable>
        {
            return range.Min.CompareTo(value) <= 0 && value.CompareTo(range.Max) <= 0;
        }

        public static TRangeable Lerp<TRangeable>(this Range<TRangeable> range, float t)
            where TRangeable : ILerpable<TRangeable>
        {
            return range.Min.Lerp(range.Max, t);
        }

        public static TRangeable Average<TRangeable>(this Range<TRangeable> range)
            where TRangeable : IAverageable<TRangeable>
        {
            return range.Min.Average(range.Max);
        }


        public static float Average(this Range<float> range) => range.Min.Average(range.Max);
        public static float Lerp(this Range<float> range, float t) => Mathf.Lerp(range.Min, range.Max, t);
        public static Color Lerp(this Range<Color> range, float t) => Color.Lerp(range.Min, range.Max, t);
        public static Vector2 Lerp(this Range<Vector2> range, float t) => Vector2.Lerp(range.Min, range.Max, t);
        public static Vector3 Lerp(this Range<Vector3> range, float t) => Vector3.Lerp(range.Min, range.Max, t);
        public static Vector3 Slerp(this Range<Vector3> range, float t) => Vector3.Slerp(range.Min, range.Max, t);
        public static Vector4 Lerp(this Range<Vector4> range, float t) => Vector4.Lerp(range.Min, range.Max, t);
        public static Quaternion Lerp(this Range<Quaternion> range, float t) => Quaternion.Lerp(range.Min, range.Max, t);
        public static Quaternion Slerp(this Range<Quaternion> range, float t) => Quaternion.Slerp(range.Min, range.Max, t);
    }
}