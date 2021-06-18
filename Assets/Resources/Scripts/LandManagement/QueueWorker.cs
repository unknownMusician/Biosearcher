using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Biosearcher.Common;
using UnityEngine.Events;

namespace Biosearcher.LandManagement
{
    // todo: do not do job, when there are no requests
    public class QueueWorker<I, O> : System.IDisposable
    {
        #region Properties

        protected readonly System.Func<I, O> _job;

        protected readonly Queue<Request> _requests = new Queue<Request>();

        protected bool _isAlive = true;
        protected readonly int _generatingFrequency;

        #endregion

        public QueueWorker(System.Func<I, O> job, int generatingFrequency)
        {
            _job = job;
            _generatingFrequency = generatingFrequency;
            this.StartCoroutine(Job());
        }

        #region Methods

        public void MakeRequest(I input, UnityAction<O> onJobDone)
        {
            var request = new Request() { input = input, onJobDone = onJobDone };
            _requests.Enqueue(request);
            if (!_isAlive)
            {
                _isAlive = true;
                this.StartCoroutine(Job());
            }
        }
        public void TryRemoveRequest(I input)
        {
            Request request = _requests.FirstOrDefault(request => request.input.Equals(input));
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
                    if (_requests.Count <= 0)
                    {
                        _isAlive = false;
                        break;
                    }
                    
                    Request request = _requests.Dequeue();
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

        public void Dispose() => _isAlive = false;

        #endregion

        #region Classes

        protected class Request
        {
            public I input;
            public bool isCanceled;
            public UnityAction<O> onJobDone;
        }

        #endregion
    }
}
