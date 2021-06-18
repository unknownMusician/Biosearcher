using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Biosearcher.Common;
using Biosearcher.Buildings.Resources.Interfaces;
using Biosearcher.Buildings.Types.Interfaces;
using UnityEngine;

namespace Biosearcher.Buildings.Resources
{
    public sealed class Network<TResource> : System.IDisposable where TResource : IResource<TResource>, new()
    {
        #region Properties

        private bool _isCycleActive = true;
        private float _cyclesPerSecond;

        private List<IResourceProducer<TResource>> _producers = new List<IResourceProducer<TResource>>();
        private List<IResourceReceiver<TResource>> _receivers = new List<IResourceReceiver<TResource>>();

        public Connections Connection { get; private set; }

        #endregion

        public Network(IResourceMover<TResource> resourceMover, float cyclesPerSecond)
        {
            Connection = new Connections(this, resourceMover);
            _cyclesPerSecond = cyclesPerSecond;
            this.StartCoroutine(NetworkCycle(_cyclesPerSecond));
        }

        #region Methods

        private IEnumerator NetworkCycle(float cyclesPerSecond)
        {
            var waitForSeconds = new WaitForSeconds(1 / cyclesPerSecond);
            while (_isCycleActive)
            {
                Tick();
                yield return waitForSeconds;
            }
        }
        private void Tick()
        {
            TResource availableResource = new TResource { Value = 0 };
            TResource neededResource = new TResource { Value = 0 };

            foreach (var producer in _producers)
            {
                availableResource = availableResource.Add(producer.Produce());
            }
            foreach (var receiver in _receivers)
            {
                neededResource = neededResource.Add(receiver.CurrentPossibleReceived);
            }

            float coefficient = Mathf.Min(availableResource.Divide(neededResource), 1);

            foreach (var receiver in _receivers)
            {
                var resourceWeCanGive = new TResource { Value = coefficient * receiver.CurrentPossibleReceived.Value };
                receiver.Receive(resourceWeCanGive);
            }
        }

        public void Dispose()
        {
            _isCycleActive = false;
            Connection.Dispose();
        }

        #endregion

        public sealed class Connections : System.IDisposable
        {
            private Network<TResource> _network;
            private HashSet<Entry> _movers = new HashSet<Entry>();

            public Connections(Network<TResource> network, IResourceMover<TResource> resourceMover)
            {
                _network = network;
                TryAdd(resourceMover);

                CommonMonoBehaviour.OnDrawGizmos += OnDrawGizmos;
            }

            public void TryAdd(IResourceMover<TResource> resourceMover, IResourceMover<TResource> networkMember)
            {
                Entry newMoverEntry = TryAdd(resourceMover);
                Entry connectedMoverEntry = EntryOrDefault(networkMember);

                connectedMoverEntry.ConnectedMovers.Add(newMoverEntry);
                newMoverEntry.ConnectedMovers.Add(connectedMoverEntry);
            }
            private Entry TryAdd(IResourceMover<TResource> resourceMover)
            {
                if (resourceMover.Network?.Connection.EntryOrDefault(resourceMover) == default)
                {
                    return TryAddWithoutEntry(resourceMover);
                }
                Entry newMoverEntry = EntryOrDefault(resourceMover);
                if (newMoverEntry == default)
                {
                    return TryAddFromOtherNetwork(resourceMover);
                }
                return newMoverEntry;
            }
            private Entry TryAddWithoutEntry(IResourceMover<TResource> resourceMover)
            {
                Entry newMoverEntry = new Entry(resourceMover);
                AddToNetwork(newMoverEntry);
                return newMoverEntry;
            }
            private Entry TryAddFromOtherNetwork(IResourceMover<TResource> resourceMover)
            {
                Entry newMoverEntry = resourceMover.Network.Connection.EntryOrDefault(resourceMover);
                newMoverEntry.Mover.Network.Dispose();
                AddToNetwork(newMoverEntry);
                return newMoverEntry;
            }
            public void TryRemove(IResourceMover<TResource> resourceMover)
            {
                Entry removedMoverEntry = EntryOrDefault(resourceMover);
                if (removedMoverEntry != default)
                {
                    DisconnectAndDivide(removedMoverEntry);
                }
            }
            private void DisconnectAndDivide(Entry removedEntry)
            {
                for (int i = 0; i < removedEntry.ConnectedMovers.Count; i++)
                {
                    removedEntry.ConnectedMovers[i].ConnectedMovers.Remove(removedEntry);
                }
                var disconnectedNeighbours = new Entry[removedEntry.ConnectedMovers.Count + 1];
                disconnectedNeighbours[0] = removedEntry;
                removedEntry.ConnectedMovers.CopyTo(disconnectedNeighbours, 1);
                removedEntry.ConnectedMovers.Clear();

                DivideNetwork(disconnectedNeighbours);
            }
            private void DivideNetwork(Entry[] disconnectedNeighbours)
            {
                var addedEntries = new HashSet<Entry>();
                foreach (Entry disconnectedNeighbour in disconnectedNeighbours)
                {
                    var potentialNetwork = new HashSet<Entry>();

                    TryAddToNetwork(disconnectedNeighbour, potentialNetwork, addedEntries);

                    if (potentialNetwork.Count == 0)
                    {
                        continue;
                    }

                    new Network<TResource>(potentialNetwork.First().Mover, _network._cyclesPerSecond);
                }
                _network.Dispose();
            }
            private void TryAddToNetwork(Entry entry, HashSet<Entry> potentialNetwork, HashSet<Entry> addedEntries)
            {
                if (addedEntries.Add(entry) && potentialNetwork.Add(entry))
                {
                    foreach (Entry connectedEntry in entry.ConnectedMovers)
                    {
                        TryAddToNetwork(connectedEntry, potentialNetwork, addedEntries);
                    }
                }
            }

