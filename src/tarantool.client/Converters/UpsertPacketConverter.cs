using System;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Requests;

namespace Tarantool.Client.Converters
{
    internal class UpsertPacketConverter<T> : IMsgPackConverter<UpsertRequest<T>>
        where T : ITuple
    {
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<T> _tupleConverter;
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _uintConverter = context.GetConverter<uint>();
            _keyConverter = context.GetConverter<Key>();
            _tupleConverter = context.GetConverter<T>();
            _context = context;
        }

        public void Write(UpsertRequest<T> value, IMsgPackWriter writer)
        {
            writer.WriteMapHeader(3);

            _keyConverter.Write(Key.SpaceId, writer);
            _uintConverter.Write(value.SpaceId, writer);

            _keyConverter.Write(Key.Tuple, writer);
            _tupleConverter.Write(value.Tuple, writer);

            _keyConverter.Write(Key.Ops, writer);
            writer.WriteArrayHeader((uint) value.UpdateOperations.Length);

            foreach (var updateOperation in value.UpdateOperations)
            {
                var operationConverter = updateOperation.GetConverter(_context);
                operationConverter.Write(updateOperation, writer);
            }
        }

        public UpsertRequest<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}