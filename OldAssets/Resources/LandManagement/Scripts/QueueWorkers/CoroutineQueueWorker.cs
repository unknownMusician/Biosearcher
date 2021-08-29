using Biosearcher.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Biosearcher.LandManagement.QueueWorkers
{
    public class CoroutineQueueWorker<I, O> : QueueWorker<I, O>
    {
        #region Properties

        protected readonly Queue<CancelableRequest> _requests = new Queue<CancelableRequest>();

        #endregion

        public CoroutineQueueWorker(System.Func<I, O> job, int generatingFrequency) : base(job, generatingFrequency)
        {
            this.StartCoroutine(Job());
        }

        #region Methods

        public override void MakeRequest(I input, UnityAction<O> onJobDone)
        {
            var request = new CancelableRequest() { input = input, onJobDone = onJobDone };
            _requests.Enqueue(request);
        }
        public override void TryRemoveRequest(I input)
        {
            CancelableRequest request = _requests.FirstOrDefault(request => request.input.Equals(input));
            if (request != default)
            {
                request.isCanceled = true;
            }
        }

        protected IEnumerator Job()
        {
            while (_isAlive)
            {
                for (int i = 0; i < _generatingFrequency; i++)
                {
                    if (_requests.Count == 0)
                    {
                        break;
                    }

                    CancelableRequest request = _requests.Dequeue();
                    if (request.isCanceled)
                    {
                        i--;
                        continue;
                    }

                    O output = _job.Invoke(request.input);
                    request.onJobDone?.Invoke(output);
                }

                yield return null;
            }
        }

        #endregion
    }
}