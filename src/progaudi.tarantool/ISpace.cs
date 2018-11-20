using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

namespace ProGaudi.Tarantool.Client
{
    public interface ISpace
    {
        uint Id { get; }

        uint FieldCount { get; }

        string Name { get; }

        string Engine { get; }

        IReadOnlyCollection<IIndex> Indices { get; }

        IReadOnlyCollection<SpaceField> Fields { get; }

        ILogicalConnection LogicalConnection { get; }

        [Obsolete("Use indexer")]
        Task<IIndex> GetIndex(string indexName);

        [Obsolete("Use indexer")]
        Task<IIndex> GetIndex(uint indexId);

        IIndex this[string name] { get; }

        IIndex this[uint id] { get; }

        Task<DataResponse<TTuple[]>> Insert<TTuple>(TTuple tuple);

        Task<DataResponse<TTuple[]>> Select<TKey, TTuple>(TKey selectKey);

        Task<TTuple> Get<TKey, TTuple>(TKey key);

        Task<DataResponse<TTuple[]>> Replace<TTuple>(TTuple tuple);

        Task<T> Put<T>(T tuple);

        Task<DataResponse<TTuple[]>> Update<TKey, TTuple>(TKey key, UpdateOperation[] updateOperations);

        Task Upsert<TTuple>(TTuple tuple, UpdateOperation[] updateOperations);

        Task<DataResponse<TTuple[]>> Delete<TKey, TTuple>(TKey key);

        Task<uint> Count<TKey>(TKey key);

        Task<uint> Length();

        Task<DataResponse<TTuple[]>> Increment<TTuple, TKey>(TKey key);

        Task<DataResponse<TTuple[]>> Decrement<TTuple, TKey>(TKey key);
    }
}
