using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Tarantool.Client.IProto;
using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;
using Tarantool.Client.IProto.Data.UpdateOperations;

namespace Tarantool.Client
{
    public class Space
    {
        private const int VIndex = 0x121;

        private const int IndexById = 0;

        private const int IndexByName = 2;

        public ILogicalConnection Connection { get; set; }

        private Index PrimaryIndex => Indices.First();

        public Space(uint id, uint fieldCount, string name, IReadOnlyCollection<Index> indices, StorageEngine engine, IReadOnlyCollection<SpaceField> fields)
        {
            Id = id;
            FieldCount = fieldCount;
            Name = name;
            Indices = indices;
            Engine = engine;
            Fields = fields;
        }

        public uint Id { get; }

        public uint FieldCount { get; }

        public string Name { get; }

        public StorageEngine Engine { get; }

        public IReadOnlyCollection<Index> Indices { get; }

        public IReadOnlyCollection<SpaceField> Fields { get; }

        public void CreateIndex()
        {
            throw new NotImplementedException();
        }

        public void Drop()
        {
            throw new NotImplementedException();
        }

        public void Rename(string newName)
        {
            throw new NotImplementedException();
        }


        public async Task<Index> GetIndexAsync(string indexName)
        {
            var selectIndexRequest = new SelectPacket<IProto.Tuple<uint, string>>(VIndex, IndexByName, uint.MaxValue, 0, Iterator.Eq, IProto.Tuple.Create(Id, indexName));

            var response = await Connection.SendRequest<SelectPacket<IProto.Tuple<uint, string>>, ResponsePacket<Index[]>>(selectIndexRequest);

            if (!response.Data.Any())
                throw new ArgumentException($"Index with name '{indexName}' was not found in space {ToString()}!");

            return response.Data.Single();
        }

        public async Task<Index> GetIndexAsync(uint indexId)
        {
            var selectIndexRequest = new SelectPacket<IProto.Tuple<uint, uint>>(VIndex, IndexById, uint.MaxValue, 0, Iterator.Eq, IProto.Tuple.Create(Id, indexId));

            var response = await Connection.SendRequest<SelectPacket<IProto.Tuple<uint, uint>>, ResponsePacket<Index[]>>(selectIndexRequest);

            if (!response.Data.Any())
                throw new ArgumentException($"Index with id '{indexId}' was found in space {ToString()}!");

            return response.Data.Single();
        }

        public async Task<ResponsePacket<TTuple[]>> Insert<TTuple>(TTuple tuple)
            where TTuple : ITuple
        {
            var insertRequest = new InsertPacket<TTuple>(Id, tuple);
            return await Connection.SendRequest<InsertReplacePacket<TTuple>, ResponsePacket<TTuple[]>>(insertRequest);
        }

        public async Task<ResponsePacket<TTuple[]>> Select<TKey, TTuple>(TKey selectKey)
          where TKey : ITuple
          where TTuple : ITuple
        {
            var selectRequest = new SelectPacket<TKey>(Id, PrimaryIndex.Id, uint.MaxValue, 0, Iterator.Eq, selectKey);
            return await Connection.SendRequest<SelectPacket<TKey>, ResponsePacket<TTuple[]>>(selectRequest);
        }

        public async Task<TTuple> Get<TKey, TTuple>(TKey key)
            where TKey : ITuple
          where TTuple : ITuple
        {
            var selectRequest = new SelectPacket<TKey>(Id, PrimaryIndex.Id, 1, 0, Iterator.Eq, key);
            var response = await Connection.SendRequest<SelectPacket<TKey>, ResponsePacket<TTuple[]>>(selectRequest);
            return response.Data.Single();
        }

        public async Task<ResponsePacket<TTuple[]>> Replace<TTuple>(TTuple tuple)
            where TTuple : ITuple
        {
            var insertRequest = new ReplacePacket<TTuple>(Id, tuple);
            return await Connection.SendRequest<InsertReplacePacket<TTuple>, ResponsePacket<TTuple[]>>(insertRequest);
        }

        public async Task<T> Put<T>(T tuple)
            where T : ITuple
        {
            var response = await Replace(tuple);
            return response.Data.First();
        }

        public async Task<ResponsePacket<TTuple[]>> Update<TKey, TUpdate, TTuple>(TKey key, UpdateOperation<TUpdate> updateOperation)
            where TKey : ITuple
            where TTuple : ITuple
        {
            var updateRequest = new UpdatePacket<TKey, TUpdate>(Id, PrimaryIndex.Id, key, updateOperation);
            return await Connection.SendRequest<UpdatePacket<TKey, TUpdate>, ResponsePacket<TTuple[]>>(updateRequest);
        }

        public async Task<ResponsePacket<TTuple[]>> Delete<TTuple, TKey>(TKey key)
           where TTuple : ITuple
           where TKey : ITuple
        {
            var deleteRequest = new DeletePacket<TKey>(Id, PrimaryIndex.Id, key);
            return await Connection.SendRequest<DeletePacket<TKey>, ResponsePacket<TTuple[]>>(deleteRequest);
        }

        public uint Count<TKey>(TKey key)
           where TKey : ITuple
        {
            throw new NotImplementedException();
        }

        public uint Length()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponsePacket<TTuple[]>> Increment<TTuple, TKey>(TKey key)
            where TKey : ITuple
        {
            var lastFieldKeyNumber = PrimaryIndex.Parts.Max(part => part.FieldNo);
            var upsertRequest = new UpsertPacket<TKey, int>(Id, key,
                UpdateOperation<int>.CreateAddition(1, (int)lastFieldKeyNumber + 1));

            return await Connection.SendRequest<UpsertPacket<TKey, int>, ResponsePacket<TTuple[]>>(upsertRequest);
        }

        public async Task<ResponsePacket<TTuple[]>> Decrement<TTuple, TKey>(TKey key)
            where TKey : ITuple
        {
            var lastFieldKeyNumber = PrimaryIndex.Parts.Max(part => part.FieldNo);
            var upsertRequest = new UpsertPacket<TKey, int>(Id, key,
                UpdateOperation<int>.CreateAddition(-1, (int)lastFieldKeyNumber + 1));

            return await Connection.SendRequest<UpsertPacket<TKey, int>, ResponsePacket<TTuple[]>>(upsertRequest);
        }

        public TTuple AutoIncrement<TTuple, TRest>(TRest tupleRest)
            where TTuple : ITuple
            where TRest : ITuple
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> Pairs<TKey, TValue>()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Name}, id={Id}";
        }
    }
}