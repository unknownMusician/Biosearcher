using System;
using AreYouFruits.Common.ComponentGeneration;
using Biosearcher.Player.Interactions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Biosearcher.Plants
{
    public class GreenHouse : IInsertFriendly
    {
        public event Action<IInsertable>? OnInsert;
        public event Action<Plant>? OnPlantGrew;

        private readonly Transform _transform;
        private readonly Vector3 _alignLocalPosition;

        private (Plant plant, GameObject gameObject)? _plantInfo;

        public GreenHouse(Transform transform, Vector3 alignLocalPosition)
        {
            _transform = transform;
            _alignLocalPosition = alignLocalPosition;
        }

        private void HandlePlantGrew(Plant plant, GameObject plantObject)
        {
            Object.Destroy(plantObject);

            OnPlantGrew?.Invoke(plant);
        }

        public void HandleInsert(IInsertable insertable)
        {
            this.ThrowIfCannotInsert(insertable, out Seed seed);

            PlantInfo plantInfo = seed.PlantInfo;

            HandleAlignStart(seed);
            Vector3 plantPosition = seed.GameObject.transform.localPosition;

            Object.Destroy(seed.GameObject);
            GameObject plantObject = Object.Instantiate(plantInfo.PlantPrefab, _transform);
            plantObject.transform.localPosition = plantPosition;

            Plant plant = plantObject.GetHeldItem<Plant>();
            _plantInfo = (plant, plantObject);

            plant.OnGrowEnd += () => HandlePlantGrew(plant, plantObject);
            plant.TryStartGrow(plantInfo);
        }

        public void HandleAlignStart(IInsertable insertable)
        {
            this.ThrowIfCannotInsert(insertable, out Seed seed);

            seed.GameObject.transform.SetPositionAndRotation(
                _transform.position + _alignLocalPosition,
                _transform.rotation
            );
        }

        public void HandleAlignEnd(IInsertable insertable)
        {
            this.ThrowIfCannotInsert(insertable, out Seed _);
        }

        public bool CanInsert(IInsertable insertable) => insertable is Seed && _plantInfo is null;
    }
}
