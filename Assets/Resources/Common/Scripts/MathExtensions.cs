using UnityEngine;

namespace Biosearcher.Common
{
    public static class MathExtensions
    {
        public static Vector2 SnappedToGrid(this Vector2 v)
        {
            return new Vector2(Mathf.Round(v.x + 0.5f) - 0.5f, Mathf.Round(v.y + 0.5f) - 0.5f);
        }
        public static Vector3 SnappedToGrid(this Vector3 v, bool keepY)
        {
            return new Vector3(Mathf.Round(v.x + 0.5f) - 0.5f, keepY ? v.y : 0, Mathf.Round(v.z + 0.5f) - 0.5f);
        }
        public static Vector3 SnappedToGrid(this Vector3 v, float newY)
        {
            return new Vector3(Mathf.Round(v.x + 0.5f) - 0.5f, newY, Mathf.Round(v.z + 0.5f) - 0.5f);
        }

        public static Vector2 ProjectedZY(this Vector3 v) => new Vector2(v.z, v.y);
        public static Vector3 ReProjectedZY(this Vector2 v, float newX = 0) => new Vector3(newX, v.y, v.x);
        public static Vector2 ProjectedXZ(this Vector3 v) => new Vector2(v.x, v.z);
        public static Vector3 ReProjectedXZ(this Vector2 v, float newY = 0) => new Vector3(v.x, newY, v.y);
        public static Vector2 ProjectedXY(this Vector3 v) => new Vector2(v.x, v.y);
        public static Vector3 ReProjectedXY(this Vector2 v, float newZ = 0) => new Vector3(v.x, v.y, newZ);

        public static Vector3 DroppedY(this Vector3 v) => new Vector3(v.x, 0, v.z);

        public static Vector2 NormalizedDiamond(this Vector2 v) => v / (Mathf.Abs(v.x) + Mathf.Abs(v.y));
        public static Vector2 ClampedDiamond(this Vector2 v, float maxClamp)
        {
            if (maxClamp <= 0)
            {
                Debug.Log("maxClamp has value less than or 0");
            }

            Vector2 normalized = v.NormalizedDiamond() * maxClamp;
            return v.magnitude > normalized.magnitude ? normalized : v;
        }

        public static void Deconstruct(this Vector2 v, out float x, out float y)
        {
            x = v.x;
            y = v.y;
        }

        public static Quaternion SmoothDamp(this Quaternion current, Quaternion target, ref Quaternion velocity,
            float smoothTime)
        {
            float x = Mathf.SmoothDamp(current.x, target.x, ref velocity.x, smoothTime);
            float y = Mathf.SmoothDamp(current.y, target.y, ref velocity.y, smoothTime);
            float z = Mathf.SmoothDamp(current.z, target.z, ref velocity.z, smoothTime);
            float w = Mathf.SmoothDamp(current.w, target.w, ref velocity.w, smoothTime);
            return new Quaternion(x, y, z, w);
        }

        public static Vector3 DivideBy(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        public static int GetOdd(this int n) => n | 1;
        public static void MakeOdd(this ref int n) => n |= 1;

        public static int GetEven(this int n) => n & ~1;
        public static void MakeEven(this ref int n) => n &= ~1;
    }
}