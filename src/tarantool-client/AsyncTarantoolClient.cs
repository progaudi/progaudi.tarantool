using System;
using System.Net.Sockets;
using System.IO;
using System.Net;

using iproto.Data.Packets;
using iproto.Interfaces;
using iproto.Services;

using TarantoolDnx.MsgPack;

namespace tarantool_client
{

    public class AsyncTarantoolClient : IDisposable
    {
        private readonly Socket _socket;

        private readonly IResponseReader _responseReader;

        private readonly IRequestFactory _requestFactory;

        private readonly MsgPackContext _msgPackContext;
        
        public AsyncTarantoolClient()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _responseReader = new ResponseReader();
            _requestFactory = new RequestFactory();
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
            var greetingsBytes = Send(new byte[0]);
            var greetings = _responseReader.ReadGreetings(greetingsBytes);
            var authenticateRequest = _requestFactory.CreateAuthentication(greetings, userName, password);
            var loginResponse = Send(authenticateRequest);

            var response = _responseReader.ReadResponse(loginResponse, _msgPackContext);
        }

        private byte[] Send(UnifiedPacket authenticateRequest)
        {
            var serializedRequest = authenticateRequest.Serialize(_msgPackContext);
           return Send(serializedRequest);
        }

        private byte[] Send(byte[] request)
        {
            _socket.Send(request);

            using (var memoryStream = new MemoryStream())
            {
                const int readBufferSize = 1024;
                var responseBuffer = new byte[readBufferSize];
                int readCount;

                do
                {
                    readCount = _socket.Receive(responseBuffer, 0, readBufferSize, 0);
                    memoryStream.Write(responseBuffer, 0, readCount);
                } while (readCount == readBufferSize);

                return memoryStream.ToArray();
            }
        }

    }
}