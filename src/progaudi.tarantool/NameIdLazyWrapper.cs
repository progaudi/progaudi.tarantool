using System;
using System.Collections.Generic;

namespace ProGaudi.Tarantool.Client
{
    public sealed class NameIdLazyWrapper<T>
    {
        private static T _nil;
        private readonly T[] _entities;
        private readonly Func<T, string> _nameGetter;
        private readonly Func<T, uint> _idGetter;
        private Dictionary<uint, int> _idCache;
        private Dictionary<string, int> _nameCache;

        public NameIdLazyWrapper(T[] entities, Func<T, uint> idGetter, Func<T, string> nameGetter)
        {
            _entities = entities;
            _nameGetter = nameGetter;
            _idGetter = idGetter;
        }

        public ref T this[string name]
        {
            get
            {
                Init();

                if (!_nameCache.TryGetValue(name, out var index))
                {
                    return ref _nil;
                }

                return ref _entities[index];
            }
        }

        public ref T this[uint id]
        {
            get
            {
                Init();

                if (!_idCache.TryGetValue(id, out var index))
                {
                    return ref _nil;
                }

                return ref _entities[index];
            }
        }

        private void Init()
        {
            if (_nameCache != null)
            {
                return;
            }

            var nameCache = new Dictionary<string, int>();
            var idCache = new Dictionary<uint, int>();
            for (var i = 0; i < _entities.Length; i++)
            {
                var entity = _entities[i];
                nameCache[_nameGetter(entity)] = i;
                idCache[_idGetter(entity)] = i;
            }

            _idCache = idCache;
            _nameCache = nameCache;
        }
    }
}