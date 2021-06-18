using Biosearcher.Plants;
using UnityEngine;

namespace Biosearcher.Buildings.GreenHouses
{
    public abstract class GreenHouse : Building
    {
        #region Properties

        // todo: shouldn't it be in ScriptableObject
        [SerializeField] protected Slot[] _slots;

        #endregion

        #region Methods

        protected virtual void Start() => RecalculateNeededResourcesForAllSlots();

        protected virtual void RecalculateNeededResourcesForAllSlots()
        {
            PreRecalculateNeededResources();
            foreach (Slot slot in _slots)
            {
                if (slot != null)
                {
                    RecalculateNeededResources(slot);
                }
            }
        }

        protected abstract void PreRecalculateNeededResources();
        protected abstract void RecalculateNeededResources(Slot slot);

        public void Plant(Seed seed, int slotNumber)
        {
            // todo
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            Transform parent = _slots[slotNumber].transform;
            _slots[slotNumber].Plant = seed.Plant(position, rotation, parent);
            RecalculateNeededResourcesForAllSlots();
            Destroy(seed.gameObject);
        }
        public void ChangeSlot(Slot slot, int slotNumber)
        {
            _slots[slotNumber] = slot;
            RecalculateNeededResourcesForAllSlots();
        }

        #endregion
    }
}