            private void AddToNetwork(Entry newMoverEntry)
            {
                _movers.Add(newMoverEntry);

                IResourceMover<TResource> resourceMover = newMoverEntry.Mover;
                resourceMover.Network = _network;
                if (resourceMover is IResourceReceiver<TResource> resourceReceiver)
                {
                    _network._receivers.Add(resourceReceiver);
                }
                if (resourceMover is IResourceProducer<TResource> resourceProducer)
                {
                    _network._producers.Add(resourceProducer);
                }

                HookEntries(newMoverEntry);
            }
            // todo: unused
            private void RemoveFromNetwork(Entry removedMoverEntry)
            {
                IResourceMover<TResource> resourceMover = removedMoverEntry.Mover;
                if (resourceMover is IResourceReceiver<TResource> resourceReceiver)
                {
                    _network._receivers.Remove(resourceReceiver);
                }
                if (resourceMover is IResourceProducer<TResource> resourceProducer)
                {
                    _network._producers.Remove(resourceProducer);
                }

                _movers.Remove(removedMoverEntry);
            }

            // todo: optimiztion
            private Entry EntryOrDefault(IResourceMover<TResource> resourceMover)
            {
                return _movers.Where(entry => entry.Mover == resourceMover).FirstOrDefault();
            }
            private void HookEntries(Entry entry)
            {
                foreach (Entry connectedEntry in entry.ConnectedMovers)
                {
                    if (!_movers.Contains(connectedEntry))
                    {
                        AddToNetwork(connectedEntry);
                    }
                }
            }

            public void Dispose() => CommonMonoBehaviour.OnDrawGizmos -= OnDrawGizmos;
            // todo: optimization
            private void OnDrawGizmos()
            {
                Gizmos.color = Color.white;
                if (typeof(TResource) == typeof(Structs.Water))
                {
                    Gizmos.color = Color.blue;
                }
                else if (typeof(TResource) == typeof(Structs.Electricity))
                {
                    Gizmos.color = Color.yellow;
                }
                foreach (Entry mover in _movers)
                {
                    foreach (Entry connectedMover in mover.ConnectedMovers)
                    {
                        Gizmos.DrawLine(((MonoBehaviour)mover.Mover).transform.position, ((MonoBehaviour)connectedMover.Mover).transform.position);
                    }
                }

                Gizmos.color *= new Color(0.3f, 0.3f, 0.3f);
                Vector3[] moverPositions = _movers.Select(mover => ((MonoBehaviour)mover.Mover).transform.position).ToArray();
                Vector3 sum = Vector3.zero;
                foreach (Vector3 moverPosition in moverPositions)
                {
                    sum += moverPosition;
                }
                Vector3 networkPosition = sum / moverPositions.Length;
                Gizmos.DrawSphere(networkPosition, 0.1f);
                foreach (Vector3 moverPosition in moverPositions)
                {
                    Gizmos.DrawLine(networkPosition, moverPosition);
                }
            }

            public class Entry
            {
                public IResourceMover<TResource> Mover { get; protected set; }
                public List<Entry> ConnectedMovers { get; protected set; } = new List<Entry>();

                public Entry(IResourceMover<TResource> mover)
                {
                    Mover = mover;
                }
                public Entry(IResourceMover<TResource> mover, IEnumerable<Entry> connectedMovers) : this(mover)
                {
                    ConnectedMovers.AddRange(connectedMovers);
                }
            }
        }
    }
}
