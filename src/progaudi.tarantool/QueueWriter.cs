using System;
using System.Collections.Generic;
using System.Threading;
using Pipelines.Sockets.Unofficial;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client
{
    public abstract class QueueWriter : IRequestWriter
    {
        private readonly ClientOptions _clientOptions;
        private readonly Queue<Request> _buffer;
        private readonly object _lock = new object();
        private readonly Thread _thread;
        private readonly ManualResetEventSlim _exitEvent;
        private readonly ManualResetEventSlim _newRequestsAvailable;
        private bool _disposed;

        protected QueueWriter(ClientOptions clientOptions)
        {
            _clientOptions = clientOptions;
            _buffer = new Queue<Request>();
            _thread = new Thread(WriteFunction)
            {
                IsBackground = true,
                Name = $"{clientOptions.Name} :: Write"
            };
            _exitEvent = new ManualResetEventSlim();
            _newRequestsAvailable = new ManualResetEventSlim();
            LogWriter = clientOptions.LogWriter;
        }

        protected ILog LogWriter { get; }

        public void Start()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(SocketResponseReader));
            }

            _clientOptions?.LogWriter?.WriteLine("Starting thread");
            _thread.Start();
        }
        
        public void Write(Request request)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(SocketResponseReader));
            }

            _clientOptions?.LogWriter?.WriteLine($"Enqueuing request: {request.Header.Code} code.");
            bool shouldSignal;
            lock (_lock)
            {
                _buffer.Enqueue(request);
                shouldSignal = _buffer.Count == 1;
            }

            if (shouldSignal)
                _newRequestsAvailable.Set();
        }

        public void Dispose()
        {
            if (_exitEvent.IsSet || _disposed)
            {
                return;
            }

            _disposed = true;
            _exitEvent.Set();
            _thread.Join();
            _exitEvent.Dispose();
            _newRequestsAvailable.Dispose();
        }

        private void WriteFunction()
        {
            var handles = new[] { _exitEvent.WaitHandle, _newRequestsAvailable.WaitHandle };

            while (true)
            {
                switch (WaitHandle.WaitAny(handles))
                {
                    case 0:
                        return;
                    case 1:
                        WriteRequests(200);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected abstract void WriteRequests(uint batchSizeHint);

        protected Request GetRequest()
        {
            lock (_lock)
            {
                if (_buffer.Count > 0) return _buffer.Dequeue();
            }

            return null;
        }

        protected void BatchIsDone()
        {
            lock (_lock)
            {
                if (_buffer.Count == 0)
                    _newRequestsAvailable.Reset();
            }
        }
    }
}