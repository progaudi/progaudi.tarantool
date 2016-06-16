using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Tarantool.Client.IProto;
using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;
using Tarantool.Client.IProto.Data.UpdateOperations;

using Tuple = Tarantool.Client.IProto.Tuple;

namespace Tarantool.Client
{
    public class Index
    {
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

        public Box Box { get; set; }

        public IEnumerable<TResult> Pairs<TValue, TResult>(TValue value, Iterator iterator)
            where TResult : ITuple
        {
            throw new NotImplementedException();
        }

        public async Task<ResponsePacket<TTuple[]>> Select<TTuple, TKey>(TKey key, SelectOptions options = null)
            where TKey : ITuple
            where TTuple : ITuple
        {
            var selectRequest = new SelectPacket<TKey>(
                SpaceId,
                Id,
                options?.Limit ?? uint.MaxValue,
                options?.Offset ?? 0,
                options?.Iterator ?? Iterator.Eq,
                key);

            return await Box.SendPacket<SelectPacket<TKey>, TTuple[]>(selectRequest);
        }

        ///Note: there is no such method in specification http://tarantool.org/doc/book/box/box_index.html.
        ///But common sense, and sources https://github.com/tarantool/tarantool/blob/1.7/src/box/lua/index.c says that that method sould be
        public async Task<ResponsePacket<TTuple[]>> Insert<TTuple>(TTuple tuple)
            where TTuple : ITuple
        {
            var insertRequest = new InsertPacket<TTuple>(SpaceId, tuple);

            return await Box.SendPacket<InsertReplacePacket<TTuple>, TTuple[]>(insertRequest);
        }

        ///Note: there is no such method in specification http://tarantool.org/doc/book/box/box_index.html.
        ///But common sense, and sources https://github.com/tarantool/tarantool/blob/1.7/src/box/lua/index.c says that that method sould be
        public async Task<ResponsePacket<TTuple[]>> Replace<TTuple>(TTuple tuple)
            where TTuple : ITuple
        {
            var replaceRequest = new ReplacePacket<TTuple>(SpaceId, tuple);

            return await Box.SendPacket<InsertReplacePacket<TTuple>, TTuple[]>(replaceRequest);
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
                throw new NotSupportedException("Only TREE indicies support min opration.");
            }
            var iterator = key == null ? Iterator.Eq : Iterator.Ge;

            var selectPacket = new SelectPacket<TKey>(SpaceId, Id, 1, 0, iterator, key);

            var minResponse = await Box.SendPacket<SelectPacket<TKey>, TTuple[]>(selectPacket);
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
                throw new NotSupportedException("Only TREE indicies support max opration.");
            }
            var iterator = key == null ? Iterator.Req : Iterator.Le;

            var selectPacket = new SelectPacket<TKey>(SpaceId, Id, 1, 0, iterator, key);

            var maxResponse = await Box.SendPacket<SelectPacket<TKey>, TTuple[]>(selectPacket);
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

        public async Task<ResponsePacket<TTuple[]>> Update<TTuple, TKey, TUpdate>(TKey key, UpdateOperation<TUpdate> updateOperation)
            where TKey : ITuple
        {
            var updateRequest = new UpdatePacket<TKey, TUpdate>(
                SpaceId,
                Id,
                key,
                updateOperation);

            return await Box.SendPacket<UpdatePacket<TKey, TUpdate>, TTuple[]>(updateRequest);
        }

        public async Task<ResponsePacket<TTuple[]>> Delete<TTuple, TKey>(TKey key)
            where TKey : ITuple
        {
            var deleteRequest = new DeletePacket<TKey>(SpaceId, Id, key);

            return await Box.SendPacket<DeletePacket<TKey>, TTuple[]>(deleteRequest);
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
    }
}