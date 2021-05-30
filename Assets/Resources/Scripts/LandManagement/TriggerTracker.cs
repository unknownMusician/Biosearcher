using System.Collections;
using UnityEngine;

namespace Biosearcher.LandManagement
{
    public class TriggerTracker : System.IDisposable
    {
        protected ChunkTracker chunkTracker;
        protected Transform trigger;
        protected bool isAlive = true;

        public TriggerTracker(ChunkTracker chunkTracker, Transform trigger, MonoBehaviour behaviour)
        {
            this.chunkTracker = chunkTracker;
            this.trigger = trigger;

            behaviour.StartCoroutine(Tracking());
        }

        protected IEnumerator Tracking()
        {
            yield return new WaitForFixedUpdate();
            while (isAlive)
            {
                chunkTracker.SetTriggerPosition(Vector3Int.RoundToInt(trigger.position));
                yield return new WaitForFixedUpdate();
            }
        }

        public void Dispose() => isAlive = false;
    }
}
