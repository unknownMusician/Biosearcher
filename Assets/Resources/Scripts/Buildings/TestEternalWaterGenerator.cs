using System;
using Biosearcher.Buildings.Resources.Structs;
using Biosearcher.Buildings.Types.Interfaces;
using UnityEngine;

namespace Biosearcher.Buildings
{
    public class TestEternalWaterGenerator : MonoBehaviour, IResourceProducer<Water>
    {
        public Water MaxPossibleProduced { get; private set;  }
        public Water CurrentPossibleProduced { get; private set;  }

        private void Awake()
        {
            MaxPossibleProduced = new Water {volume = 100};
            CurrentPossibleProduced = new Water {volume = 100};
        }

        public Water Produce()
        {
            return new Water {volume = 100};
        }
    }
}