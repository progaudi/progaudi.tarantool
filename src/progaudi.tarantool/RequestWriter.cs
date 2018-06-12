using System;
using System.Collections.Generic;
using System.Threading;
using MessagePack;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    internal class RequestWriter : IRequestWriter
    {
        private readonly ClientOptions _clientOptions;
        private readonly IPhysicalConnection _physicalConnection;
        private readonly Queue<Tuple<byte[], byte[]>> _buffer;
        private readonly object _lock = new object();
        private readonly Thread _thread;
        private readonly ManualResetEventSlim _exitEvent;
        private readonly ManualResetEventSlim _newRequestsAvailable;
        private bool _disposed;

        public RequestWriter(ClientOptions clientOptions, IPhysicalConnection physicalConnection)
        {
            _clientOptions = clientOptions;
            _physicalConnection = physicalConnection;
            _buffer = new Queue<Tuple<byte[], byte[]>>();
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
                throw new ObjectDisposedException(nameof(ResponseReader));
            }

            _clientOptions?.LogWriter?.WriteLine("Starting thread");
            _thread.Start();
        }

        public bool IsConnected => !_disposed;

        public void Write(byte[] header, byte[] body)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ResponseReader));
            }

            _clientOptions?.LogWriter?.WriteLine($"Enqueuing request: headers {header.Length} bytes, body {body.Length} bytes.");
            bool shouldSignal;
            lock (_lock)
            {
                _buffer.Enqueue(Tuple.Create(header, body));
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
            Tuple<byte[], byte[]> GetRequest()
            {
                lock (_lock)
                {
                    if (_buffer.Count > 0)
                        return _buffer.Dequeue();
                }

                return null;
            }

            Tuple<byte[], byte[]> request;
            var count = 0;
            while ((request = GetRequest()) != null)
            {
                var header = request.Item1;
                var body = request.Item2;
                _clientOptions?.LogWriter?.WriteLine($"Writing request: headers {header.Length} bytes, body {body.Length} bytes.");

                var length = MessagePackSerializer.Serialize(new RequestLength(header, body));
                _physicalConnection.Write(length, 0, length.Length);
                _physicalConnection.Write(header, 0, header.Length);
                _physicalConnection.Write(body, 0, body.Length);

                _clientOptions?.LogWriter?.WriteLine($"Wrote request: headers {header.Length} bytes, body {body.Length} bytes.");

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
