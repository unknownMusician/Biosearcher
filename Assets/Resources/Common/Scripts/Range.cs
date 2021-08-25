using Biosearcher.Common.Interfaces;
using Biosearcher.Refactoring;
using System;
using UnityEngine;

namespace Biosearcher.Common
{
    [Serializable]
    public struct Range<TRangeable>
    {
        [SerializeField] private TRangeable _min;
        [SerializeField] private TRangeable _max;
        [SerializeField] private bool _isBounded;

        public TRangeable Min => _min;
        public TRangeable Max => _max;
        public bool IsBounded => _isBounded;

        public Range(TRangeable min, TRangeable max)
        {
            _min = min;
            _max = max;
            _isBounded = true;
        }

        public static implicit operator Range<TRangeable>((TRangeable min, TRangeable max) tuple)
        {
            return new Range<TRangeable>(tuple.min, tuple.max);
        }
    

        public void Deconstruct(out TRangeable min, out TRangeable max)
        {
            min = _min;
            max = _max;
        }
    }

    [NeedsRefactor(Needs.MakeOwnFile)]
    public static class RangeExtensions
    {
        public static bool Contains<TComparable>(this Range<TComparable> range, TComparable value)
            where TComparable : IComparable<TComparable>
        {
            return range.Min.CompareTo(value) <= 0 && value.CompareTo(range.Max) <= 0;
        }

        public static TLerpable Lerp<TLerpable>(this Range<TLerpable> range, float t)
            where TLerpable : ILerpable<TLerpable>
        {
            return range.Min.Lerp(range.Max, t);
        }

        public static TAverageable Average<TAverageable>(this Range<TAverageable> range)
            where TAverageable : IAverageable<TAverageable>
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