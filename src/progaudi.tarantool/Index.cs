using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    public class Index : IIndex
    {
        private readonly MsgPackContext _context;
        public ILogicalConnection LogicalConnection { get; set; }

        public Index(uint id, uint spaceId, string name, bool unique, IndexType type, IReadOnlyList<IndexPart> parts, MsgPackContext context)
        {
            Id = id;
            SpaceId = spaceId;
            Name = name;
            Unique = unique;
            Type = type;
            Parts = parts;
            _context = context;
        }

        public uint Id { get; }

        public uint SpaceId { get; }

        public string Name { get; }

        public bool Unique { get; }

        public IndexType Type { get; }

        public IReadOnlyList<IndexPart> Parts { get; }

        public Task<IEnumerable<TResult>> Pairs<TValue, TResult>(TValue value, Iterator iterator)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<TTuple[]>> Select<TKey, TTuple>(TKey key, SelectOptions options = null)
        {
            var selectRequest = new SelectRequest<TKey>(
                SpaceId,
                Id,
                options?.Limit ?? uint.MaxValue,
                options?.Offset ?? 0,
                options?.Iterator ?? Iterator.Eq,
                key,
                _context);

            return await LogicalConnection.SendRequest<SelectRequest<TKey>, TTuple>(selectRequest).ConfigureAwait(false);
        }

        ///Note: there is no such method in specification http://tarantool.org/doc/book/box/box_index.html.
        ///But common sense, and sources https://github.com/tarantool/tarantool/blob/1.7/src/box/lua/index.c says that that method sould be
        public async Task<DataResponse<TTuple[]>> Insert<TTuple>(TTuple tuple)
        {
            var insertRequest = new InsertRequest<TTuple>(SpaceId, tuple, _context);

            return await LogicalConnection.SendRequest<InsertReplaceRequest<TTuple>, TTuple>(insertRequest).ConfigureAwait(false);
        }

        ///Note: there is no such method in specification http://tarantool.org/doc/book/box/box_index.html.
        ///But common sense, and sources https://github.com/tarantool/tarantool/blob/1.7/src/box/lua/index.c says that that method sould be
        public async Task<DataResponse<TTuple[]>> Replace<TTuple>(TTuple tuple)
        {
            var replaceRequest = new ReplaceRequest<TTuple>(SpaceId, tuple, _context);

            return await LogicalConnection.SendRequest<InsertReplaceRequest<TTuple>, TTuple>(replaceRequest).ConfigureAwait(false);
        }

        public async Task<TTuple> Min<TTuple>()
        {
            return await Min<TTuple, int[]>(Array.Empty<int>()).ConfigureAwait(false);
        }

        public async Task<TTuple> Min<TTuple, TKey>(TKey key)
        {
            if (Type != IndexType.Tree)
            {
                throw ExceptionHelper.WrongIndexType("TREE", "min");
            }
            var iterator = key == null ? Iterator.Eq : Iterator.Ge;

            var selectPacket = new SelectRequest<TKey>(SpaceId, Id, 1, 0, iterator, key, _context);

            var minResponse = await LogicalConnection.SendRequest<SelectRequest<TKey>, TTuple>(selectPacket).ConfigureAwait(false);
            return minResponse.Data.SingleOrDefault();
        }

        public async Task<TTuple> Max<TTuple>()
        {
            return await Max<TTuple, int[]>(Array.Empty<int>()).ConfigureAwait(false);
        }

        public async Task<TTuple> Max<TTuple, TKey>(TKey key)
        {
            if (Type != IndexType.Tree)
            {
                throw ExceptionHelper.WrongIndexType("TREE", "max");
            }
            var iterator = key == null ? Iterator.Req : Iterator.Le;

            var selectPacket = new SelectRequest<TKey>(SpaceId, Id, 1, 0, iterator, key, _context);

            var maxResponse = await LogicalConnection.SendRequest<SelectRequest<TKey>, TTuple>(selectPacket).ConfigureAwait(false);
            return maxResponse.Data.SingleOrDefault();
        }

        public TTuple Random<TTuple>(int randomValue)
        {
            throw new NotImplementedException();
        }

        public uint Count<TKey>(TKey key, Iterator it = Iterator.Eq)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse<TTuple[]>> Update<TTuple, TKey>(TKey key, UpdateOperation[] updateOperations)
        {
            var updateRequest = new UpdateRequest<TKey>(
                SpaceId,
                Id,
                key,
                _context,
                updateOperations);

            return await LogicalConnection.SendRequest<UpdateRequest<TKey>, TTuple>(updateRequest).ConfigureAwait(false);
        }

        public async Task Upsert<TKey>(TKey key, UpdateOperation[] updateOperations)
        {
            var updateRequest = new UpsertRequest<TKey>(
                SpaceId,
                key,
                _context,
                updateOperations);

            await LogicalConnection.SendRequestWithEmptyResponse(updateRequest).ConfigureAwait(false);
        }

        public async Task<DataResponse<TTuple[]>> Delete<TKey, TTuple>(TKey key)
        {
            var deleteRequest = new DeleteRequest<TKey>(SpaceId, Id, key, _context);

            return await LogicalConnection.SendRequest<DeleteRequest<TKey>, TTuple>(deleteRequest).ConfigureAwait(false);
        }

        public Task Alter(IndexCreationOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Drop()
        {
            throw new NotImplementedException();
        }

        public Task Rename(string indexName)
        {
            throw new NotImplementedException();
        }

        public Task<uint> BSize()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Name}, id={Id}, spaceId={SpaceId}";
        }
    }
}