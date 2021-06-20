using Biosearcher.Common;
using System.Collections;
using UnityEngine;

namespace Biosearcher.LandManagement
{
    public class TriggerTracker : System.IDisposable
    {
        #region Properties

        protected readonly ChunkTracker _chunkTracker;
        protected readonly Transform _trigger;
        protected bool _isAlive = true;

        #endregion

        public TriggerTracker(ChunkTracker chunkTracker, Transform trigger)
        {
            _chunkTracker = chunkTracker;
            _trigger = trigger;

            CommonMonoBehaviour.StartCoroutine(Tracking());
        }

        #region Methods

        protected IEnumerator Tracking()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();

            yield return waitForFixedUpdate;
            while (_isAlive)
            {
                _chunkTracker.SetTriggerPosition(Vector3Int.RoundToInt(_trigger.position));
                yield return waitForFixedUpdate;
            }
        }

        public void Dispose() => _isAlive = false;

        #endregion
    }
}
