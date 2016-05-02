using System.Net.Sockets;
using System.Net;

using iproto;
using iproto.Data;
using iproto.Data.Packets;
using iproto.Services;

using MsgPack.Light;

namespace tarantool_client
{
    public class Connection : System.IDisposable
    {
        private const int VSpace = 281;

        private const int VIndex = 289;

        private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private readonly AuthenticationRequestFactory _authenticationRequestFactory = new AuthenticationRequestFactory();

        private readonly GreetingsResponseReader _responseReader = new GreetingsResponseReader();

        private readonly MsgPackContext _msgPackContext = MsgPackContextFactory.Create();

        public void Connect(string ipAddress, int port)
        {
            var remoteEndpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            _socket.Connect(remoteEndpoint);
        }

        public void Dispose()
        {
            _socket.Dispose();
        }

        public ResponsePacket<object> Login(string userName, string password)
        {
            var greetingsBytes = ReceiveGreetings();
            var greetings = _responseReader.ReadGreetings(greetingsBytes);
            var authenticateRequest = _authenticationRequestFactory.CreateAuthentication(greetings, userName, password);
            var response = SendPacket<AuthenticationPacket,object>(authenticateRequest);
            return response;
        }

        public Schema GetSchema()
        {
            var selectSpacesRequest = new SelectPacket<Tuple<int>>(VSpace, 0, 1000, 0, Iterator.All, Tuple.Create(0));
            var selectSpacesResponse = SendPacket<SelectPacket<Tuple<int>>, object>(selectSpacesRequest);

            var selectIndecesRequest = new SelectPacket<Tuple<int>>(VIndex, 0, 1000, 0, Iterator.All, Tuple.Create(0));
            var selectIndecesResponse = SendPacket<SelectPacket<Tuple<int>>, object>(selectIndecesRequest);
            return null;
        }

        public ResponsePacket<TResult> SendPacket<TRequest,TResult>(TRequest unifiedPacket) where TRequest : UnifiedPacket
        {
            var request = MsgPackSerializer.Serialize(unifiedPacket, _msgPackContext);
            var requestHeaderLength = MsgPackSerializer.Serialize(request.Length, _msgPackContext);
            var responseBytes = SendBytes(requestHeaderLength, request);

            if (responseBytes.Length == 0)
            {
                throw new System.ArgumentException("Zero-length response received, possible wrong packet sent.");
            }

            var response = MsgPackSerializer.Deserialize<ResponsePacket<TResult>>(responseBytes, _msgPackContext);
            return response;
        }

        public ResponsePacket<object> SendPacket<TRequest>(TRequest unifiedPacket) where TRequest : UnifiedPacket
        {
            var request = MsgPackSerializer.Serialize(unifiedPacket, _msgPackContext);
            var requestHeaderLength = MsgPackSerializer.Serialize(request.Length, _msgPackContext);
            var responseBytes = SendBytes(requestHeaderLength, request);

            if (responseBytes.Length == 0)
            {
                throw new System.ArgumentException("Zero-length response received, possible wrong packet sent.");
            }

            var response = MsgPackSerializer.Deserialize<ResponsePacket<object>>(responseBytes, _msgPackContext);
            return response;
        }



        private byte[] ReceiveGreetings()
        {
            _socket.Send(new byte[0]);
            var greetingsResponse = new byte[128];
            _socket.Receive(greetingsResponse);

            return greetingsResponse;
        }

        private byte[] SendBytes(byte[] headerAndBodySize, byte[] headerAndBody)
        {
            if (headerAndBodySize != null)
            {
                _socket.Send(headerAndBodySize);
            }

            _socket.Send(headerAndBody);
            var headerSizeBuffer = new byte[5];
            _socket.Receive(headerSizeBuffer);

            var headerSize = MsgPackSerializer.Deserialize<ulong>(headerSizeBuffer);
            var responseBuffer = new byte[headerSize];
            _socket.Receive(responseBuffer);
            return responseBuffer;
        }
    }
}