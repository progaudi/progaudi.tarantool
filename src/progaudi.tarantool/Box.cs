using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    public class Box : IBox
    {
        private readonly ClientOptions _clientOptions;

        private readonly ILogicalConnection _logicalConnection;

        private BoxInfo _info;

        private bool _sqlReady;

        public Box(ClientOptions options)
        {
            _clientOptions = options;
            TarantoolConvertersRegistrator.Register(options.MsgPackContext);

            _logicalConnection = new LogicalConnection(options, new RequestIdCounter());
            Metrics = new Metrics(_logicalConnection);
            Schema = new Schema(_logicalConnection);
        }

        public Metrics Metrics { get; }

        public bool IsConnected => _logicalConnection.IsConnected();

        public ISchema Schema { get; }

        public ILuaCode<TResult> GetLuaFunc<TResult>(string name)
        {
            throw new NotImplementedException();
        }

        public ILuaCode<TResult> GetLuaCode<TResult>(string code)
        {
            throw new NotImplementedException();
        }

        public BoxInfo Info
        {
            get => _info;
            private set
            {
                _info = value;
                _sqlReady = value.IsSqlAvailable();
            }
        }

        public async Task Connect()
        {
            await _logicalConnection.Connect().ConfigureAwait(false);
            await Task.WhenAll(GetAdditionalTasks()).ConfigureAwait(false);

            IEnumerable<Task> GetAdditionalTasks()
            {
                if (_clientOptions.ConnectionOptions.ReadSchemaOnConnect)
                    yield return ReloadSchema();

                if (_clientOptions.ConnectionOptions.ReadBoxInfoOnConnect)
                    yield return ReloadBoxInfo();
            }
        }

        public Task ReloadBoxInfo()
        {
            //var report = await Eval<BoxInfo, >("return box.info").ConfigureAwait(false);
            //if (report.Data.Length != 1) throw ExceptionHelper.CantParseBoxInfoResponse();
            //Info = report.Data[0];
            throw new NotImplementedException();
        }

        public static async Task<Box> Connect(string replicationSource)
        {
            var box = new Box(new ClientOptions(replicationSource));
            await box.Connect().ConfigureAwait(false);
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

        public ISchema GetSchema() => Schema;

        public Task ReloadSchema()
        {
            _clientOptions.LogWriter?.WriteLine("Schema reloading...");
            return Schema.Reload();
        }

        public async Task<DataResponse<TResponse[]>> Call<TTuple, TResponse>(string functionName, TTuple parameters)
        {
            var callRequest = new CallRequest<TTuple>(functionName, parameters);
            var result = await _logicalConnection.SendRequest<CallRequest<TTuple>, TResponse>(callRequest);
            return result.Data;
        }

        public async Task<DataResponse<TResponse[]>> Eval<TTuple, TResponse>(string expression, TTuple parameters)
        {
            var evalRequest = new EvalRequest<TTuple>(expression, parameters);
            var result = await _logicalConnection.SendRequest<EvalRequest<TTuple>, TResponse>(evalRequest);
            return result.Data;
        }

        //public Task<DataResponse> ExecuteSql(string query, params SqlParameter[] parameters)
        //{
        //    if (!_sqlReady) throw ExceptionHelper.SqlIsNotAvailable(Info.Version);

        //    return _logicalConnection.SendRequest(new ExecuteSqlRequest(query, parameters));
        //}

        //public Task<DataResponse<TResponse[]>> ExecuteSql<TResponse>(string query, params SqlParameter[] parameters)
        //{
        //    if (!_sqlReady) throw ExceptionHelper.SqlIsNotAvailable(Info.Version);

        //    return _logicalConnection.SendRequest<ExecuteSqlRequest, TResponse>(new ExecuteSqlRequest(query, parameters));
        //}
    }
}