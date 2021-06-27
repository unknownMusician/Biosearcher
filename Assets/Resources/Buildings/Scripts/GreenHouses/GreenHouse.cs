using System.Linq;
using Biosearcher.Plants;
using Biosearcher.Player;
using UnityEngine;

namespace Biosearcher.Buildings.GreenHouses
{
    public abstract class GreenHouse : Building
    {
        #region Properties

        // todo: shouldn't it be in ScriptableObject
        [SerializeField] protected Slot[] _slots;
        protected Capsule[] _capsules;

        #endregion

        #region Methods

        protected new virtual void Awake()
        {
            _capsules = new Capsule[_slots.Length];
            base.Awake();
        }
        protected virtual void Start() => RecalculateNeededResourcesForAllSlots();

        protected virtual void RecalculateNeededResourcesForAllSlots()
        {
            PreRecalculateNeededResources();
            foreach (Capsule capsule in _capsules)
            {
                if (capsule != null)
                {
                    RecalculateNeededResources(capsule);
                }
            }
        }

        protected abstract void PreRecalculateNeededResources();
        protected abstract void RecalculateNeededResources(Capsule capsule);

        public void Plant(Seed seed, int capsuleNumber)
        {
            // todo
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            Transform parent = _capsules[capsuleNumber].transform;
            _capsules[capsuleNumber].Plant = seed.Plant(position, rotation, parent);
            RecalculateNeededResourcesForAllSlots();
            Destroy(seed.gameObject);
        }
        public void Plant(Seed seed, Capsule capsule)
        {
            var capsuleTransform = capsule.transform;
            
            Vector3 position = capsuleTransform.position;
            Quaternion rotation = capsuleTransform.rotation;
            capsule.Plant = seed.Plant(position, rotation, capsuleTransform);
            RecalculateNeededResourcesForAllSlots();
            Destroy(seed.gameObject);
        }
        
        //todo: капсула должна ложиться в массив капсул
        public void ChangeCapsule(Capsule capsule, int slotNumber)
        {
            _slots[slotNumber].Capsule = capsule;
            capsule.GreenHouse = this;
            capsule.transform.parent = _slots[slotNumber].transform;
            RecalculateNeededResourcesForAllSlots();
        }
        public void ChangeCapsule(Capsule capsule, Slot slot)
        {
            slot.Capsule = capsule;
            capsule.GreenHouse = this;
            capsule.transform.parent = slot.transform;
            RecalculateNeededResourcesForAllSlots();
        }

        #endregion
    }
}
