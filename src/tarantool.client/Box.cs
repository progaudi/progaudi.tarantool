using System;
using System.Linq;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    public class Box : IBox
    {
        private readonly ClientOptions _clientOptions;

        private readonly ILogicalConnection _logicalConnection;

        public Box(ClientOptions options)
        {
            _clientOptions = options;
            TarantoolConvertersRegistrator.Register(options.MsgPackContext);

            _logicalConnection = new LogicalConnection(options) { GreetingFunc = this.LoginIfNotGuest };
        }

        public async Task Connect()
        {
            await _logicalConnection.Connect();
        }

        public static async Task<Box> Connect(string replicationSource)
        {
            var box = new Box(new ClientOptions(replicationSource));
            await box.Connect();
            return box;
        }

        public static Task<Box> Connect(string host, int port)
        {
            return Connect($"{host}:{port}");
        }

        public static Task<Box> Connect(string host, int port, string user, string password)
        {
            return Connect($"{user}:{password}@{host}:{port}");
        }

        public void Dispose()
        {
            _clientOptions.LogWriter?.WriteLine("Box is disposing...");
            _clientOptions.LogWriter?.Flush();
            _logicalConnection.Dispose();
        }

        public ISchema GetSchema()
        {
            _clientOptions.LogWriter?.WriteLine("Schema acquiring...");
            return new Schema(_logicalConnection);
        }

        public async Task Call_1_6(string functionName)
        {
            await Call_1_6<TarantoolTuple, TarantoolTuple>(functionName, TarantoolTuple.Empty);
        }

        public async Task Call_1_6<TTuple>(string functionName, TTuple parameters)
            where TTuple : ITarantoolTuple
        {
            await Call_1_6<TTuple, TarantoolTuple>(functionName, parameters);
        }

        public Task<DataResponse<TResponse[]>> Call_1_6<TResponse>(string functionName)
            where TResponse : ITarantoolTuple
        {
            return Call_1_6<TarantoolTuple, TResponse>(functionName, TarantoolTuple.Empty);
        }

        public async Task<DataResponse<TResponse[]>> Call_1_6<TTuple, TResponse>(string functionName, TTuple parameters)
            where TTuple : ITarantoolTuple
            where TResponse : ITarantoolTuple
        {
            var callRequest = new CallRequest<TTuple>(functionName, parameters, false);
            return await _logicalConnection.SendRequest<CallRequest<TTuple>, TResponse>(callRequest);
        }

        public async Task Call(string functionName)
        {
            await Call<TarantoolTuple, TarantoolTuple>(functionName, TarantoolTuple.Empty);
        }

        public async Task Call<TTuple>(string functionName, TTuple parameters)
            where TTuple : ITarantoolTuple
        {
            await Call<TTuple, TarantoolTuple>(functionName, parameters);
        }

        public Task<DataResponse<TResponse[]>> Call<TResponse>(string functionName)
        {
            return Call<TarantoolTuple, TResponse>(functionName, TarantoolTuple.Empty);
        }

        public async Task<DataResponse<TResponse[]>> Call<TTuple, TResponse>(string functionName, TTuple parameters)
            where TTuple : ITarantoolTuple
        {
            var callRequest = new CallRequest<TTuple>(functionName, parameters);
            return await _logicalConnection.SendRequest<CallRequest<TTuple>, TResponse>(callRequest);
        }

        public async Task<DataResponse<TResponse[]>> Eval<TTuple, TResponse>(string expression, TTuple parameters)
           where TTuple : ITarantoolTuple
        {
            var evalRequest = new EvalRequest<TTuple>(expression, parameters);
            return await _logicalConnection.SendRequest<EvalRequest<TTuple>, TResponse>(evalRequest);
        }

        public Task<DataResponse<TResponse[]>> Eval<TResponse>(string expression)
        {
            return Eval<TarantoolTuple, TResponse>(expression, TarantoolTuple.Empty);
        }

        private async Task LoginIfNotGuest(GreetingsResponse greetings)
        {
            var singleNode = _clientOptions.ConnectionOptions.Nodes.Single();

            if (string.IsNullOrEmpty(singleNode.Uri.UserName))
            {
                _clientOptions.LogWriter?.WriteLine("Guest mode, no authentication attempt.");
                return;
            }

            var authenticateRequest = AuthenticationRequest.Create(greetings, singleNode.Uri);

            await _logicalConnection.SendRequestWithEmptyResponse(authenticateRequest);
            _clientOptions.LogWriter?.WriteLine($"Authentication request send: {authenticateRequest}");
        }
    }
}