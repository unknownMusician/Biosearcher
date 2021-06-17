using Biosearcher.Buildings.Resources.Interfaces;

namespace Biosearcher.Buildings.Resources.Structs
{
    [System.Serializable]
    public struct Water : IResource<Water>
    {
        public float volume;
        
        public float Value
        {
            get => volume;
            set => volume = value;
        }

        public Water Add(Water a) => new Water {volume = volume + a.volume};
        public Water Subtract(Water a) => new Water {volume = volume - a.volume};
        public Water Multiply(Water a) => new Water {volume = volume * a.volume};
        public float Divide(Water a) => volume / a.volume;
        public int CompareTo(Water other) => volume.CompareTo(other.volume);

        public static Water operator +(Water w1, Water w2) => w1.Add(w2);
        public static Water operator -(Water w1, Water w2) => w1.Subtract(w2);
        public static Water operator *(Water w1, Water w2) => w1.Multiply(w2);
        public static float operator /(Water w1, Water w2) => w1.Divide(w2);
        public static bool operator >(Water w1, Water w2) => w1.CompareTo(w2) > 0;
        public static bool operator >=(Water w1, Water w2) => w1.CompareTo(w2) >= 0;
        public static bool operator <(Water w1, Water w2) => w1.CompareTo(w2) < 0;
        public static bool operator <=(Water w1, Water w2) => w1.CompareTo(w2) <= 0;
    }
}