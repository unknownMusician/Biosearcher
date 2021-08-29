using Biosearcher.Common;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

namespace Biosearcher.LandManagement.QueueWorkers
{
    public sealed class JobQueueWorker<I, O> : QueueWorker<I, O>
    {
        #region Static

        private static readonly ConcurrentDictionary<int, JobQueueWorker<I, O>> instances = new ConcurrentDictionary<int, JobQueueWorker<I, O>>();

        private static bool TryGetJobInfo(int workerId, int jobId, out JobInfo jobInfo)
        {
            if (instances.TryGetValue(workerId, out JobQueueWorker<I, O> worker))
            {
                return worker.TryGetAndRemoveJobInfo(jobId, out jobInfo);
            }
            jobInfo = default;
            return false;
        }

        #endregion

        #region Properties

        private readonly ConcurrentDictionary<int, JobInfo> _jobInfos = new ConcurrentDictionary<int, JobInfo>();
        private readonly Queue<CancelableRequest> _requests = new Queue<CancelableRequest>();
        private readonly ConcurrentQueue<CancelableResponse> _responses = new ConcurrentQueue<CancelableResponse>();

        #endregion

        public JobQueueWorker(System.Func<I, O> job, int generatingFrequency) : base(job, generatingFrequency)
        {
            if (!instances.TryAdd(GetHashCode(), this))
            {
                Debug.LogError($"Failed to add instance of {GetType()} to instances.");
            }
            this.StartCoroutine(ResponseReceiving());
        }

        #region Methods

        private bool TryGetAndRemoveJobInfo(int jobId, out JobInfo jobInfo)
        {
            return _jobInfos.TryRemove(jobId, out jobInfo);
        }

        public override void MakeRequest(I input, UnityAction<O> onJobDone)
        {
            var request = new CancelableRequest() { input = input, onJobDone = onJobDone };
            _requests.Enqueue(request);

            var jobInfo = new JobInfo { request = request, responses = _responses, job = _job };

            if (!_jobInfos.TryAdd(jobInfo.GetHashCode(), jobInfo))
            {
                Debug.LogError($"Failed to add Request in {GetType()}.");
            }

            var workerJob = new WorkerJob { workerId = GetHashCode(), jobId = jobInfo.GetHashCode() };
            workerJob.Schedule();
        }

        public override void TryRemoveRequest(I input)
        {
            CancelableRequest request = _requests.FirstOrDefault(request => request.input.Equals(input));
            if (request != default)
            {
                request.isCanceled = true;
            }
        }

        private IEnumerator ResponseReceiving()
        {
            while (_isAlive)
            {
                for (int i = 0; i < _generatingFrequency; i++)
                {
                    if (_responses.Count == 0)
                    {
                        break;
                    }

                    if (!_responses.TryDequeue(out CancelableResponse response))
                    {
                        Debug.LogWarning("Failed to Dequeue response");
                        i--;
                        continue;
                    }
                    if (response.isCanceled)
                    {
                        i--;
                        continue;
                    }

                    response.onJobDone?.Invoke(response.output);
                }

                yield return null;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            if (!instances.TryRemove(GetHashCode(), out _))
            {
                Debug.LogError($"Failed to remove instance of {GetType()} from instances.");
            }
        }

        #endregion

        #region Types

        private class CancelableResponse : Response
        {
            public bool isCanceled;
        }

        private struct JobInfo
        {
            public CancelableRequest request;
            public ConcurrentQueue<CancelableResponse> responses;
            public System.Func<I, O> job;
        }

        private struct WorkerJob : IJob
        {
            public int workerId;
            public int jobId;

            public void Execute()
            {
                if (!JobQueueWorker<I, O>.TryGetJobInfo(workerId, jobId, out JobInfo jobInfo))
                {
                    return;
                }
                var response = new CancelableResponse
                {
                    onJobDone = jobInfo.request.onJobDone,
                    output = jobInfo.request.isCanceled ? default : jobInfo.job(jobInfo.request.input),
                    isCanceled = jobInfo.request.isCanceled
                };
                jobInfo.responses.Enqueue(response);
            }
        }

        #endregion
    }
}