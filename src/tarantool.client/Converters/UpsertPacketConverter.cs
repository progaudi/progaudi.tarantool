using System;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Requests;
using Tarantool.Client.Model.UpdateOperations;

namespace Tarantool.Client.Converters
{
    public class UpsertPacketConverter<T, TUpdate> : IMsgPackConverter<UpsertRequest<T, TUpdate>>
        where T : ITuple
    {
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<T> _tupleConverter;
        private IMsgPackConverter<UpdateOperation<TUpdate>> _updateOperationConverter;

        public void Initialize(MsgPackContext context)
        {
            _uintConverter = context.GetConverter<uint>();
            _keyConverter = context.GetConverter<Key>();
            _tupleConverter = context.GetConverter<T>();
            _updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();
        }

        public void Write(UpsertRequest<T, TUpdate> value, IMsgPackWriter writer)
        {
            writer.WriteMapHeader(3);

            _keyConverter.Write(Key.SpaceId, writer);
            _uintConverter.Write(value.SpaceId, writer);

            _keyConverter.Write(Key.Tuple, writer);
            _tupleConverter.Write(value.Tuple, writer);

            _keyConverter.Write(Key.Ops, writer);
            writer.WriteArrayHeader(1);
            _updateOperationConverter.Write(value.UpdateOperation, writer);
        }

        public UpsertRequest<T, TUpdate> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}