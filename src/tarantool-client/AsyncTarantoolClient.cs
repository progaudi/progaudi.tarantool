using System;
using System.Net.Sockets;
using System.Net;

using iproto.Data.Packets;
using iproto.Interfaces;
using iproto.Services;

using TarantoolDnx.MsgPack;
using TarantoolDnx.MsgPack.Converters;

namespace tarantool_client
{

    public class AsyncTarantoolClient : IDisposable
    {
        private readonly Socket _socket;

        private readonly IRequestFactory _requestFactory;

        private readonly GreetingsResponseReader _responseReader;

        private readonly MsgPackContext _msgPackContext;

        public AsyncTarantoolClient()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _requestFactory = new RequestFactory();
            _responseReader = new GreetingsResponseReader();
            _msgPackContext = MsgPackContextFactory.Create();
        }

        public void Connect(string ipAddress, int port)
        {
            var remoteEndpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            _socket.Connect(remoteEndpoint);
        }

        public void Dispose()
        {
            _socket.Dispose();
        }

        public void Login(string userName, string password)
        {
            var greetingsBytes = ReceiveGreetings();
            var greetings = _responseReader.ReadGreetings(greetingsBytes);
            var authenticateRequest = _requestFactory.CreateAuthentication(greetings, userName, password);
            var responseBytes = SendPacket(authenticateRequest);
            var response = MsgPackConverter.Deserialize<ResponsePacket>(responseBytes, _msgPackContext);
        }

        private byte[] ReceiveGreetings()
        {
            _socket.Send(new byte[0]);
            var greetingsResponse = new byte[128];
            _socket.Receive(greetingsResponse);

            return greetingsResponse;
        }

        private byte[] SendPacket<T>(T authenticateUnified) where T : UnifiedPacket
        {
            var request = MsgPackConverter.Serialize(authenticateUnified, _msgPackContext);
            var requestHeaderLength = MsgPackConverter.Serialize(request.Length, _msgPackContext);
            return SendBytes(requestHeaderLength, request);
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

            var headerSize = MsgPackConverter.Deserialize<ulong>(headerSizeBuffer);
            var responseBuffer = new byte[headerSize];
            _socket.Receive(responseBuffer);
            return responseBuffer;
        }
    }
}