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

        Task<IEnumerable<TResult>> Pairs<TValue, TResult>(TValue value, Iterator iterator);

        Task<DataResponse<TTuple[]>> Select<TKey, TTuple>(TKey key, SelectOptions options = null);

        ///Note: there is no such method in specification http://tarantool.org/doc/book/box/box_index.html.
        ///But common sense, and sources https://github.com/tarantool/tarantool/blob/1.7/src/box/lua/index.c says that that method sould be
        Task<DataResponse<TTuple[]>> Insert<TTuple>(TTuple tuple);

        ///Note: there is no such method in specification http://tarantool.org/doc/book/box/box_index.html.
        ///But common sense, and sources https://github.com/tarantool/tarantool/blob/1.7/src/box/lua/index.c says that that method sould be
        Task<DataResponse<TTuple[]>> Replace<TTuple>(TTuple tuple);

        Task<TTuple> Min<TTuple>();

        Task<TTuple> Min<TTuple, TKey>(TKey key);

        Task<TTuple> Max<TTuple>();

        Task<TTuple> Max<TTuple, TKey>(TKey key);

        TTuple Random<TTuple>(int randomValue);

        uint Count<TKey>(TKey key, Iterator it = Iterator.Eq);

        Task<DataResponse<TTuple[]>> Update<TTuple, TKey>(TKey key, UpdateOperation[] updateOperations);

        Task Upsert<TKey>(TKey key, UpdateOperation[] updateOperations);

        Task<DataResponse<TTuple[]>> Delete<TKey, TTuple>(TKey key);

        Task Alter(IndexCreationOptions options);

        Task Drop();

        Task Rename(string indexName);

        Task<uint> BSize();
    }
}
