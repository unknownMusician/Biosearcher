using Biosearcher.Refactoring;
using System;
using UnityEngine;

namespace Biosearcher.Common
{
    [Serializable]
    public struct Range<TRangeable> where TRangeable : IComparable<TRangeable>
    {
        [SerializeField] private TRangeable _min;
        [SerializeField] private TRangeable _max;
        [SerializeField] private bool _isBounded;

        public TRangeable Min => _min;
        public TRangeable Max => _max;

        public Range(TRangeable min, TRangeable max)
        {
            _min = min;
            _max = max;
            _isBounded = true;
        }

        public bool Contains(TRangeable value)
        {
            return !_isBounded || (_min.CompareTo(value) <= 0 && value.CompareTo(_max) <= 0);
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