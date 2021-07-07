using System;
using Biosearcher.Buildings.GreenHouses;
using Biosearcher.Planets.Orientation;
using Biosearcher.Player;
using UnityEngine;

namespace Biosearcher.Plants
{
    [RequireComponent(typeof(PlanetTransform))]
    public class Slot : MonoBehaviour, IInsertFriendly<Capsule>
    {
        private GreenHouse _greenHouse;
        private PlanetTransform _planetTransform;

        private Capsule _capsule;

        private void Awake()
        {
            _greenHouse = GetComponentInParent<GreenHouse>();
            _planetTransform = GetComponent<PlanetTransform>();

            if (_greenHouse == null)
            {
                Debug.LogError("Missing GreenHouse!");
            }
        }

        public bool TryInsert(Capsule insertable)
        {
            if (_capsule == null)
            {
                insertable.transform.SetParent(transform);
                _greenHouse.HandleCapsuleInsert(insertable);
                return true;
            }
            return false;
        }

        public bool TryAlign(Capsule insertable)
        {
            insertable.transform.position = transform.position + _planetTransform.ToUniverse(0.5f * Vector3.up);
            return true;
        }

        public void HandleInsertableGrabbed(Capsule insertable)
        {
            _greenHouse.HandleCapsuleGrabbed(insertable);
            insertable.transform.SetParent(null);
        }
    }
}
