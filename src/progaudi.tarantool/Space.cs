using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

namespace ProGaudi.Tarantool.Client
{
    public class Space<T> : ISpace<T>
    {
        private readonly Schema _schema;
        private readonly SpaceMeta _meta;
        private readonly Lazy<NameIdLazyWrapper<IndexMeta>> _indices;

        public Space(Schema schema, SpaceMeta meta, IEnumerable<IndexMeta> indexMetas)
        {
            _schema = schema;
            _meta = meta;
            _indices = new Lazy<NameIdLazyWrapper<IndexMeta>>(() => new NameIdLazyWrapper<IndexMeta>(indexMetas.ToArray(), x => x.Id, x => x.Name));
        }

        public override string ToString()
        {
            return $"{Name}, id={Id}, Engine={Engine}";
        }

        public uint Id => _meta.Id;

        public int OwnerId => _meta.OwnerId;

        public string Name => _meta.Name;

        public StorageEngine Engine => _meta.Engine;

        public uint FieldCount => _meta.FieldCount;

        public SpaceOptions Options => _meta.Options;

        public SpaceField[] Fields => _meta.Fields;

        public IIndex<T> this[string name] => TryGetIndex(name, out var x) ? x : throw new IndexOutOfRangeException();

        public IIndex<T> this[uint id] => TryGetIndex(id, out var x) ? x : throw new IndexOutOfRangeException();

        public bool TryGetIndex(string name, out IIndex<T> index) => TryGetIndex<T>(name, out index);

        public bool TryGetIndex(uint id, out IIndex<T> index) => TryGetIndex<T>(id, out index);

        public bool TryGetIndex<TCastedValue>(string name, out IIndex<TCastedValue> index)
        {
            var meta = _indices.Value[name];

            if (meta == default)
            {
                index = default;
                return false;
            }

            index = new Index<TCastedValue>(meta, _schema);
            return true;
        }

        public bool TryGetIndex<TCastedValue>(uint id, out IIndex<TCastedValue> index)
        {
            var meta = _indices.Value[id];

            if (meta == default)
            {
                index = default;
                return false;
            }

            index = new Index<TCastedValue>(meta, _schema);
            return true;
        }

        public Task<T> Insert(ref T tuple)
        {
            throw new NotImplementedException();
        }

        public Task<T> Insert<TInsertable>(ref TInsertable tuple)
        {
            throw new NotImplementedException();
        }

        public Task<T[]> Select<TKey>(TKey selectKey, Iterator iterator = Iterator.Eq, uint limit = UInt32.MaxValue, uint offset = 0)
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

        public Task<int> Count<TKey>(TKey key, Iterator iterator)
        {
            throw new NotImplementedException();
        }

        public Task<int> Count()
        {
            throw new NotImplementedException();
        }

        public Task<int> Length()
        {
            throw new NotImplementedException();
        }

        public Task<int> ByteSize()
        {
            throw new NotImplementedException();
        }
    }
}