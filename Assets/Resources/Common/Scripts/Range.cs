using Biosearcher.Refactoring;
using System;
using UnityEngine;

namespace Biosearcher.Common
{
    [Serializable]
    [NeedsRefactor("Custom Editor")]
    public struct Range<TRangeable> : IRange<TRangeable> where TRangeable : IComparable<TRangeable>
    {
        [SerializeField] private TRangeable _min;
        [SerializeField] private TRangeable _max;

        public TRangeable Min => _min;
        public TRangeable Max => _max;

        public Range(TRangeable min, TRangeable max)
        {
            _min = min;
            _max = max;
        }

        public bool Contains(TRangeable value)
        {
            return _min.CompareTo(value) <= 0 && value.CompareTo(_max) <= 0;
        }
    }

    [NeedsRefactor(Needs.Remove)]
    public interface IRange<TRangeable> where TRangeable : IComparable<TRangeable>
    {
        public TRangeable Min { get; }
        public TRangeable Max { get; }

        public bool Contains(TRangeable value);
    }

    [NeedsRefactor("Custom Editor", Needs.Remove)]
    public struct FloatRange : IRange<float>
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        public float Min => _min;
        public float Max => _max;

        public FloatRange(float min, float max)
        {
            _min = min;
            _max = max;
        }

        public bool Contains(float value)
        {
            return _min.CompareTo(value) <= 0 && value.CompareTo(_max) <= 0;
        }

        public static explicit operator FloatRange(Range<float> range) => new FloatRange(range.Min, range.Max);
        public static explicit operator Range<float>(FloatRange range) => new Range<float>(range.Min, range.Max);
    }

    [NeedsRefactor(Needs.Remove)]
    public static class RangeExtensions
    {
        public static float Average(this Range<float> range)
        {
            return (range.Min + range.Max) / 2;
        }

        public static float Lerp(this Range<float> range, float t)
        {
            return Mathf.Lerp(range.Min, range.Max, t);
        }
    }
}