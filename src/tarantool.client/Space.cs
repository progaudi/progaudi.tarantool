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
        public Space(uint id, uint fieldCount, string name, IReadOnlyCollection<Index> indices, StorageEngine engine, IReadOnlyCollection<SpaceField> fields, Multiplexer multiplexer)
        {
            Id = id;
            FieldCount = fieldCount;
            Name = name;
            Indices = indices;
            Engine = engine;
            Fields = fields;
            Multiplexer = multiplexer;
        }

        public Multiplexer Multiplexer { get; set; }

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

        public async Task<ResponsePacket<T[]>> Insert<T>(T tuple)
            where T : ITuple
        {
            var insertRequest = new InsertReplacePacket<T>(CommandCode.Insert, Id, tuple);
            return await Multiplexer.SendPacket<InsertReplacePacket<T>, T[]>(insertRequest);
        }

        public async Task<ResponsePacket<TResult[]>> Select<TKey, TResult>(TKey selectKey)
          where TKey : ITuple
          where TResult : ITuple
        {
            var selectRequest = new SelectPacket<TKey>(Id, PrimaryIndex.Id, uint.MaxValue, 0, Iterator.Eq, selectKey);
            return await Multiplexer.SendPacket<SelectPacket<TKey>, TResult[]>(selectRequest);
        }

        public async Task<TResult> Get<TKey, TResult>(TKey key)
            where TKey : ITuple
          where TResult : ITuple
        {
            var selectRequest = new SelectPacket<TKey>(Id, PrimaryIndex.Id, 1, 0, Iterator.Eq, key);
            var response = await Multiplexer.SendPacket<SelectPacket<TKey>, TResult[]>(selectRequest);
            return response.Data.Single();
        }

        public async Task<ResponsePacket<T[]>> Replace<T>(T tuple)
            where T : ITuple
        {
            var insertRequest = new InsertReplacePacket<T>(CommandCode.Replace, Id, tuple);
            return await Multiplexer.SendPacket<InsertReplacePacket<T>, T[]>(insertRequest);
        }

        public async Task<T> Put<T>(T tuple)
            where T : ITuple
        {
            var response = await Replace(tuple);
            return response.Data.First();
        }

        public async Task<ResponsePacket<TResult[]>> Update<TKey, TUpdate, TResult>(TKey key, UpdateOperation<TUpdate> updateOperation)
            where TKey : ITuple
            where TResult : ITuple
        {
            var updateRequest = new UpdatePacket<TKey, TUpdate>(Id, PrimaryIndex.Id, key, updateOperation);
            return await Multiplexer.SendPacket<UpdatePacket<TKey, TUpdate>, TResult[]>(updateRequest);
        }
        
        public async Task<ResponsePacket<TTuple[]>> Delete<TTuple, TKey>(TKey key)
           where TTuple : ITuple
           where TKey : ITuple
        {
            var deleteRequest = new DeletePacket<TKey>(Id, PrimaryIndex.Id, key);
            return await Multiplexer.SendPacket<DeletePacket<TKey>, TTuple[]>(deleteRequest);
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

            return await Multiplexer.SendPacket<UpsertPacket<TKey, int>, TTuple[]>(upsertRequest);
        }

        public async Task<ResponsePacket<TTuple[]>> Decrement<TTuple, TKey>(TKey key)
            where TKey : ITuple
        {
            var lastFieldKeyNumber = PrimaryIndex.Parts.Max(part => part.FieldNo);
            var upsertRequest = new UpsertPacket<TKey, int>(Id, key,
                UpdateOperation<int>.CreateAddition(-1, (int)lastFieldKeyNumber + 1));

            return await Multiplexer.SendPacket<UpsertPacket<TKey, int>, TTuple[]>(upsertRequest);
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

        private Index PrimaryIndex => Indices.First();
    }
}