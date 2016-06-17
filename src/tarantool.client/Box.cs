using System;
using System.Threading.Tasks;

using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client
{
    public class Box : IBox
    {
        private readonly ConnectionOptions _connectionOptions;

        private readonly ILogicalConnection _logicalConnection;

        private readonly IResponseReader _responseReader;

        public Box(ConnectionOptions options)
        {
            _connectionOptions = options;
            _logicalConnection = new LogicalConnection(options);
            _responseReader = options.ResponseReaderFactory.Create(_logicalConnection, options);
        }

        public async Task ConnectAsync()
        {
            _connectionOptions.PhysicalConnection.Connect(_connectionOptions);
         
            var greetingsResponseBytes = new byte[128];
            var readCount = await _connectionOptions.PhysicalConnection.ReadAsync(greetingsResponseBytes, 0, greetingsResponseBytes.Length);
            if (readCount != greetingsResponseBytes.Length)
            {
                throw ExceptionHelper.UnexpectedGreetingBytesCount(readCount);
            }

            var greetings = new GreetingsResponse(greetingsResponseBytes);

            _responseReader.BeginReading();

            await TryLogin(greetings);
        }

        private async Task TryLogin(GreetingsResponse greetings)
        {
            if (string.IsNullOrEmpty(_connectionOptions.UserName))
            {
                if (!_connectionOptions.GuestMode)
                {
                    throw new InvalidOperationException("Empty username in non-guest mode!");
                }
            }
            else
            {
                var authenticateRequest = AuthenticationPacket.Create(
                    greetings,
                    _connectionOptions.UserName,
                    _connectionOptions.Password);

                await _logicalConnection.SendRequest<AuthenticationPacket, AuthenticationResponse>(authenticateRequest);
            }
        }

        public void Dispose()
        {
            _connectionOptions.PhysicalConnection.Dispose();
        }
        
        public Schema GetSchemaAsync()
        {
            return new Schema(_logicalConnection);
        }
    }
}