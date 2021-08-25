using Biosearcher.Buildings.Resources.Interfaces;
using UnityEngine;

namespace Biosearcher.Buildings.Resources.Structs
{
    [System.Serializable]
    public struct Electricity : IResource<Electricity>
    {
        [SerializeField] private float _energy;

        public Electricity(float energy) => _energy = energy;

        public Electricity Add(Electricity a) => new Electricity(_energy + a._energy);
        public Electricity Subtract(Electricity s) => new Electricity(_energy - s._energy);
        public Electricity Multiply(float m) => new Electricity(_energy * m);
        public Electricity Divide(float d) => new Electricity(_energy / d);
        public float Divide(Electricity d) => _energy / d._energy;
        public int CompareTo(Electricity other) => _energy.CompareTo(other._energy);
        public Electricity Average(Electricity other) => new Electricity((_energy + other._energy) * 0.5f);

        public static Electricity operator +(Electricity e1, Electricity e2) => e1.Add(e2);
        public static Electricity operator -(Electricity e1, Electricity e2) => e1.Subtract(e2);
        public static Electricity operator *(Electricity e1, float f2) => e1.Multiply(f2);
        public static Electricity operator /(Electricity e1, float f2) => e1.Divide(f2);
        public static float operator /(Electricity e1, Electricity e2) => e1.Divide(e2);
        public static bool operator >(Electricity e1, Electricity e2) => e1.CompareTo(e2) > 0;
        public static bool operator >=(Electricity e1, Electricity e2) => e1.CompareTo(e2) >= 0;
        public static bool operator <(Electricity e1, Electricity e2) => e1.CompareTo(e2) < 0;
        public static bool operator <=(Electricity e1, Electricity e2) => e1.CompareTo(e2) <= 0;
    }
}