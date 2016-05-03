using System;
using System.Collections.Generic;

using iproto;
using iproto.Data;
using iproto.Data.Packets;

namespace tarantool_client
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

        public Connection Connection { get; set; }

        public IEnumerable<TResult> Pairs<TValue, TResult>(TValue value, Iterator iterator)
            where TResult : ITuple
        {
            throw new NotImplementedException();
        }

        public ResponsePacket<TTuple[]> Select<TTuple, TKey>(TKey key, SelectOptions options = null)
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

            return Connection.SendPacket<SelectPacket<TKey>, TTuple[]>(selectRequest);
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