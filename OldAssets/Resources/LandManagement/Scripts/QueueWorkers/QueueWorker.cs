using UnityEngine.Events;

namespace Biosearcher.LandManagement.QueueWorkers
{
    public abstract class QueueWorker<I, O> : System.IDisposable
    {
        #region Properties

        protected readonly System.Func<I, O> _job;

        protected bool _isAlive = true;
        protected readonly int _generatingFrequency;

        #endregion

        public QueueWorker(System.Func<I, O> job, int generatingFrequency)
        {
            _job = job;
            _generatingFrequency = generatingFrequency;
        }

        #region Methods

        public abstract void MakeRequest(I input, UnityAction<O> onJobDone);
        public abstract void TryRemoveRequest(I input);

        public virtual void Dispose() => _isAlive = false;

        #endregion

        #region Types

        protected class CancelableRequest
        {
            public I input;
            public UnityAction<O> onJobDone;
            public bool isCanceled;
        }

        public class Response
        {
            public O output;
            public UnityAction<O> onJobDone;
        }

        #endregion
    }
}
