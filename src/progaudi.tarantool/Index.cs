using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

namespace ProGaudi.Tarantool.Client
{
    public class Index<T> : IIndex<T>
    {
        private readonly IndexMeta _meta;
        private readonly Schema _schema;

        public Index(IndexMeta meta, Schema schema)
        {
            _meta = meta;
            _schema = schema;
        }

        public uint Id => _meta.Id;

        public uint SpaceId => _meta.SpaceId;

        public string Name => _meta.Name;

        public bool Unique => _meta.Options.Unique;

        public IndexType Type => _meta.Type;

        public IReadOnlyList<IndexPart> Parts => _meta.Parts;

        public Task<T[]> Select<TKey>(TKey key, Iterator iterator = Iterator.Eq, uint limit = uint.MaxValue, uint offset = 0)
        {
            throw new NotImplementedException();
        }

        public Task<T> Insert(ref T tuple)
        {
            throw new NotImplementedException();
        }

        public Task<T> Insert<TInsertable>(ref TInsertable tuple)
        {
            throw new NotImplementedException();
        }

        public Task<T> Get<TKey>(TKey key, GetOptions options = GetOptions.Eval)
        {
            throw new NotImplementedException();
        }

        public Task<T> Replace(T tuple)
        {
            throw new NotImplementedException();
        }

        public Task<T> Put(T tuple)
        {
            throw new NotImplementedException();
        }

        public Task<T> Update<TKey>(TKey key, UpdateOperation[] updateOperations)
        {
            throw new NotImplementedException();
        }

        public Task Upsert(T tuple, UpdateOperation[] updateOperations)
        {
            throw new NotImplementedException();
        }

        public Task Upsert<TInsertable>(TInsertable tuple, UpdateOperation[] updateOperations)
        {
            throw new NotImplementedException();
        }

        public Task<T> Delete<TKey>(TKey key)
        {
            throw new NotImplementedException();
        }

        public Task<T> Min()
        {
            throw new NotImplementedException();
        }

        public Task<T> Min<TKey>(TKey key)
        {
            throw new NotImplementedException();
        }

        public Task<T> Max()
        {
            throw new NotImplementedException();
        }

        public Task<T> Max<TKey>(TKey key)
        {
            throw new NotImplementedException();
        }

        public Task<T> Random(uint seed)
        {
            throw new NotImplementedException();
        }

        public Task<int> Count<TKey>(TKey key, Iterator iterator)
        {
            throw new NotImplementedException();
        }

        public Task<int> Count()
        {
            throw new NotImplementedException();
        }

        public Task<int> ByteSize()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Name}, id={Id}, spaceId={SpaceId}";
        }
    }
}