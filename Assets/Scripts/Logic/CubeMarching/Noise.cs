using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Biosearcher.CubeMarching
{
    public static class Noise
    {
        public static uint Hash(uint seed)
        {
            seed += 654657895;
            seed *= 7567;
            seed += 789543263;
            seed %= 59875;
            seed *= 89563;
            seed += 21987955;
            seed %= 2894591;
            seed *= 25084677;
            seed += 34872045;

            //int hash = HashCode.Combine(seed);
            //seed = UnsafeUtility.As<int, uint>(ref hash);
            
            return seed;
        }

        public static float Generate(uint seed)
        {
            uint hash = Hash(seed);
            
            return (float)((double)hash / uint.MaxValue);
        }

        public static float Simple(float x) => Generate(UnsafeUtility.As<float, uint>(ref x));

        public static float Simple(float x, float y)
        {
            uint seed = UnsafeUtility.As<float, uint>(ref x);
            seed += Hash(UnsafeUtility.As<float, uint>(ref y));

            return Generate(seed);
        }

        public static float Simple(float x, float y, float z)
        {
            uint seed = UnsafeUtility.As<float, uint>(ref x);
            seed += Hash(UnsafeUtility.As<float, uint>(ref y));
            seed += Hash(Hash(UnsafeUtility.As<float, uint>(ref z)));

            return Generate(seed);
        }

        public static float GetFraction(float x, float scale)
        {
            float fraction = x % scale;

            if (fraction < 0)
            {
                fraction = scale + fraction;
            }

            return fraction;
        }

        public static float Smooth(float t) => t * t * t * (t * (t * 6 - 15) + 10);

        public static float SimpleGradient(float x, float scale)
        {
            float fraction = GetFraction(x, scale);
            float floor = x - fraction;

            float p0 = Simple(floor);
            float p1 = Simple(floor + 1);

            return Mathf.LerpUnclamped(p0, p1, Smooth(fraction / scale));
        }

        public static float SimpleGradient(float x, float y, float scale)
        {
            float xFraction = GetFraction(x, scale);
            float yFraction = GetFraction(y, scale);
            float xFloor = x - xFraction;
            float yFloor = y - yFraction;
            float xCeil = xFloor + scale;
            float yCeil = yFloor + scale;

            float p00 = Simple(xFloor, yFloor);
            float p10 = Simple(xCeil, yFloor);
            float p01 = Simple(xFloor, yCeil);
            float p11 = Simple(xCeil, yCeil);

            float xLerp = Smooth(xFraction / scale);
            float yLerp = Smooth(yFraction / scale);

            return Mathf.LerpUnclamped(
                Mathf.LerpUnclamped(p00, p10, xLerp), 
                Mathf.LerpUnclamped(p01, p11, xLerp), 
                yLerp
            );
        }

        public static float SimpleGradient(float x, float y, float z, float scale)
        {
            float xFraction = GetFraction(x, scale);
            float yFraction = GetFraction(y, scale);
            float zFraction = GetFraction(z, scale);
            float xFloor = x - xFraction;
            float yFloor = y - yFraction;
            float zFloor = z - zFraction;
            float xCeil = xFloor + scale;
            float yCeil = yFloor + scale;
            float zCeil = zFloor + scale;

            float p000 = Simple(xFloor, yFloor, zFloor);
            float p100 = Simple(xCeil, yFloor, zFloor);
            float p010 = Simple(xFloor, yCeil, zFloor);
            float p110 = Simple(xCeil, yCeil, zFloor);
            float p001 = Simple(xFloor, yFloor, zCeil);
            float p101 = Simple(xCeil, yFloor, zCeil);
            float p011 = Simple(xFloor, yCeil, zCeil);
            float p111 = Simple(xCeil, yCeil, zCeil);
            
            float xLerp = Smooth(xFraction / scale);
            float yLerp = Smooth(yFraction / scale);
            float zLerp = Smooth(zFraction / scale);

            return Mathf.LerpUnclamped(
                Mathf.LerpUnclamped(
                    Mathf.LerpUnclamped(p000, p100, xLerp), 
                    Mathf.LerpUnclamped(p010, p110, xLerp), 
                    yLerp
                ),
                Mathf.LerpUnclamped(
                    Mathf.LerpUnclamped(p001, p101, xLerp), 
                    Mathf.LerpUnclamped(p011, p111, xLerp), 
                    yLerp
                ),
                zLerp
            );
        }
    }
}
