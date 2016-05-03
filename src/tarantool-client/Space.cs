using System;
using System.Collections.Generic;
using System.Linq;
using iproto;
using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

namespace tarantool_client
{
    public class Space
    {
        public Space(uint id, uint fieldCount, string name, IReadOnlyCollection<Index> indices, StorageEngine engine, IReadOnlyCollection<SpaceField> fields, Connection connection)
        {
            Id = id;
            FieldCount = fieldCount;
            Name = name;
            Indices = indices;
            Engine = engine;
            Fields = fields;
            Connection = connection;
        }

        public Connection Connection { get; set; }

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

        public ResponsePacket<T[]> Insert<T>(T tuple)
            where T : ITuple
        {
            var insertRequest = new InsertReplacePacket<T>(CommandCode.Insert, Id, tuple);
            return Connection.SendPacket<InsertReplacePacket<T>, T[]>(insertRequest);
        }

        public ResponsePacket<object> Select<T>(T selectKey)
            where T : ITuple
        {
            var selectRequest = new SelectPacket<T>(Id, PrimaryIndex.Id, uint.MaxValue, 0, Iterator.Eq, selectKey);
            return Connection.SendPacket(selectRequest);
        }

        public ResponsePacket<TResult[]> Select<TKey, TResult>(TKey selectKey)
          where TKey : ITuple
          where TResult : ITuple
        {
            var selectRequest = new SelectPacket<TKey>(Id, PrimaryIndex.Id, uint.MaxValue, 0, Iterator.Eq, selectKey);
            return Connection.SendPacket<SelectPacket<TKey>, TResult[]>(selectRequest);
        }

        public TResult Get<TKey, TResult>(TKey key)
            where TKey : ITuple
          where TResult : ITuple
        {
            var selectRequest = new SelectPacket<TKey>(Id, PrimaryIndex.Id, 1, 0, Iterator.Eq, key);
            return Connection.SendPacket<SelectPacket<TKey>, TResult[]>(selectRequest).Data.Single();
        }

        public ResponsePacket<T[]> Replace<T>(T tuple)
            where T : ITuple
        {
            var insertRequest = new InsertReplacePacket<T>(CommandCode.Replace, Id, tuple);
            return Connection.SendPacket<InsertReplacePacket<T>, T[]>(insertRequest);
        }

        public T Put<T>(T tuple)
            where T : ITuple
        {
            return Replace(tuple).Data.First();
        }

        public ResponsePacket<TResult[]> Update<TKey, TUpdate, TResult>(TKey key, UpdateOperation<TUpdate> updateOperation)
            where TKey : ITuple
            where TResult : ITuple
        {
            var updateRequest = new UpdatePacket<TKey, TUpdate>(Id, PrimaryIndex.Id, key, updateOperation);
            return Connection.SendPacket<UpdatePacket<TKey, TUpdate>, TResult[]>(updateRequest);
        }

        public ResponsePacket<object> Update<TKey, TUpdate>(TKey key, UpdateOperation<TUpdate> updateOperation)
           where TKey : ITuple
        {
            var updateRequest = new UpdatePacket<TKey, TUpdate>(Id, PrimaryIndex.Id, key, updateOperation);
            return Connection.SendPacket(updateRequest);
        }

        public ResponsePacket<object> Upsert<TTuple, TUpdate>(TTuple tuple, UpdateOperation<TUpdate> updateOperation)
           where TTuple : ITuple
        {
            var updateRequest = new UpsertPacket<TTuple, TUpdate>(Id, tuple, updateOperation);
            return Connection.SendPacket(updateRequest);
        }
        public ResponsePacket<TResult[]> Upsert<TTuple, TUpdate, TResult>(TTuple tuple, UpdateOperation<TUpdate> updateOperation)
           where TTuple : ITuple
        {
            var updateRequest = new UpsertPacket<TTuple, TUpdate>(Id, tuple, updateOperation);
            return Connection.SendPacket<UpsertPacket<TTuple, TUpdate>, TResult[]>(updateRequest);
        }

        public ResponsePacket<TTuple[]> Delete<TTuple, TKey>(TKey key)
           where TTuple : ITuple
           where TKey : ITuple
        {
            var deleteResponse = new DeletePacket<TKey>(Id, PrimaryIndex.Id, key);
            return Connection.SendPacket<DeletePacket<TKey>, TTuple[]>(deleteResponse);
        }

        public ResponsePacket<object> Delete<TKey>(TKey key)
          where TKey : ITuple
        {
            var deleteResponse = new DeletePacket<TKey>(Id, PrimaryIndex.Id, key);
            return Connection.SendPacket(deleteResponse);
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

        public int Increment<TKey>(TKey key)
            where TKey : ITuple
        {
            throw new NotImplementedException();
        }

        public int Decrement<TKey>(TKey key)
            where TKey : ITuple
        {
            throw new NotImplementedException();
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