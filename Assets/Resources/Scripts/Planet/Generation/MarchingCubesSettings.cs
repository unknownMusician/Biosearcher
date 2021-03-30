using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Biosearcher
{
    [CreateAssetMenu(fileName = "MarchingCubes Settings", menuName = "MarchingCubes Settings", order = 52)]
    public class MarchingCubesSettings : ScriptableObject
    {
        // if changing this, change numthreads in MarchingCubes.compute
        [SerializeField] protected int cubesChunkSize = 6;
        [SerializeField] protected float surfaceValue = 0.2f;

        public int CubesChunkSize => cubesChunkSize;
        public int PointsChunkSize => cubesChunkSize + 1;
        public float SurfaceValue => surfaceValue;
    }
}
