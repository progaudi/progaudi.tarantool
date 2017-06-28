using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    internal class ResponseWriter : IResponseWriter
    {
        private readonly ClientOptions _clientOptions;
        private readonly IPhysicalConnection _physicalConnection;
        private bool _disposed;
        private readonly Queue<Tuple<ArraySegment<byte>, ArraySegment<byte>>> _buffer;
        private readonly object _lock = new object();
        private readonly Thread _thread;
        private readonly ManualResetEventSlim _exitEvent;
        private readonly ManualResetEventSlim _newRequestsAvailable;

        public ResponseWriter(ClientOptions clientOptions, IPhysicalConnection physicalConnection)
        {
            _clientOptions = clientOptions;
            _physicalConnection = physicalConnection;
            _buffer = new Queue<Tuple<ArraySegment<byte>, ArraySegment<byte>>>();
            _thread = new Thread(WriteFunction);
            _exitEvent = new ManualResetEventSlim();
            _newRequestsAvailable = new ManualResetEventSlim();
        }

        public void BeginWriting()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ResponseReader));
            }

            _clientOptions?.LogWriter?.WriteLine("Starting ResponseWriter.");
            _thread.Start();
        }

        public bool IsConnected => !_disposed;

        public Task Write(ArraySegment<byte> header, ArraySegment<byte> body)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ResponseReader));
            }

            _clientOptions?.LogWriter?.WriteLine($"Enqueuing request: headers {header.Count} bytes, body {body.Count} bytes.");
            lock (_lock)
                _buffer.Enqueue(Tuple.Create(header, body));

            _newRequestsAvailable.Set();

            return Task.CompletedTask;
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
            void WriteBuffer(ArraySegment<byte> buffer)
            {
                _physicalConnection.Write(buffer.Array, buffer.Offset, buffer.Count);
            }

            Tuple<ArraySegment<byte>, ArraySegment<byte>> request;
            var count = 0;
            while ((request = GetRequest()) != null)
            {
                _clientOptions?.LogWriter?.WriteLine($"Writing request: headers {request.Item1.Count} bytes, body {request.Item2.Count} bytes.");

                WriteBuffer(request.Item1);
                WriteBuffer(request.Item2);

                _clientOptions?.LogWriter?.WriteLine($"Wrote request: headers {request.Item1.Count} bytes, body {request.Item2.Count} bytes.");

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

        private Tuple<ArraySegment<byte>, ArraySegment<byte>> GetRequest()
        {
            lock (_lock)
            {
                if (_buffer.Count > 0)
                    return _buffer.Dequeue();
            }

            return null;
        }
    }

    internal interface IResponseWriter : IDisposable
    {
        void BeginWriting();

        bool IsConnected { get; }

        Task Write(ArraySegment<byte> header, ArraySegment<byte> body);
    }
}