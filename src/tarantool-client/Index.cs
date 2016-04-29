using System;
using System.Collections.Generic;

using iproto;
using iproto.Data;

namespace tarantool_client
{
    public class Index
    {
        public bool Unique { get; }

        public IndexType Type { get; }

        public IReadOnlyList<IndexPart> Parts { get; }

        public IEnumerable<TResult> Pairs<TValue, TResult>(TValue value, Iterator iterator)
            where TResult : ITuple
        {
            throw new NotImplementedException();
        }

        public TTuple Select<TTuple, TKey>(TKey key, SelectOptions options = null)
            where TKey : ITuple
            where TTuple : ITuple

        {
            throw new NotImplementedException();
        }

        public TTuple Min<TTuple, TKey>(TKey key)
            where TTuple : ITuple
            where TKey : ITuple
        {
            throw new NotImplementedException();
        }

        public TTuple Max<TTuple, TKey>(TKey key)
            where TTuple : ITuple
            where TKey : ITuple
        {
            throw new NotImplementedException();
        }

        public TTuple Random<TTuple>(int randomValue)
            where TTuple : ITuple
        {
            throw new NotImplementedException();
        }

        public uint Count<TKey>(TKey key = default(TKey), Iterator it = Iterator.Eq)
            where TKey : ITuple
        {
            throw new NotImplementedException();
        }
    }
}