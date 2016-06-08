using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class UpsertPacketConverter<T, TUpdate> : IMsgPackConverter<UpsertPacket<T, TUpdate>>
        where T : ITuple
    {
        private IMsgPackConverter<Header> _headerConverter;
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<T> _tupleConverter;
        private IMsgPackConverter<UpdateOperation<TUpdate>> _updateOperationConverter;

        public void Initialize(MsgPackContext context)
        {
            _headerConverter = context.GetConverter<Header>();
            _uintConverter = context.GetConverter<uint>();
            _keyConverter = context.GetConverter<Key>();
            _tupleConverter = context.GetConverter<T>();
            _updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();
        }

        public void Write(UpsertPacket<T, TUpdate> value, IMsgPackWriter writer)
        {
            _headerConverter.Write(value.Header, writer);

            
            writer.WriteMapHeader(3);

            _keyConverter.Write(Key.SpaceId, writer);
            _uintConverter.Write(value.SpaceId, writer);

            _keyConverter.Write(Key.Tuple, writer);
            _tupleConverter.Write(value.Tuple, writer);

            _keyConverter.Write(Key.Ops, writer);
            writer.WriteArrayHeader(1);
            _updateOperationConverter.Write(value.UpdateOperation, writer);
        }

        public UpsertPacket<T, TUpdate> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}