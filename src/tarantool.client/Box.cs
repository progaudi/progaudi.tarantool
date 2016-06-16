using System;
using System.Threading;
using System.Threading.Tasks;

using MsgPack.Light;

using Tarantool.Client.IProto.Data.Packets;
using Tarantool.Client.IProto.Services;

namespace Tarantool.Client
{
    public class Box : IBox
    {
        private const long MaxRequestId = long.MaxValue / 2;

        private readonly IPhysicalConnection _physicalConnection = new NetworkStreamPhysicalConnection();

        private readonly AuthenticationRequestFactory _authenticationRequestFactory = new AuthenticationRequestFactory();

        private readonly MsgPackContext _msgPackContext = MsgPackContextFactory.Create();

        private readonly IResponseReader _responseReader;

        private readonly IRequestWriter _requestWriter;

        private readonly ConnectionOptions _connectionOptions;

        private IConnection _connection;

        public Box(ConnectionOptions options)
        {
            _connectionOptions = options;
            _requestWriter = new RequestWriter(_physicalConnection, _connectionOptions);
            _responseReader = new ResponseReader(_physicalConnection, _requestWriter, _connectionOptions);
        }

        public async Task ConnectAsync()
        {
            _physicalConnection.Connect(_connectionOptions);

            var greetingsResponseBytes = new byte[128];
            var readCount = await _physicalConnection.ReadAsync(greetingsResponseBytes, 0, greetingsResponseBytes.Length);
            if (readCount != greetingsResponseBytes.Length)
            {
                throw ExceptionHelper.UnexpectedGreetingBytesCount(readCount);
            }

            var greetings = new GreetingsResponse(greetingsResponseBytes);

            var authenticateRequest = _authenticationRequestFactory.CreateAuthentication(
                greetings,
                _connectionOptions.UserName,
                _connectionOptions.Password);

            await _connection.SendPacket<AuthenticationPacket, AuthenticationResponse>(authenticateRequest);
        }

        private async Task<byte[]> SendAsync(byte[] request, ulong requestId)
        {
            return await _requestWriter.WriteRequest(request, requestId);
        }

        public void Dispose()
        {
            _physicalConnection.Dispose();
        }
        
        public Schema GetSchemaAsync()
        {
            return new Schema(_connection);
        }

        internal async Task<ResponsePacket<TResult>> SendPacket<TRequest,TResult>(TRequest unifiedPacket) where TRequest : IRequestPacket
        {
            var request = MsgPackSerializer.Serialize(unifiedPacket, _msgPackContext);
            var requestHeaderLength = MsgPackSerializer.Serialize(request.Length, _msgPackContext);
            var responseBytes = await SendBytes(ArrayConcat(requestHeaderLength, request), 0);

            if (responseBytes.Length == 0)
            {
                throw new System.ArgumentException("Zero-length response received, possible wrong packet sent.");
            }

            var response = MsgPackSerializer.Deserialize<ResponsePacket<TResult>>(responseBytes, _msgPackContext);
            return response;
        }
        
        private async Task<byte[]> SendBytes(byte[] requestBytes, ulong requestId)
        {
            var resonse = await SendAsync(requestBytes, requestId);
            return resonse;
        }

        private T[] ArrayConcat<T>(T[] first, T[] second){
            var result = new T[first.Length + second.Length];
            first.CopyTo(result, 0);
            second.CopyTo(result, first.Length);

            return result;
        }

    }
}