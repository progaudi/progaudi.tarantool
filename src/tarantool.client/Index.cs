using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Requests;
using Tarantool.Client.Model.Responses;
using Tarantool.Client.Model.UpdateOperations;
using Tarantool.Client.Utils;

using Tuple = Tarantool.Client.Model.Tuple;

namespace Tarantool.Client
{
    public class Index
    {
        public ILogicalConnection LogicalConnection { get; set; }

        public Index(uint id, uint spaceId, string name, bool unique, IndexType type, IReadOnlyList<IndexPart> parts)
        {
            Id = id;
            SpaceId = spaceId;
            Name = name;
            Unique = unique;
            Type = type;
            Parts = parts;
        }

        public uint Id { get; }

        public uint SpaceId { get; }

        public string Name { get; }

        public bool Unique { get; }

        public IndexType Type { get; }

        public IReadOnlyList<IndexPart> Parts { get; }

        public IEnumerable<TResult> Pairs<TValue, TResult>(TValue value, Iterator iterator)
            where TResult : ITuple
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<TTuple[]>> Select<TKey, TTuple>(TKey key, SelectOptions options = null)
            where TKey : ITuple
            where TTuple : ITuple
        {
            var selectRequest = new SelectRequest<TKey>(
                SpaceId,
                Id,
                options?.Limit ?? uint.MaxValue,
                options?.Offset ?? 0,
                options?.Iterator ?? Iterator.Eq,
                key);

            return await LogicalConnection.SendRequest<SelectRequest<TKey>, TTuple>(selectRequest);
        }

        ///Note: there is no such method in specification http://tarantool.org/doc/book/box/box_index.html.
        ///But common sense, and sources https://github.com/tarantool/tarantool/blob/1.7/src/box/lua/index.c says that that method sould be
        public async Task<DataResponse<TTuple[]>> Insert<TTuple>(TTuple tuple)
            where TTuple : ITuple
        {
            var insertRequest = new InsertRequest<TTuple>(SpaceId, tuple);

            return await LogicalConnection.SendRequest<InsertReplaceRequest<TTuple>, TTuple>(insertRequest);
        }

        ///Note: there is no such method in specification http://tarantool.org/doc/book/box/box_index.html.
        ///But common sense, and sources https://github.com/tarantool/tarantool/blob/1.7/src/box/lua/index.c says that that method sould be
        public async Task<DataResponse<TTuple[]>> Replace<TTuple>(TTuple tuple)
            where TTuple : ITuple
        {
            var replaceRequest = new ReplaceRequest<TTuple>(SpaceId, tuple);

            return await LogicalConnection.SendRequest<InsertReplaceRequest<TTuple>, TTuple>(replaceRequest);
        }

        public async Task<TTuple> Min<TTuple>()
           where TTuple : ITuple
        {
            return await Min<TTuple, Tuple>(Tuple.Create());
        }

        public async Task<TTuple> Min<TTuple, TKey>(TKey key)
            where TTuple : ITuple
            where TKey : class, ITuple
        {
            if (Type != IndexType.Tree)
            {
                throw ExceptionHelper.WrongIndexType("TREE", "min");
            }
            var iterator = key == null ? Iterator.Eq : Iterator.Ge;

            var selectPacket = new SelectRequest<TKey>(SpaceId, Id, 1, 0, iterator, key);

            var minResponse = await LogicalConnection.SendRequest<SelectRequest<TKey>, TTuple>(selectPacket);
            return minResponse.Data.SingleOrDefault();
        }

        public async Task<TTuple> Max<TTuple>()
            where TTuple : ITuple
        {
            return await Max<TTuple, Tuple>(Tuple.Create());
        }

        public async Task<TTuple> Max<TTuple, TKey>(TKey key = null)
            where TTuple : ITuple
            where TKey : class, ITuple
        {
            if (Type != IndexType.Tree)
            {
                throw ExceptionHelper.WrongIndexType("TREE", "max");
            }
            var iterator = key == null ? Iterator.Req : Iterator.Le;

            var selectPacket = new SelectRequest<TKey>(SpaceId, Id, 1, 0, iterator, key);

            var maxResponse = await LogicalConnection.SendRequest<SelectRequest<TKey>, TTuple>(selectPacket);
            return maxResponse.Data.SingleOrDefault();
        }

        public TTuple Random<TTuple>(int randomValue)
            where TTuple : ITuple
        {
            throw new NotImplementedException();
        }

        public uint Count<TKey>(TKey key = null, Iterator it = Iterator.Eq)
            where TKey : class, ITuple
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<TTuple[]>> Update<TTuple, TKey>(TKey key, UpdateOperation[] updateOperations)
            where TKey : ITuple
            where TTuple : ITuple
        {
            var updateRequest = new UpdateRequest<TKey>(
                SpaceId,
                Id,
                key,
                updateOperations);

            return await LogicalConnection.SendRequest<UpdateRequest<TKey>, TTuple>(updateRequest);
        }

        public async Task Upsert<TKey>(TKey key, UpdateOperation[] updateOperations)
            where TKey : ITuple
        {
            var updateRequest = new UpsertRequest<TKey>(
                SpaceId,
                key,
                updateOperations);

            await LogicalConnection.SendRequestWithEmptyResponse(updateRequest);
        }

        public async Task<DataResponse<TTuple[]>> Delete<TKey, TTuple>(TKey key)
            where TKey : ITuple
            where TTuple : ITuple
        {
            var deleteRequest = new DeleteRequest<TKey>(SpaceId, Id, key);

            return await LogicalConnection.SendRequest<DeleteRequest<TKey>, TTuple>(deleteRequest);
        }

        public void Alter(IndexCreationOptions options)
        {
            throw new NotImplementedException();
        }

        public void Drop()
        {
            throw new NotImplementedException();
        }

        public void Rename(string indexName)
        {
            throw new NotImplementedException();
        }

        public uint BSize()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Name}, id={Id}, spaceId={SpaceId}";
        }
    }
}