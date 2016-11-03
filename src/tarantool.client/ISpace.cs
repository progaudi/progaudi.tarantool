using System.Collections.Generic;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

namespace ProGaudi.Tarantool.Client
{
    public interface ISpace
    {
        uint Id { get; }

        uint FieldCount { get; }

        string Name { get; }

        StorageEngine Engine { get; }

        IReadOnlyCollection<Index> Indices { get; }

        IReadOnlyCollection<SpaceField> Fields { get; }

        ILogicalConnection LogicalConnection { get; }

        Task CreateIndex();

        Task Drop();

        Task Rename(string newName);

        Task<Index> GetIndex(string indexName);

        Task<Index> GetIndex(uint indexId);

        Task<DataResponse<TTuple[]>> Insert<TTuple>(TTuple tuple)
            where TTuple : ITarantoolTuple;

        Task<DataResponse<TTuple[]>> Select<TKey, TTuple>(TKey selectKey)
            where TKey : ITarantoolTuple
            where TTuple : ITarantoolTuple;

        Task<TTuple> Get<TKey, TTuple>(TKey key)
            where TKey : ITarantoolTuple
            where TTuple : ITarantoolTuple;

        Task<DataResponse<TTuple[]>> Replace<TTuple>(TTuple tuple)
            where TTuple : ITarantoolTuple;

        Task<T> Put<T>(T tuple)
            where T : ITarantoolTuple;

        Task<DataResponse<TTuple[]>> Update<TKey, TTuple>(TKey key, UpdateOperation[] updateOperations)
            where TKey : ITarantoolTuple
            where TTuple : ITarantoolTuple;

        Task Upsert<TTuple>(TTuple tuple, UpdateOperation[] updateOperations)
            where TTuple : ITarantoolTuple;

        Task<DataResponse<TTuple[]>> Delete<TKey, TTuple>(TKey key)
            where TTuple : ITarantoolTuple
            where TKey : ITarantoolTuple;

        Task<uint> Count<TKey>(TKey key)
            where TKey : ITarantoolTuple;

        Task<uint> Length();

        Task<DataResponse<TTuple[]>> Increment<TTuple, TKey>(TKey key)
            where TKey : ITarantoolTuple
            where TTuple : ITarantoolTuple;

        Task<DataResponse<TTuple[]>> Decrement<TTuple, TKey>(TKey key)
            where TKey : ITarantoolTuple
            where TTuple : ITarantoolTuple;

        TTuple AutoIncrement<TTuple, TRest>(TRest tupleRest)
            where TTuple : ITarantoolTuple
            where TRest : ITarantoolTuple;

        Task<IEnumerable<KeyValuePair<TKey, TValue>>> Pairs<TKey, TValue>();
    }
}
