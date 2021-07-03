using System;
using Biosearcher.Buildings.GreenHouses;
using Biosearcher.Planets.Orientation;
using Biosearcher.Player;
using UnityEngine;

namespace Biosearcher.Plants
{
    public class Slot : MonoBehaviour, IInsertFriendly<Capsule>
    {
        private GreenHouse _greenHouse;
        private PlanetTransform _planetTransform;

        private Capsule _capsule;
        
        public Capsule Capsule
        {
            get => _capsule;
            set => _capsule = value;
        }
        
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
            _greenHouse.ChangeCapsule(insertable, this);
            return true;
        }

        public bool TryAlign(Capsule insertable)
        {
            insertable.transform.position = transform.position + _planetTransform.ToUniverse(0.5f * Vector3.up);
            return true;
        }
    }
}
