using System;
using System.Linq;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    internal abstract class PhysicalConnection : IPhysicalConnection
    {
        private bool _disposed;
        private IResponseReader _reader;
        private IRequestWriter _writer;
        private ITaskSource _taskSource;

        protected virtual void Dispose(bool disposing)
        {
            _disposed = true;
            if (!disposing) return;
            _reader?.Dispose();
            _writer?.Dispose();
            _taskSource.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual bool IsConnected => !_disposed;

        public IResponseReader Reader
        {
            get
            {
                CheckConnectionStatus();
                return _reader;
            }
            protected set => _reader = value;
        }

        public IRequestWriter Writer
        {
            get
            {
                CheckConnectionStatus();
                return _writer;
            }
            protected set => _writer = value;
        }

        public ITaskSource TaskSource
        {
            get
            {
                CheckConnectionStatus();
                return _taskSource;
            }
            private set => _taskSource = value;
        }

        private void CheckConnectionStatus()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(NetworkStreamPhysicalConnection));
            }

            if (!IsConnected)
            {
                throw ExceptionHelper.NotConnected();
            }
        }

        public async Task<ReadOnlyMemory<byte>> Connect(ClientOptions options)
        {
            if (! options.ConnectionOptions.Nodes.Any()) 
                throw new ClientSetupException("There are zero configured nodes, you should provide one");

            var singleNode = options.ConnectionOptions.Nodes.Single();

            TaskSource = new TcsSource(options);
            var result = await ConnectAndReadGreeting(options, singleNode);
            Writer.Start();
            Reader.Start();

            return result;
        }

        protected abstract Task<ReadOnlyMemory<byte>> ConnectAndReadGreeting(ClientOptions options, TarantoolNode singleNode);
    }
}