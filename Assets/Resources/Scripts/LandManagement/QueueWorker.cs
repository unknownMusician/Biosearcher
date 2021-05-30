using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Biosearcher.LandManagement
{
    // todo: do not do job, when there are no requests
    public class QueueWorker<I, O> : System.IDisposable
    {
        protected System.Func<I, O> job;

        protected Queue<Request> requests = new Queue<Request>();

        protected bool isAlive = true;
        protected int generatingFrequency;

        public QueueWorker(MonoBehaviour behaviour, System.Func<I, O> job, int generatingFrequency)
        {
            this.job = job;
            this.generatingFrequency = generatingFrequency;
            behaviour.StartCoroutine(Job());
        }

        public void MakeRequest(I input, UnityAction<O> onJobDone)
        {
            var request = new Request() { input = input, onJobDone = onJobDone };
            requests.Enqueue(request);
        }

        public void TryRemoveRequest(I input)
        {
            Request request = requests.Where(request => request.input.Equals(input)).FirstOrDefault();
            if (!request.Equals(default))
            {
                request.isCanceled = true;
            }
        }

        protected IEnumerator Job()
        {
            while (isAlive)
            {
                for (int i = 0; i < generatingFrequency; i++)
                {
                    if (requests.Count <= 0)
                    {
                        break;
                    }
                    Request request = requests.Dequeue();
                    if (request.isCanceled)
                    {
                        i--;
                        continue;
                    }
                    O output = job.Invoke(request.input);
                    request.onJobDone?.Invoke(output);

                }
                yield return null;
            }
        }

        public void Dispose() => isAlive = false;

        protected class Request
        {
            public I input;
            public bool isCanceled;
            public UnityAction<O> onJobDone;
        }
    }
}