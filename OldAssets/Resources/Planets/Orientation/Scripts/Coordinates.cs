﻿using Biosearcher.Refactoring;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Biosearcher.Planets.Orientation
{
    /// <summary>
    /// (Height - r, Latitude - θ, Longitude - φ)
    /// </summary>
    [System.Serializable]
    [NeedsRefactor("Create Custom Editor")]
    public struct Coordinates : System.IEquatable<Coordinates>
    {
        public static Coordinates Up => new Coordinates(1, 0, 0);
        public static Coordinates Down => new Coordinates(-1, 0, 0);
        public static Coordinates Zero => new Coordinates(0, 0, 0);

        [NeedsRefactor("maybe use double")]
        public float height;
        [NeedsRefactor("maybe use double")]
        public float latitude;
        [NeedsRefactor("maybe use double")]
        public float longitude;

        public Coordinates(float height, float latitude, float longitude)
        {
            this.height = height;
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public static bool operator ==(Coordinates c1, Coordinates c2) => c1.Equals(c2);
        public static bool operator !=(Coordinates c1, Coordinates c2) => !c1.Equals(c2);

        public static Coordinates operator +(Coordinates c1, Coordinates c2)
        {
            return new Coordinates(c1.height + c2.height, c1.latitude + c2.latitude, c1.longitude + c2.longitude);
        }
        public static Coordinates operator -(Coordinates c1, Coordinates c2)
        {
            return new Coordinates(c1.height - c2.height, c1.latitude - c2.latitude, c1.longitude - c2.longitude);
        }

        public bool Equals(Coordinates other)
        {
            return height == other.height && latitude == other.latitude && longitude == other.longitude;
        }
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode()
        {
            int hashCode = -1567576899;
            hashCode = hashCode * -1521134295 + height.GetHashCode();
            hashCode = hashCode * -1521134295 + latitude.GetHashCode();
            hashCode = hashCode * -1521134295 + longitude.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"Coordinates{{r={height}, θ={latitude}, φ={longitude}}}";
    }
}