using System.Collections.Generic;
using System.Linq;
using Biosearcher.Plants;
using Biosearcher.Player;
using Biosearcher.Refactoring;
using UnityEngine;

namespace Biosearcher.Buildings.GreenHouses
{
    public abstract class GreenHouse : Building
    {
        #region Properties

        protected List<Capsule> _capsules;

        #endregion

        #region Methods

        protected override void Awake()
        {
            base.Awake();
            _capsules = new List<Capsule>();
        }
        protected virtual void Start() => RecalculateNeededResourcesForAllCapsules();

        protected virtual void RecalculateNeededResourcesForAllCapsules()
        {
            PreRecalculateNeededResourcesForAllCapsules();
            foreach (Capsule capsule in _capsules)
            {
                if (capsule != null)
                {
                    RecalculateNeededResources(capsule);
                }
            }
        }

        protected abstract void PreRecalculateNeededResourcesForAllCapsules();
        protected abstract void RecalculateNeededResources(Capsule capsule);

        public void PlantChanged() => RecalculateNeededResourcesForAllCapsules();

        public void HandleCapsuleInsert(Capsule capsule)
        {
            _capsules.Add(capsule);
            capsule.GreenHouse = this;
            RecalculateNeededResourcesForAllCapsules();
        }

        public void HandleCapsuleGrabbed(Capsule capsule)
        {
            capsule.GreenHouse = null;
            _capsules.Remove(capsule);
            RecalculateNeededResourcesForAllCapsules();
        }

        #endregion
    }
}
