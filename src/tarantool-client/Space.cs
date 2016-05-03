using System;
using System.Collections.Generic;

using iproto;
using iproto.Data.UpdateOperations;

namespace tarantool_client
{
    public class Space
    {
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

        public T Insert<T>(T tuple)
            where T : ITuple
        {
            throw new NotImplementedException();
        }

        public IList<object> Select<T>(T selectKey)
            where T : ITuple
        {
            throw new NotImplementedException();
        }

        public IList<TResult> Select<TKey, TResult>(TKey selectKey)
          where TKey : ITuple
          where TResult : ITuple
        {
            throw new NotImplementedException();
        }

        public TResult Get<TKey, TResult>(TKey key)
            where TKey : ITuple
          where TResult : ITuple
        {
            throw new NotImplementedException();
        }

        public T Replace<T>(T tuple)
            where T : ITuple
        {
            throw new NotImplementedException();
        }

        public T Put<T>(T tuple)
            where T: ITuple
        {
            return Replace(tuple);
        }

        public TTuple Update<TTuple, TUpdate>(TTuple tuple, UpdateOperation<TUpdate> updateOperation)
            where TTuple : ITuple
        {
            throw new NotImplementedException();
        }

        public TTuple Upsert<TTuple, TUpdate>(TTuple tuple, UpdateOperation<TUpdate> updateOperation)
           where TTuple : ITuple
        {
            throw new NotImplementedException();
        }

        public TTuple Delete<TTuple, TKey>(TKey key)
           where TTuple : ITuple
           where TKey: ITuple
        {
            throw new NotImplementedException();
        }

        public uint Count<TKey>(TKey key)
           where TKey: ITuple
        {
            throw new NotImplementedException();
        }

        public uint Length()
        {
            throw new NotImplementedException();
        }

        public int Increment<TKey>(TKey key)
            where TKey:ITuple
        {
            throw new NotImplementedException();
        }

        public int Decrement<TKey>(TKey key)
            where TKey:ITuple
        {
            throw new NotImplementedException();
        }

        public TTuple AutoIncrement<TTuple, TRest>(TRest tupleRest)
            where TTuple: ITuple
            where TRest : ITuple
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> Pairs<TKey, TValue>()
        {
            throw new NotImplementedException();
        }
    }
}