using System;

using MsgPack.Light;

using tarantool_client.IProto.Data;
using tarantool_client.IProto.Data.Packets;
using tarantool_client.IProto.Data.UpdateOperations;

namespace tarantool_client.IProto.Converters
{
    public class UpdatePacketConverter<T, TUpdate> : IMsgPackConverter<UpdatePacket<T, TUpdate>>
        where T : ITuple
    {
        private IMsgPackConverter<Header> _headerConverter;
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<T> _selectKeyConverter;
        private IMsgPackConverter<UpdateOperation<TUpdate>> _updateOperationConverter;

        public void Initialize(MsgPackContext context)
        {
            _headerConverter = context.GetConverter<Header>();
            _uintConverter = context.GetConverter<uint>();
            _keyConverter = context.GetConverter<Key>();
            _selectKeyConverter = context.GetConverter<T>();
            _updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();
        }

        public void Write(UpdatePacket<T, TUpdate> value, IMsgPackWriter writer)
        {
            _headerConverter.Write(value.Header, writer);

        
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

        public UpdatePacket<T, TUpdate> Read(IMsgPackReader readerr)
        {
            throw new NotImplementedException();
        }
    }
}