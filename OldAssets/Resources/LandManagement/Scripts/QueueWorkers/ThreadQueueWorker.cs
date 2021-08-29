using Biosearcher.Common;
using Biosearcher.Refactoring;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace Biosearcher.LandManagement.QueueWorkers
{
    public sealed class ThreadQueueWorker<I, O> : QueueWorker<I, O>
    {
        #region Properties

        private readonly Thread _thread;

        private readonly ConcurrentQueue<CancelableRequest> _requests = new ConcurrentQueue<CancelableRequest>();
        private readonly ConcurrentQueue<Response> _responses = new ConcurrentQueue<Response>();

        #endregion

        public ThreadQueueWorker(System.Func<I, O> job, int generatingFrequency) : base(job, generatingFrequency)
        {
            _thread = new Thread(ThreadCycle) { Name = "AsyncQueueWorker Thread" };
            _thread.Start();

            this.StartCoroutine(ResponseReceiving());
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

        private void ThreadCycle()
        {
            while (_isAlive)
            {
                if (_requests.Count == 0)
                {
                    continue;
                }

                if (!_requests.TryDequeue(out CancelableRequest request))
                {
                    Debug.LogWarning("Failed to Dequeue request");
                    continue;
                }
                if (request.isCanceled)
                {
                    continue;
                }

                var response = new Response
                {
                    onJobDone = request.onJobDone,
                    output = _job.Invoke(request.input)
                };
                _responses.Enqueue(response);
            }
        }

        private IEnumerator ResponseReceiving()
        {
            while (_isAlive)
            {
                for (int i = 0; (i < _generatingFrequency) && (_responses.Count > 0); i++)
                {
                    if (!_responses.TryDequeue(out Response response))
                    {
                        Debug.LogWarning("Failed to Dequeue response");
                        i--;
                        continue;
                    }

                    response.onJobDone?.Invoke(response.output);
                }

                yield return null;
            }
        }

        [NeedsRefactor]
        public override void Dispose()
        {
            base.Dispose();
            // todo
            _thread.Abort();
        }

        #endregion
    }

}