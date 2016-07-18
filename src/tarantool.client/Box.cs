using System;
using System.Threading.Tasks;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Requests;
using Tarantool.Client.Model.Responses;
using Tarantool.Client.Utils;

namespace Tarantool.Client
{
    public class Box : IDisposable
    {
        private readonly ConnectionOptions _connectionOptions;

        private readonly ILogicalConnection _logicalConnection;

        private readonly IResponseReader _responseReader;

        private readonly INetworkStreamPhysicalConnection _physicalConnection;

        public Box(ConnectionOptions options)
        {
            _connectionOptions = options;
            TarantoolConvertersRegistrator.Register(options.MsgPackContext);

            _physicalConnection = new NetworkStreamPhysicalConnection();
            _logicalConnection = new LogicalConnection(options, _physicalConnection);
            _responseReader = new ResponseReader(_logicalConnection, options, _physicalConnection);
        }

        public async Task Connect()
        {
            _physicalConnection.Connect(_connectionOptions);

            var greetingsResponseBytes = new byte[128];
            var readCount = await _physicalConnection.Read(greetingsResponseBytes, 0, greetingsResponseBytes.Length);
            if (readCount != greetingsResponseBytes.Length)
            {
                throw ExceptionHelper.UnexpectedGreetingBytesCount(readCount);
            }

            var greetings = new GreetingsResponse(greetingsResponseBytes);

            _connectionOptions.LogWriter?.WriteLine($"Greetings received, salt is {Convert.ToBase64String(greetings.Salt)} .");

            _responseReader.BeginReading();

            _connectionOptions.LogWriter?.WriteLine("Server responses reading started.");

            await LoginIfNotGuest(greetings);
        }

        public void Dispose()
        {
            _connectionOptions.LogWriter?.WriteLine("Box is disposing...");
            _connectionOptions.LogWriter?.Flush();
            _responseReader.Dispose();
            _physicalConnection.Dispose();
        }

        public Schema GetSchema()
        {
            _connectionOptions.LogWriter?.WriteLine("Schema acquiring...");
            return new Schema(_logicalConnection);
        }

        public async Task<DataResponse<TResponse[]>> Call<TTuple, TResponse>(string functionName, TTuple request)
            where TTuple : ITuple
            where TResponse : ITuple
        {
            var callRequest = new CallRequest<TTuple>(functionName, request);
            return await _logicalConnection.SendRequest<CallRequest<TTuple>, TResponse>(callRequest);
        }

        public async Task<DataResponse<TResponse[]>> Eval<TTuple, TResponse>(string expression, TTuple request)
           where TTuple : ITuple
        {
            var evalRequest = new EvalRequest<TTuple>(expression, request);
            return await _logicalConnection.SendRequest<EvalRequest<TTuple>, TResponse>(evalRequest);
        }

        private async Task LoginIfNotGuest(GreetingsResponse greetings)
        {
            if (string.IsNullOrEmpty(_connectionOptions.UserName))
            {
                if (!_connectionOptions.GuestMode)
                {
                    throw ExceptionHelper.EmptyUsernameInGuestMode();
                }
            }
            else
            {
                if (_connectionOptions.GuestMode)
                {
                    _connectionOptions.LogWriter?.WriteLine("Guest mode, no authentication attempt.");

                    return;
                }

                var authenticateRequest = AuthenticationRequest.Create(
                    greetings,
                    _connectionOptions.UserName,
                    _connectionOptions.Password);

                await _logicalConnection.SendRequestWithEmptyResponse(authenticateRequest);
                _connectionOptions.LogWriter?.WriteLine($"Authentication request send: {authenticateRequest}");
            }
        }
    }
}