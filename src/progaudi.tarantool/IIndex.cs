using System.Collections.Generic;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

namespace ProGaudi.Tarantool.Client
{
    public interface IIndex
    {
        ILogicalConnection LogicalConnection { get; }

        uint Id { get; }

        uint SpaceId { get; }

        string Name { get; }

        bool Unique { get; }

        IndexType Type { get; }

        IReadOnlyList<IndexPart> Parts { get; }

        Task<IEnumerable<TResult>> Pairs<TValue, TResult>(TValue value, Iterator iterator)
            where TResult : ITarantoolTuple;

        Task<DataResponse<TTuple[]>> Select<TKey, TTuple>(TKey key, SelectOptions options = null)
            where TKey : ITarantoolTuple
            where TTuple : ITarantoolTuple;

        ///Note: there is no such method in specification http://tarantool.org/doc/book/box/box_index.html.
        ///But common sense, and sources https://github.com/tarantool/tarantool/blob/1.7/src/box/lua/index.c says that that method sould be
        Task<DataResponse<TTuple[]>> Insert<TTuple>(TTuple tuple)
            where TTuple : ITarantoolTuple;

        ///Note: there is no such method in specification http://tarantool.org/doc/book/box/box_index.html.
        ///But common sense, and sources https://github.com/tarantool/tarantool/blob/1.7/src/box/lua/index.c says that that method sould be
        Task<DataResponse<TTuple[]>> Replace<TTuple>(TTuple tuple)
            where TTuple : ITarantoolTuple;

        Task<TTuple> Min<TTuple>()
            where TTuple : ITarantoolTuple;

        Task<TTuple> Min<TTuple, TKey>(TKey key)
            where TTuple : ITarantoolTuple
            where TKey : class, ITarantoolTuple;

        Task<TTuple> Max<TTuple>()
            where TTuple : ITarantoolTuple;

        Task<TTuple> Max<TTuple, TKey>(TKey key = null)
            where TTuple : ITarantoolTuple
            where TKey : class, ITarantoolTuple;

        TTuple Random<TTuple>(int randomValue)
            where TTuple : ITarantoolTuple;

        uint Count<TKey>(TKey key = null, Iterator it = Iterator.Eq)
            where TKey : class, ITarantoolTuple;

        Task<DataResponse<TTuple[]>> Update<TTuple, TKey>(TKey key, UpdateOperation[] updateOperations)
            where TKey : ITarantoolTuple
            where TTuple : ITarantoolTuple;

        Task Upsert<TKey>(TKey key, UpdateOperation[] updateOperations)
            where TKey : ITarantoolTuple;

        Task<DataResponse<TTuple[]>> Delete<TKey, TTuple>(TKey key)
            where TKey : ITarantoolTuple
            where TTuple : ITarantoolTuple;

        Task Alter(IndexCreationOptions options);

        Task Drop();

        Task Rename(string indexName);

        Task<uint> BSize();
    }
}
