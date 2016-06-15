using System.Net;
using System.Threading;
using System.Threading.Tasks;

using MsgPack.Light;

using Tarantool.Client.IProto;
using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;
using Tarantool.Client.IProto.Services;

namespace Tarantool.Client
{
    public class Multiplexer : System.IDisposable
    {
        private const int VSpace = 281;

        private const int VIndex = 289;

        private const long MaxRequestId = long.MaxValue / 2;

        private readonly IConnection _connection = new Connection();

        private readonly AuthenticationRequestFactory _authenticationRequestFactory = new AuthenticationRequestFactory();

        private readonly GreetingsResponseReader _responseReader = new GreetingsResponseReader();

        private readonly MsgPackContext _msgPackContext = MsgPackContextFactory.Create();

        private long _currentRequestId = 0;

        public async Task ConnectAsync(string ipAddress, int port)
        {
            var remoteEndpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            await _connection.ConnectAsync(remoteEndpoint);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public async Task<ResponsePacket<object>> Login(string userName, string password)
        {
            var greetingsBytes = await ReceiveGreetings();
            var greetings = _responseReader.ReadGreetings(greetingsBytes);
            var authenticateRequest = _authenticationRequestFactory.CreateAuthentication(greetings, userName, password);
            var response = await SendPacket<AuthenticationPacket,object>(authenticateRequest);
            return response;
        }

        public async Task<Schema> GetSchemaAsync()
        {
            var selectIndecesRequest = new SelectPacket<Tuple<int>>(VIndex, 0, uint.MaxValue, 0, Iterator.All, Tuple.Create(0));
            var selectIndecesResponse = await SendPacket<SelectPacket<Tuple<int>>, Index[]>(selectIndecesRequest);

            var selectSpacesRequest = new SelectPacket<Tuple<int>>(VSpace, 0, uint.MaxValue, 0, Iterator.All, Tuple.Create(0));
            var selectSpacesResponse = await SendPacket<SelectPacket<Tuple<int>>, Space[]>(selectSpacesRequest);

            return new Schema(selectIndecesResponse.Data, selectSpacesResponse.Data, this);
        }

        public async Task<ResponsePacket<TResult>> SendPacket<TRequest,TResult>(TRequest unifiedPacket) where TRequest : UnifiedPacket
        {
            unifiedPacket.Header.Sync = GetNextRequestId();
            var request = MsgPackSerializer.Serialize(unifiedPacket, _msgPackContext);
            var requestHeaderLength = MsgPackSerializer.Serialize(request.Length, _msgPackContext);
            var responseBytes = await SendBytes(ArrayConcat(requestHeaderLength, request), unifiedPacket.Header.Sync.Value);

            if (responseBytes.Length == 0)
            {
                throw new System.ArgumentException("Zero-length response received, possible wrong packet sent.");
            }

            var response = MsgPackSerializer.Deserialize<ResponsePacket<TResult>>(responseBytes, _msgPackContext);
            return response;
        }

        private ulong GetNextRequestId()
        {
            Interlocked.CompareExchange(ref _currentRequestId, 0, MaxRequestId);
            Interlocked.Increment(ref _currentRequestId);

            return (ulong)_currentRequestId;
        }

        private async Task<byte[]> ReceiveGreetings()
        {
            var greetingsResponse = await _connection.SendAsync(new byte[0], 128);
            return greetingsResponse;
        }

        private async Task<byte[]> SendBytes(byte[] requestBytes, ulong requestId)
        {
            var resonse = await _connection.SendAsync(requestBytes, requestId);
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