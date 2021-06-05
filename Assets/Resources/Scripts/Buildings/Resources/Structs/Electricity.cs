using Biosearcher.Buildings.Resources.Interfaces;

namespace Biosearcher.Buildings.Resources.Structs
{
    [System.Serializable]
    public struct Electricity : IResource<Electricity>
    {
        public float energy;

        public float Value
        {
            get => energy;
            set => energy = value;
        }

        public int CompareTo(Electricity other) => energy.CompareTo(other.energy);

        public Electricity Add(Electricity a) => new Electricity {energy = energy + a.energy};
        public Electricity Subtract(Electricity a) => new Electricity {energy = energy - a.energy};
        public Electricity Multiply(Electricity a) => new Electricity {energy = energy * a.energy};
        public Electricity Divide(Electricity a) => new Electricity {energy = energy / a.energy};


        public static Electricity operator +(Electricity e1, Electricity e2) => e1.Add(e2);
        public static Electricity operator -(Electricity e1, Electricity e2) => e1.Subtract(e2);
        public static Electricity operator *(Electricity e1, Electricity e2) => e1.Multiply(e2);
        public static Electricity operator /(Electricity e1, Electricity e2) => e1.Divide(e2);
    }
}