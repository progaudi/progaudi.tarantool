using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly ConnectionOptions _connectionOptions;
        private bool _disposed;
        private long _remaining;

        public RequestWriter(ClientOptions clientOptions, IPhysicalConnection physicalConnection)
        {
            _clientOptions = clientOptions;
            _physicalConnection = physicalConnection;
            _buffer = new Queue<ArraySegment<byte>>();
            _thread = new Thread(WriteFunction)
            {
                IsBackground = true,
                Name = $"{clientOptions.Name} :: Write"
            };
            _exitEvent = new ManualResetEventSlim();
            _newRequestsAvailable = new ManualResetEventSlim();
            _connectionOptions = _clientOptions.ConnectionOptions;
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

        public void Write(ArraySegment<byte> request)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ResponseReader));
            }

            _clientOptions?.LogWriter?.WriteLine($"Enqueuing request: {request.Count} bytes.");
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
            var throttle = _connectionOptions.WriteThrottlePeriodInMs;
            long remaining;
            while (true)
            {
                switch (WaitHandle.WaitAny(handles))
                {
                    case 0:
                        return;
                    case 1:
                        WriteRequests(_connectionOptions.WriteStreamBufferSize,
                            _connectionOptions.MaxRequestsInBatch);

                        remaining = Interlocked.Read(ref _remaining);

                        // Thread.Sleep will be called only if the number of pending bytes less than
                        // MinRequestsWithThrottle

                        if (throttle > 0 && remaining < _connectionOptions.MinRequestsWithThrottle)
                            Thread.Sleep(throttle);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void WriteRequests(int bufferLength, int limit)
        {
            void WriteBuffer(ArraySegment<byte> buffer)
            {
                _physicalConnection.Write(buffer.Array, buffer.Offset, buffer.Count);
            }

            bool GetRequest(out ArraySegment<byte> result)
            {
                lock (_lock)
                {
                    if (_buffer.Count > 0)
                    {
                        _remaining = _buffer.Count + 1;
                        result = _buffer.Dequeue();
                        return true;
                    }
                }

                result = default(ArraySegment<byte>);
                return false;
             }

            ArraySegment<byte> request;
            var count = 0;
            UInt64 length = 0;
            var list = new List<ArraySegment<byte>>();
            while (GetRequest(out request))
            {
                _clientOptions?.LogWriter?.WriteLine($"Writing request: {request.Count} bytes.");
                length += (uint)request.Count;

                list.Add(request);
                _clientOptions?.LogWriter?.WriteLine($"Wrote request: {request.Count} bytes.");

                count++;
                if ((limit > 0 && count > limit) || length > (ulong)bufferLength)
                {
                    break;
                }

            }

            if (list.Count > 0)
            {
                // merge requests into one buffer
                var result = new byte[length];
                int position = 0;
                foreach (var r in list)
                {
                    Buffer.BlockCopy(r.Array, r.Offset, result, position, r.Count);
                    position += r.Count;
                }

                WriteBuffer(new ArraySegment<byte>(result));
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
