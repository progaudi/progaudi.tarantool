using System;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Requests;
using Tarantool.Client.Model.UpdateOperations;

namespace Tarantool.Client.Converters
{
    public class UpdatePacketConverter<T, TUpdate> : IMsgPackConverter<UpdateRequest<T, TUpdate>>
        where T : ITuple
    {
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<T> _selectKeyConverter;
        private IMsgPackConverter<UpdateOperation<TUpdate>> _updateOperationConverter;

        public void Initialize(MsgPackContext context)
        {
            _uintConverter = context.GetConverter<uint>();
            _keyConverter = context.GetConverter<Key>();
            _selectKeyConverter = context.GetConverter<T>();
            _updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();
        }

        public void Write(UpdateRequest<T, TUpdate> value, IMsgPackWriter writer)
        {
            writer.WriteMapHeader(4);

            _keyConverter.Write(Key.SpaceId, writer);
            _uintConverter.Write(value.SpaceId, writer);

            _keyConverter.Write(Key.IndexId, writer);
            _uintConverter.Write(value.IndexId, writer);

            _keyConverter.Write(Key.Key, writer);
            _selectKeyConverter.Write(value.Key, writer);

            _keyConverter.Write(Key.Tuple, writer);
            writer.WriteArrayHeader(1);
            _updateOperationConverter.Write(value.UpdateOperation, writer);
        }

        public UpdateRequest<T, TUpdate> Read(IMsgPackReader readerr)
        {
            throw new NotImplementedException();
        }
    }
}