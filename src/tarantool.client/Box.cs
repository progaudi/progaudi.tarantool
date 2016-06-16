using System.Threading;
using System.Threading.Tasks;

using Tarantool.Client.IProto.Data.Packets;
using Tarantool.Client.IProto.Services;

namespace Tarantool.Client
{
    public class Box : IBox
    {
        private readonly IPhysicalConnection _physicalConnection = new NetworkStreamPhysicalConnection();

        private readonly AuthenticationRequestFactory _authenticationRequestFactory = new AuthenticationRequestFactory();

        private readonly ConnectionOptions _connectionOptions;

        private readonly IRequestWriter _requestWriter;

        private readonly IResponseReader _responseReader;

        public Box(ConnectionOptions options)
        {
            _connectionOptions = options;
            _requestWriter = new RequestWriter(options.MsgPackContext, _physicalConnection);
            _responseReader = new ResponseReader(_physicalConnection, _requestWriter, options);
        }

        public async Task ConnectAsync()
        {
            _physicalConnection.Connect(_connectionOptions);
         
            var greetingsResponseBytes = new byte[128];
            var readCount = _physicalConnection.Read(greetingsResponseBytes, 0, greetingsResponseBytes.Length);
            if (readCount != greetingsResponseBytes.Length)
            {
                throw ExceptionHelper.UnexpectedGreetingBytesCount(readCount);
            }

            var readerThread = new Thread(_responseReader.BeginReading);
            readerThread.Start();

            var greetings = new GreetingsResponse(greetingsResponseBytes);

            var authenticateRequest = _authenticationRequestFactory.CreateAuthentication(
                greetings,
                _connectionOptions.UserName,
                _connectionOptions.Password);

            await _requestWriter.SendRequest<AuthenticationPacket, AuthenticationResponse>(authenticateRequest);
            
        }
        
        public void Dispose()
        {
            _physicalConnection.Dispose();
        }
        
        public Schema GetSchemaAsync()
        {
            return new Schema(_requestWriter);
        }
    }
}