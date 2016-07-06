using System;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Requests;

namespace Tarantool.Client.Converters
{
    internal class UpdatePacketConverter<T> : IMsgPackConverter<UpdateRequest<T>>
        where T : ITuple
    {
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<T> _selectKeyConverter;
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _uintConverter = context.GetConverter<uint>();
            _keyConverter = context.GetConverter<Key>();
            _selectKeyConverter = context.GetConverter<T>();
            _context = context;
        }

        public void Write(UpdateRequest<T> value, IMsgPackWriter writer)
        {
            writer.WriteMapHeader(4);

            _keyConverter.Write(Key.SpaceId, writer);
            _uintConverter.Write(value.SpaceId, writer);

            _keyConverter.Write(Key.IndexId, writer);
            _uintConverter.Write(value.IndexId, writer);

            _keyConverter.Write(Key.Key, writer);
            _selectKeyConverter.Write(value.Key, writer);

            _keyConverter.Write(Key.Tuple, writer);
            writer.WriteArrayHeader((uint) value.UpdateOperations.Length);

            foreach (var updateOperation in value.UpdateOperations)
            {
                var operationConverter = updateOperation.GetConverter(_context);
                operationConverter.Write(updateOperation, writer);
            }
        }

        public UpdateRequest<T> Read(IMsgPackReader readerr)
        {
            throw new NotImplementedException();
        }
    }
}