using System;
using System.Linq;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    public class Box : IDisposable
    {
        private readonly ClientOptions _clientOptions;

        private readonly ILogicalConnection _logicalConnection;

        private readonly IResponseReader _responseReader;

        private readonly INetworkStreamPhysicalConnection _physicalConnection;

        public Box(ClientOptions options)
        {
            _clientOptions = options;
            TarantoolConvertersRegistrator.Register(options.MsgPackContext);

            _physicalConnection = new NetworkStreamPhysicalConnection();
            _logicalConnection = new LogicalConnection(options, _physicalConnection);
            _responseReader = new ResponseReader(_logicalConnection, options, _physicalConnection);
        }

        public async Task Connect()
        {
            _physicalConnection.Connect(_clientOptions);

            var greetingsResponseBytes = new byte[128];
            var readCount = await _physicalConnection.ReadAsync(greetingsResponseBytes, 0, greetingsResponseBytes.Length);
            if (readCount != greetingsResponseBytes.Length)
            {
                throw ExceptionHelper.UnexpectedGreetingBytesCount(readCount);
            }

            var greetings = new GreetingsResponse(greetingsResponseBytes);

            _clientOptions.LogWriter?.WriteLine($"Greetings received, salt is {Convert.ToBase64String(greetings.Salt)} .");

            _responseReader.BeginReading();

            _clientOptions.LogWriter?.WriteLine("Server responses reading started.");

            await LoginIfNotGuest(greetings);
        }

        public void Dispose()
        {
            _clientOptions.LogWriter?.WriteLine("Box is disposing...");
            _clientOptions.LogWriter?.Flush();
            _responseReader.Dispose();
            _physicalConnection.Dispose();
        }

        public Schema GetSchema()
        {
            _clientOptions.LogWriter?.WriteLine("Schema acquiring...");
            return new Schema(_logicalConnection);
        }

        public async Task<DataResponse<TResponse[]>> Call<TTuple, TResponse>(string functionName, TTuple request)
            where TTuple : ITarantoolTuple
            where TResponse : ITarantoolTuple
        {
            var callRequest = new CallRequest<TTuple>(functionName, request);
            return await _logicalConnection.SendRequest<CallRequest<TTuple>, TResponse>(callRequest);
        }

        public async Task<DataResponse<TResponse[]>> Eval<TTuple, TResponse>(string expression, TTuple request)
           where TTuple : ITarantoolTuple
        {
            var evalRequest = new EvalRequest<TTuple>(expression, request);
            return await _logicalConnection.SendRequest<EvalRequest<TTuple>, TResponse>(evalRequest);
        }

        private async Task LoginIfNotGuest(GreetingsResponse greetings)
        {
            var singleNode = _clientOptions.NodeOptions.Single();
            if (string.IsNullOrEmpty(singleNode.UserName))
            {
                if (!singleNode.GuestMode)
                {
                    throw ExceptionHelper.EmptyUsernameInGuestMode();
                }
            }
            else
            {
                if (singleNode.GuestMode)
                {
                    _clientOptions.LogWriter?.WriteLine("Guest mode, no authentication attempt.");

                    return;
                }

                var authenticateRequest = AuthenticationRequest.Create(
                    greetings,
                    singleNode.UserName,
                    singleNode.Password);

                await _logicalConnection.SendRequestWithEmptyResponse(authenticateRequest);
                _clientOptions.LogWriter?.WriteLine($"Authentication request send: {authenticateRequest}");
            }
        }
    }
}