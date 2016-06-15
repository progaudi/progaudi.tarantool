using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using MsgPack.Light;

namespace tarantool_client
{
    internal class Connection : IConnection
    {
        private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private readonly ILog _log;

        private IResponseReader _responseReader;

        private IRequestWriter _requestWriter;

        private Stream _networkStream;

        public async Task ConnectAsync(EndPoint endPoint)
        {
            _socket.Connect(endPoint);

            _networkStream = new NetworkStream(_socket);

            _requestWriter = new RequestWriter(_networkStream, _log);
            _responseReader = new ResponseReader(_networkStream, _requestWriter, _log, null);

            await _responseReader.BeginReading();
        }

        public async Task<byte[]> SendAsync(byte[] request, ulong requestId)
        {
            return await _requestWriter.WriteRequest(request, requestId);
        }

        #region IDisposable Implementation

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _networkStream.Dispose();
                    _socket.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
};
