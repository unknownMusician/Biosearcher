using Biosearcher.Buildings.Resources.Interfaces;
using UnityEngine;

namespace Biosearcher.Buildings.Resources.Structs
{
    [System.Serializable]
    public struct Water : IResource<Water>
    {
        [SerializeField] private float _volume;

        public Water(float volume) => _volume = volume;

        public Water Add(Water a) => new Water(_volume + a._volume);
        public Water Subtract(Water s) => new Water(_volume - s._volume);
        public Water Multiply(float m) => new Water(_volume * m);
        public Water Divide(float d) => new Water(_volume / d);
        public float Divide(Water d) => _volume / d._volume;
        public int CompareTo(Water other) => _volume.CompareTo(other._volume);
        public Water Average(Water other) => new Water((_volume + other._volume) * 0.5f);

        public static Water operator +(Water w1, Water w2) => w1.Add(w2);
        public static Water operator -(Water w1, Water w2) => w1.Subtract(w2);
        public static Water operator *(Water w1, float f2) => w1.Multiply(f2);
        public static Water operator /(Water w1, float f2) => w1.Divide(f2);
        public static float operator /(Water w1, Water w2) => w1.Divide(w2);
        public static bool operator >(Water w1, Water w2) => w1.CompareTo(w2) > 0;
        public static bool operator >=(Water w1, Water w2) => w1.CompareTo(w2) >= 0;
        public static bool operator <(Water w1, Water w2) => w1.CompareTo(w2) < 0;
        public static bool operator <=(Water w1, Water w2) => w1.CompareTo(w2) <= 0;
    }
}