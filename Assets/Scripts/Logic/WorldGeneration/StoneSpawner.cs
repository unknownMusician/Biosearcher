using System;
using System.Collections.Generic;
using AreYouFruits.Common.ComponentGeneration;
using Biosearcher.CubeMarching;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Biosearcher.WorldGeneration
{
    [HasComponent(false)]
    public class StoneSpawner : IPropSpawner<PropSpawnerInfo, GameObject>
    {
        private readonly Transform _propParent;
        private readonly GameObject[] _propPrefabs;
        private readonly float _probability;
        private readonly float _surfaceAngle;

        public StoneSpawner(Transform propParent, GameObject[] propPrefabs, float probability, float surfaceAngle)
        {
            _propParent = propParent;
            _propPrefabs = propPrefabs;
            _probability = probability;
            _surfaceAngle = surfaceAngle;
        }

        public void Spawn(List<GameObject> spawned, PropSpawnerInfo info)
        {
            Profiler.BeginSample($"{GetType().Name}.{nameof(Spawn)}");
            
            (ReadOnlySpan<Vector3> points, Vector3 offset) = info;

            int prefabIndex = 0;

            for (int i = 0; i < points.Length; i += 3)
            {
                ReadOnlySpan<Vector3> triangle = points.Slice(i, 3);
                
                Vector3 point = (triangle[0] + triangle[1] + triangle[2]) / 3 + offset;

                if (Noise.Simple(point.x, point.y, point.z) >= _probability
                 || Mathf.Abs(Vector3.Cross(triangle[1] - triangle[0], triangle[2] - triangle[0]).normalized.y)
                  > Mathf.Sin(_surfaceAngle * Mathf.Deg2Rad))
                {
                    continue;
                }

                GameObject gameObject = Object.Instantiate(_propPrefabs[prefabIndex], _propParent);
                Transform transform = gameObject.transform;

                spawned.Add(gameObject);

                transform.position = point;
                transform.rotation = Random.rotation;
                //transform.localScale = 30.0f * Vector3.one;

                prefabIndex = (prefabIndex + 1) % _propPrefabs.Length;
            }
            
            Profiler.EndSample();
        }
    }
}
