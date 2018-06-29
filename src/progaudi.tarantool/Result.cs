using System;
using System.Runtime.CompilerServices;
using System.Threading;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public class Result<T> : INotifyCompletion
    {
        private Action _continuation;
        private DataResponse<T[]> _data;
        private Exception _error;

        public DataResponse<T[]> Data
        {
            get => _data;
            set
            {
                _data = value;
                Done();
            }
        }

        private void Done()
        {
            IsCompleted = true;
            var tmp = Interlocked.Exchange(ref _continuation, null);
            tmp?.Invoke();
        }

        public Exception Error
        {
            get => _error;
            set
            {
                _error = value;
                Done();
            }
        }

        public bool IsCompleted { get; set; }

        public Result<T> GetAwaiter()
        {
            return this;
        }

        public Result<T> GetResult()
        {
            var exception = _error;
            if (exception != null)
                throw exception;
            return this;
        }

        public void OnCompleted(Action continuation)
        {
            Interlocked.Exchange(ref _continuation, continuation);
        }
    }
}