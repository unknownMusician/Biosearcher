using System.Collections;
using System.Collections.Generic;
using Biosearcher.Buildings.Resources.Interfaces;
using Biosearcher.Buildings.Types.Interfaces;
using UnityEngine;

namespace Biosearcher.Buildings.Network
{
    public class Network<TResource> : MonoBehaviour where TResource : IResource<TResource>, new()
    {
        #region Properties

        [SerializeField] private float cyclesPerSecond;

        private bool isCycleActive;
        
        public List<IResourceProducer<TResource>> producers;
        public List<IResourceReceiver<TResource>> receivers;

        #endregion

        #region Behaviour methods

        private void Awake()
        {
            producers = new List<IResourceProducer<TResource>>();
            receivers = new List<IResourceReceiver<TResource>>();
        }
        private void Start()
        {
            isCycleActive = true;
            StartCoroutine(NetworkCycle());
        }

        #endregion

        #region Methods

        private IEnumerator NetworkCycle()
        {
            yield return new WaitForSeconds(1f);
            
            while (isCycleActive)
            {
                Tick();
                yield return new WaitForSeconds(1 / cyclesPerSecond);
            }
        }
        private void Tick()
        {
            TResource availableResource = default;
            TResource neededResource = default;

            foreach (var producer in producers)
            {
                availableResource = availableResource.Add(producer.CurrentPossibleProduced);
            }
            foreach (var receiver in receivers)
            {
                neededResource = neededResource.Add(receiver.CurrentPossibleReceived);
            }

            var delta = availableResource.Subtract(neededResource).Value;
            float coefficient;
            if (delta >= 0)
            {
                coefficient = 1;
            }
            else
            {
                // TODO : divide должен возвращать float
                coefficient = availableResource.Divide(neededResource).Value;
            }

            foreach (var producer in producers)
            {
                producer.Produce();
            }
            foreach (var receiver in receivers)
            {
                var resourceWeCanGive = new TResource {Value = coefficient * receiver.CurrentPossibleReceived.Value};
                receiver.Receive(resourceWeCanGive);
            }
        }

        #endregion
    }
}
