using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Headers;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    public sealed class TcsSource : ITaskSource
    {
        private readonly Dictionary<RequestId, TaskCompletionSource<ReadOnlySequence<byte>>> _pendingRequests =
            new Dictionary<RequestId, TaskCompletionSource<ReadOnlySequence<byte>>>();

        private readonly ReaderWriterLockSlim _pendingRequestsLock = new ReaderWriterLockSlim();

        private bool _disposed;
        private readonly IMsgPackSequenceParser<ResponseHeader> _headerParser;
        private readonly IMsgPackSequenceParser<ErrorResponse> _errorParser;
        private readonly ILog _logWriter;

        public TcsSource(ClientOptions clientOptions)
        {
            _headerParser = clientOptions.MsgPackContext.GetRequiredSequenceParser<ResponseHeader>();
            _errorParser = clientOptions.MsgPackContext.GetRequiredSequenceParser<ErrorResponse>();
            _logWriter = clientOptions.LogWriter;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _pendingRequestsLock.EnterWriteLock();

                _logWriter?.WriteLine("Cancelling all pending requests and setting faulted state...");

                foreach (var response in _pendingRequests.Values)
                {
                    response.SetException(new ObjectDisposedException(nameof(SocketResponseReader)));
                }

                _pendingRequests.Clear();
            }
            finally
            {
                _pendingRequestsLock.ExitWriteLock();
            }

            _pendingRequestsLock.Dispose();
        }

        public Task<ReadOnlySequence<byte>> GetResponseTask(in RequestId requestId)
        {
            try
            {
                _pendingRequestsLock.EnterWriteLock();

                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(SocketResponseReader));
                }

                if (_pendingRequests.ContainsKey(requestId))
                {
                    throw ExceptionHelper.RequestWithSuchIdAlreadySent(requestId);
                }

                var tcs = new TaskCompletionSource<ReadOnlySequence<byte>>();
                _pendingRequests.Add(requestId, tcs);

                return tcs.Task;
            }
            finally
            {
                _pendingRequestsLock.ExitWriteLock();
            }
        }

        public void MatchResult(in ReadOnlyMemory<byte> result) => MatchResult(new ReadOnlySequence<byte>(result));

        public void MatchResult(in ReadOnlySequence<byte> sequence)
        {
            var readSize = 0;
            var header = _headerParser.Parse(sequence, out var temp);
            readSize += temp;
            var tcs = PopResponseCompletionSource(header.Id);

            if (tcs == null)
            {
                _logWriter?.WriteLine($"Warning: can't match request via requestId from response. Response:\n{ByteUtils.ToReadableString(sequence)}");

                return;
            }

            if ((header.Code & CommandCode.ErrorMask) == CommandCode.ErrorMask)
            {
                var errorResponse = _errorParser.Parse(sequence.Slice(readSize), out _);
                tcs.SetException(ExceptionHelper.TarantoolError(header, errorResponse));
            }
            else
            {
                _logWriter?.WriteLine($"Match for request with id {header.Id} found.");
                tcs.SetResult(sequence.Slice(readSize));
            }
        }

        private TaskCompletionSource<ReadOnlySequence<byte>> PopResponseCompletionSource(RequestId requestId)
        {
            try
            {
                _pendingRequestsLock.EnterWriteLock();

                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(SocketResponseReader));
                }

                if (_pendingRequests.TryGetValue(requestId, out var request))
                {
                    _pendingRequests.Remove(requestId);
                }

                return request;
            }
            finally
            {
                _pendingRequestsLock.ExitWriteLock();
            }
        }
    }
}