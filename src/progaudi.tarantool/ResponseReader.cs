using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MessagePack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    internal class ResponseReader : IResponseReader
    {
        private static readonly AsyncCallback EndRead = result =>
        {
            ResponseReader reader;
            if (result.CompletedSynchronously || (reader = result.AsyncState as ResponseReader) == null) return;
            if (reader.EndReading(result))
                reader.BeginReading();
        };

        private readonly IPhysicalConnection _physicalConnection;

        private readonly Dictionary<RequestId, Tuple<ResultSetter, ExceptionSetter>> _pendingRequests = new Dictionary<RequestId, Tuple<ResultSetter, ExceptionSetter>>();

        private readonly ReaderWriterLockSlim _pendingRequestsLock = new ReaderWriterLockSlim();

        private readonly ClientOptions _clientOptions;
        private readonly byte[] _originalBuffer;

        private byte[] _buffer;

        private int _readingOffset;

        private int _parsingOffset;

        private bool _disposed;

        public ResponseReader(ClientOptions clientOptions, IPhysicalConnection physicalConnection)
        {
            _physicalConnection = physicalConnection;
            _clientOptions = clientOptions;
            _originalBuffer = _buffer = ArrayPool<byte>.Shared.Rent(1024 * 1024);
        }

        public bool IsConnected => !_disposed;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _disposed = true;

            ArrayPool<byte>.Shared.Return(_originalBuffer);
            var exception = new ObjectDisposedException(nameof(ResponseReader));

            lock (_pendingRequestsLock)
            {
                _clientOptions.LogWriter?.WriteLine("Cancelling all pending requests and setting faulted state...");

                foreach (var value in _pendingRequests.Values)
                {
                    value.Item2(exception);
                }

                _pendingRequests.Clear();
            }
        }

        private delegate void ResultSetter(in ArraySegment<byte> response);

        private delegate void ExceptionSetter(Exception ex);

        public Task<TResponse> GetResponseTask<TResponse>(RequestId requestId, Func<ArraySegment<byte>, TResponse> responseCreator)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ResponseReader));
            }

            lock (_pendingRequestsLock)
            {
                if (_pendingRequests.ContainsKey(requestId))
                {
                    throw ExceptionHelper.RequestWithSuchIdAlreadySent(requestId);
                }

                var tcs = new TaskCompletionSource<TResponse>();
                var completionPair = Tuple.Create<ResultSetter, ExceptionSetter>(ResultSetterImpl, ExceptionSetterImpl);

                _pendingRequests.Add(requestId, completionPair);

                return tcs.Task;

                void ResultSetterImpl(in ArraySegment<byte> response) => tcs.SetResult(responseCreator(response));

                void ExceptionSetterImpl(Exception ex) => tcs.SetException(ex);
            }
        }

        public void BeginReading()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ResponseReader));
            }

            try
            {
                bool keepReading;
                do
                {
                    keepReading = false;
                    var space = EnsureSpaceAndComputeBytesToRead();
                    var result = _physicalConnection.Stream.BeginRead(_buffer, _readingOffset, space, EndRead, this);
                    if (result.CompletedSynchronously)
                    {
                        keepReading = EndReading(result);
                    }
                } while (keepReading);
            }
            catch (IOException ex)
            {
                _clientOptions?.LogWriter?.WriteLine("Could not connect: " + ex.Message);
            }
        }

        private Tuple<ResultSetter, ExceptionSetter> PopResponseCompletionSource(RequestId requestId)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ResponseReader));
            }

            Tuple<ResultSetter, ExceptionSetter> request;

            lock (_pendingRequestsLock)
            {
                if (_pendingRequests.TryGetValue(requestId, out request))
                {
                    _pendingRequests.Remove(requestId);
                }
            }

            return request;
        }

        private bool EndReading(IAsyncResult ar)
        {
            try
            {
                var bytesRead = _physicalConnection?.Stream.EndRead(ar) ?? 0;
                return ProcessReadBytes(bytesRead);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ProcessReadBytes(int readBytesCount)
        {
            if (readBytesCount <= 0)
            {
                _clientOptions.LogWriter?.WriteLine("EOF");
                return false;
            }

            _readingOffset += readBytesCount;
            var parsedResponsesCount = TryParseResponses();
            _clientOptions.LogWriter?.WriteLine("Processed: " + parsedResponsesCount);

            if (!AllBytesProcessed())
            {
                CopyRemainingBytesToBufferBegin();
            }

            return !_disposed;
        }

        private void CopyRemainingBytesToBufferBegin()
        {
            var remainingBytesCount = _readingOffset - _parsingOffset;
            if (remainingBytesCount > 0)
            {
                _clientOptions.LogWriter?.WriteLine("Copying remaining bytes: " + remainingBytesCount);
                Buffer.BlockCopy(_buffer, _parsingOffset, _buffer, 0, remainingBytesCount);
                Array.Clear(_buffer, remainingBytesCount, _buffer.Length - remainingBytesCount);
            }

            _readingOffset = remainingBytesCount;
            _parsingOffset = 0;
        }

        private bool AllBytesProcessed()
        {
            return _parsingOffset >= _readingOffset;
        }

        private int TryParseResponses()
        {
            var messageCount = 0;
            bool nonEmptyResult;
            do
            {
                var response = TryParseResponse();
                nonEmptyResult = response != Empty;
                if (!nonEmptyResult)
                {
                    continue;
                }

                messageCount++;
                MatchResult(response);
            } while (nonEmptyResult);
            return messageCount;
        }

        private void MatchResult(in ArraySegment<byte> result)
        {
            var headerFormatter = MessagePackSerializer.DefaultResolver.GetFormatter<ResponseHeader>();
            var header = headerFormatter.Deserialize(result.Array, result.Offset, MessagePackSerializer.DefaultResolver, out var headerLength);
            var tcs = PopResponseCompletionSource(header.RequestId);

            if (tcs == null)
            {
                if (_clientOptions.LogWriter != null)
                    LogUnMatchedResponse(result, _clientOptions.LogWriter);
                return;
            }

            _clientOptions.LogWriter?.WriteLine($"Match for request with id {header.RequestId} found.");
            if ((header.Code & CommandCodes.ErrorMask) == CommandCodes.ErrorMask)
            {
                var errorFormatter = MessagePackSerializer.DefaultResolver.GetFormatter<ErrorResponse>();
                var errorResponse = errorFormatter.Deserialize(result.Array, result.Offset + headerLength, MessagePackSerializer.DefaultResolver, out _);
                tcs.Item2(ExceptionHelper.TarantoolError(header, errorResponse));
            }
            else
            {
                tcs.Item1(new ArraySegment<byte>(result.Array, result.Offset + headerLength, result.Count - headerLength));
            }
        }

        private static void LogUnMatchedResponse(Span<byte> result, [NotNull]ILog logWriter)
        {
            var builder = new StringBuilder("Warning: can't match request via requestId from response. Response:");
            var length = 80/3;
            for (var i = 0; i < result.Length; i++)
            {
                if (i%length == 0)
                    builder.AppendLine().Append("   ");
                else
                    builder.Append(" ");

                builder.AppendFormat("{0:X2}", result[i]);
            }

            logWriter.WriteLine(builder.ToString());
        }

        private static readonly ArraySegment<byte> Empty = new ArraySegment<byte>(Array.Empty<byte>());
        private ArraySegment<byte> TryParseResponse()
        {
            if (AllBytesProcessed())
            {
                return Empty;
            }

            var packetSize = _readingOffset - _parsingOffset < Constants.PacketSizeBufferSize
                ? 0
                : (int)MessagePackBinary.ReadUInt64(_buffer, _parsingOffset, out _);

            if (packetSize == 0)
            {
                _clientOptions.LogWriter?.WriteLine($"Can't read packet length, has less than {Constants.PacketSizeBufferSize} bytes.");
                return Empty;
            }

            if (PacketCompletelyRead(packetSize))
            {
                var offset = _parsingOffset += Constants.PacketSizeBufferSize;
                _parsingOffset += packetSize;

                return new ArraySegment<byte>(_buffer, offset, packetSize);
            }

            _clientOptions.LogWriter?.WriteLine($"Packet with length {packetSize} is not completely read.");

            return Empty;
        }

        private bool PacketCompletelyRead(int packetSize)
        {
            return packetSize <= _readingOffset - _parsingOffset - Constants.PacketSizeBufferSize;
        }

        private int EnsureSpaceAndComputeBytesToRead()
        {
            var space = _buffer.Length - _readingOffset;
            if (space != 0)
            {
                return space;
            }

            _clientOptions.LogWriter?.WriteLine($"Resizing buffer from {_buffer.Length} to {_buffer.Length * 2}");

            Array.Resize(ref _buffer, _buffer.Length * 2);
            space = _buffer.Length - _readingOffset;
            return space;
        }
    }
}