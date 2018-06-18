using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    internal class RequestWriter : IRequestWriter
    {
        private readonly ClientOptions _clientOptions;
        private readonly IPhysicalConnection _physicalConnection;
        private readonly Queue<ArraySegment<byte>> _buffer;
        private readonly object _lock = new object();
        private readonly Thread _thread;
        private readonly ManualResetEventSlim _exitEvent;
        private readonly ManualResetEventSlim _newRequestsAvailable;
        private bool _disposed;

        public RequestWriter(ClientOptions clientOptions, IPhysicalConnection physicalConnection)
        {
            _clientOptions = clientOptions;
            _physicalConnection = physicalConnection;
            _buffer = new Queue<ArraySegment<byte>>();
            _thread = new Thread(WriteFunction)
            {
                IsBackground = true,
                Name = $"Tarantool :: {clientOptions.Name} :: Write"
            };
            _exitEvent = new ManualResetEventSlim();
            _newRequestsAvailable = new ManualResetEventSlim();
        }

        public void BeginWriting()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(RequestWriter));
            }

            _clientOptions?.LogWriter?.WriteLine("Starting thread");
            _thread.Start();
        }

        public bool IsConnected => !_disposed;

        public void Write(in ArraySegment<byte> body)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(RequestWriter));
            }

            _clientOptions?.LogWriter?.WriteLine($"Enqueuing request: body {body.Count} bytes.");
            bool shouldSignal;
            lock (_lock)
            {
                _buffer.Enqueue(body);
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

        private void WriteRequests(int limit)
        {
            var count = 0;
            while (true)
            {
                ArraySegment<byte> request;
                lock (_lock)
                {
                    if (_buffer.Count > 0)
                        request = _buffer.Dequeue();
                    else
                        break;
                }

                _clientOptions?.LogWriter?.WriteLine($"Writing request: {request.Count} bytes.");

                _physicalConnection.Write(request);
                ArrayPool<byte>.Shared.Return(request.Array);

                _clientOptions?.LogWriter?.WriteLine($"Wrote request: {request.Count} bytes.");

                count++;
                if (limit > 0 && count > limit)
                {
                    break;
                }
            }

            lock (_lock)
            {
                if (_buffer.Count == 0)
                    _newRequestsAvailable.Reset();
            }

            _physicalConnection.Flush();
        }
    }
}
